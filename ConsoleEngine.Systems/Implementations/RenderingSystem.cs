using ConsoleEngine.Services.AssetManagement;
using ConsoleEngine.Services.Factories;
using ConsoleEngine.Services.Repositories.Entity;
using ConsoleEngine.Services.Util.Rendering;
using ConsoleEngine.Services.Util.Scenes;
using DataModel.Components;
using DataModel.Interop.Kernel32;
using DataModel.Math.Structures;
using DataModel.Rendering;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ConsoleEngine.Systems.Implementations
{
    internal sealed class RenderingSystem : System, IRenderingSystem
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern SafeFileHandle CreateFile(
        string fileName,
        [MarshalAs(UnmanagedType.U4)] uint fileAccess,
        [MarshalAs(UnmanagedType.U4)] uint fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] int flags,
        IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        private readonly ISceneManagementService sceneManagementService;
        private readonly IGUIRenderingService guiRenderingService;
        private readonly IRendererRepositoryService rendererRepositoryService;
        private readonly ICameraRepositoryService cameraRepositoryService;

        private readonly SafeFileHandle handle;
        private CharInfo[] charBuffer;
        private float[] depthBuffer;
        private SmallRect bufferRect;
        private Vector2Int screenSize;

        public RenderingSystem(            
            ISceneManagementService sceneManagementService,
            ICameraRepositoryService cameraRepositoryService,
            IRendererRepositoryService rendererRepositoryService,
            IGUIRenderingService guiRenderingService)        
        {                        
            this.sceneManagementService = sceneManagementService;
            this.guiRenderingService = guiRenderingService;
            this.cameraRepositoryService = cameraRepositoryService;
            this.rendererRepositoryService = rendererRepositoryService;           
            handle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            UpdateCharBuffer();
        }        

        private void UpdateCharBuffer()
        {
            charBuffer = new CharInfo[screenSize.x * screenSize.y];
            depthBuffer = new float[screenSize.x];
            bufferRect = new SmallRect() { Left = 0, Top = 0, Right = (short)screenSize.x, Bottom = (short)screenSize.y };
        }        

        public override void LateUpdate()
        {
            if (handle.IsInvalid)
                return;
            var tempSize = new Vector2Int(Console.WindowWidth, Console.WindowHeight);
            if (tempSize != screenSize)
            {
                screenSize = tempSize;
                UpdateCharBuffer();
            }
            var scene = sceneManagementService.GetActiveScene();
            if(scene != null)
            {
                RenderCameras(scene);
            }            
            RenderGUI();
            WriteConsoleOutput(handle, charBuffer,
                     new Coord() { X = (short)screenSize.x, Y = (short)screenSize.y },
                     new Coord() { X = 0, Y = 0 },
                     ref bufferRect);
        }

        private void RenderCameras(Scene scene)
        {
            Terrain terrain = scene.terrain;
            for(int i = 0; i < cameraRepositoryService.Count; i++)
            {
                var camera = cameraRepositoryService.Get(i);
                var cameraComponent = camera.GetComponent<ICameraComponent>();
                var cameraTransform = camera.GetComponent<ITransformComponent>();
                RectInt view = new RectInt
                    (
                        (int)(cameraComponent.ViewPort.x * screenSize.x),
                        (int)(cameraComponent.ViewPort.y * screenSize.y),
                        (int)(cameraComponent.ViewPort.width * screenSize.x),
                        (int)(cameraComponent.ViewPort.height * screenSize.y)
                    );
                for (int x = view.x; x < view.x + view.width; x++)
                {
                    float angle = cameraTransform.Rotation - cameraComponent.FieldOfView / 2f + (x - view.x) / (float)screenSize.x * cameraComponent.FieldOfView;
                    float dist = 0f;
                    bool hitWall = false;
                    Material wallMaterial = null;
                    float farClip = Math.Min(cameraComponent.FarClippingDistance, 16);

                    Vector2 eyeVec = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
                    Vector2 uv = Vector2.Zero;
                    while (!hitWall && dist < farClip)
                    {
                        dist += 0.01f;

                        Vector2Int test = new Vector2Int
                            (
                                (int)(cameraTransform.Position.X + eyeVec.X * dist),
                                (int)(cameraTransform.Position.Y + eyeVec.Y * dist)
                            );                        
                        if (test.x < 0 || test.x >= 16 || test.y < 0 || test.y >= 11)
                        {
                            hitWall = true;
                            dist = farClip;
                        }
                        else if (terrain.walls[test.y * terrain.width + test.x] != null)
                        {
                            hitWall = true;
                            wallMaterial = terrain.walls[test.y * terrain.width + test.x];

                            Vector2 blockMid = new Vector2(test.x + 0.5f, test.y + 0.5f);
                            Vector2 testPoint = new Vector2
                                (
                                cameraTransform.Position.X + eyeVec.X * dist,
                                cameraTransform.Position.Y + eyeVec.Y * dist
                                );

                            float testAngle = (float)Math.Atan2(testPoint.Y - blockMid.Y, testPoint.X - blockMid.X);

                            if (testAngle >= -Math.PI * 0.25f && testAngle < Math.PI * 0.25f)
                            {
                                uv.X = testPoint.Y - test.y;
                            }
                            if (testAngle >= Math.PI * 0.25f && testAngle < Math.PI * 0.75f)
                            {
                                uv.X = testPoint.X - test.x;
                            }
                            if (testAngle < -Math.PI * 0.25f && testAngle >= -Math.PI * 0.75f)
                            {
                                uv.X = testPoint.X - test.x;
                            }
                            if (testAngle >= Math.PI * 0.75f || testAngle < -Math.PI * 0.75f)
                            {
                                uv.X = testPoint.Y - test.y;
                            }
                        }
                    }

                    int ceiling = (int)((view.height / 2f) - view.height / dist);
                    int floor = view.height - ceiling;
                    depthBuffer[x] = dist;
                    for (int y = view.y; y < view.y + view.height; y++)
                    {
                        int index = y * screenSize.x + x;
                        if (ceiling != 0 && y <= ceiling + view.y)
                        {
                            byte[] pix = null;
                            foreach (var layer in scene.skybox.layers)
                            {
                                float degrees = (angle + layer.rotation) * (180f / (float)Math.PI);
                                degrees %= 360;
                                if (degrees < 0)
                                    degrees += 360;
                                float skyX = degrees / 360f;
                                float skyY = (y - view.y) / (view.height / 2f);
                                var skyColor = layer.texture.SamplePixel(new Vector2(skyX, skyY));
                                if (skyColor == null || skyColor[0] == ' ' || skyColor[1] == 0)
                                    continue;
                                pix = skyColor;
                                break;
                            }
                            if (pix != null)
                            {
                                charBuffer[index].Attributes = pix[1];
                                charBuffer[index].Char.AsciiChar = pix[0];
                            }
                            else
                            {
                                charBuffer[index].Attributes = 0;
                                charBuffer[index].Char.AsciiChar = 0;
                            }

                        }
                        else if (y > ceiling + view.y && y <= floor + view.y)
                        {
                            uv.Y = (y - (float)(ceiling + view.y)) / ((floor + view.y + 1) - (float)(ceiling + view.y));

                            if (wallMaterial != null)
                            {

                                var pixel = wallMaterial.SamplePixel(uv);
                                if (pixel != null)
                                {
                                    charBuffer[index].Attributes = pixel[1];
                                    charBuffer[index].Char.AsciiChar = pixel[0];
                                }
                            }
                        }
                        else
                        {
                            byte[] floorShade = GetFloorShade(y - view.y, view.height, cameraTransform.Position, eyeVec, dist, view.height - floor);
                            charBuffer[index].Attributes = floorShade[1];
                            charBuffer[index].Char.AsciiChar = floorShade[0];
                        }
                    }
                }
                Dictionary<Vector2Int, float> depthBufferMap = new Dictionary<Vector2Int, float>();
                for(int j = 0; j < rendererRepositoryService.Count; j++)
                {
                    var renderer = rendererRepositoryService.Get(j);
                    var rendererTransform = renderer.GetComponent<ITransformComponent>();
                    var rendererComponent = renderer.GetComponent<IRendererComponent>();
                    var dist = Vector2.Distance(cameraTransform.Position, rendererTransform.Position);
                    var dir = rendererTransform.Position - cameraTransform.Position;
                    Vector2 eye = new Vector2((float)Math.Sin(cameraTransform.Rotation), (float)Math.Cos(cameraTransform.Rotation));

                    float angleDiff = (float)(Math.Atan2(eye.Y, eye.X) - Math.Atan2(dir.Y, dir.X));
                    if (angleDiff < -Math.PI)
                        angleDiff += 2f * (float)Math.PI;
                    if (angleDiff > Math.PI)
                        angleDiff -= 2f * (float)Math.PI;

                    if (Math.Abs(angleDiff) >= cameraComponent.FieldOfView)
                        continue;
                    if (dist < cameraComponent.NearClippingDistance)
                        continue;
                    if (dist > cameraComponent.FarClippingDistance)
                        continue;
                    float objectCeiling = (int)((view.height / 2f) - view.height / dist);
                    float objectFloor = view.height - objectCeiling;
                    float objectHeight = objectFloor - objectCeiling;
                    float objectAspectRatio = rendererComponent.Material.texture.height / (float)rendererComponent.Material.texture.width;
                    float objectWidth = objectHeight / objectAspectRatio;
                    float middle = view.x + (0.5f * (angleDiff / (cameraComponent.FieldOfView / 2f)) + 0.5f) * view.width;

                    for (float lx = 0; lx < objectWidth; lx++)
                    {
                        for (float ly = 0; ly < objectHeight; ly++)
                        {
                            Vector2 uv = new Vector2(lx / objectWidth, ly / objectHeight);
                            var pixel = rendererComponent.Material.texture.SamplePixel(uv);
                            if (pixel == null)
                                continue;
                            Vector2Int objectPos = new Vector2Int
                                (
                                    (int)(middle + lx - (objectWidth / 2f)),
                                    (int)(objectCeiling + ly)
                                );
                            if (objectPos.x < view.x)
                                continue;
                            if (objectPos.x >= view.x + view.width)
                                continue;
                            if (objectPos.y < 0)
                                continue;
                            if (objectPos.y >= view.y + view.height)
                                continue;
                            if (rendererComponent.Material.texture.isTransparent && pixel[0] == 0 || pixel[1] == 0)
                                continue;
                            if (depthBuffer[objectPos.x] < dist)
                                continue;
                            if (!depthBufferMap.ContainsKey(objectPos))
                            {
                                depthBufferMap.Add(objectPos, dist);
                            }
                            if (dist > depthBufferMap[objectPos])
                                continue;

                            int index = objectPos.y * screenSize.x + objectPos.x;
                            charBuffer[index].Attributes = pixel[1];
                            charBuffer[index].Char.AsciiChar = pixel[0];
                            depthBufferMap[objectPos] = dist;
                        }
                    }
                }                
            }
        }

        private byte[] GetFloorShade(int y, int viewHeight, Vector2 position, Vector2 direction, float hitDistance, int floorLines)
        {
            var terrain = sceneManagementService.GetActiveScene().terrain;
            var b = Math.Abs((y - viewHeight) / (float)floorLines);
            hitDistance *= b;
            Vector2 terrainLocation = position + direction * hitDistance;
            byte color = 0;
            Vector2Int test = new Vector2Int((int)terrainLocation.X, (int)terrainLocation.Y);
            if (test.x < 0 || test.x >= terrain.width || test.y < 0 || test.y >= terrain.height)
            {
                color = 0;
            }
            else
            {
                color = terrain.floor[test.y * 16 + test.x];
            }            

            if (b < 0.25f)
            {
                return new byte[] { (byte)'#', color };
            }
            else if (b < 0.5f)
            {
                return new byte[] { (byte)'x', color };
            }
            else if (b < 0.75f)
            {
                return new byte[] { (byte)'-', color };
            }
            else if (b < 1f)
            {
                return new byte[] { (byte)'.', color };
            }
            return new byte[] { (byte)' ', 0 };

        }

        private void RenderGUI()
        {
            GUIEntry entry = guiRenderingService.GetGetNextGUIEntry();
            while (entry != null)
            {
                RenderGUIEntry(entry);
                entry = guiRenderingService.GetGetNextGUIEntry();
            }
        }

        private void RenderGUIEntry(GUIEntry entry)
        {
            for (int x = 0; x < entry.rect.width; x++)
            {
                for (int y = 0; y < entry.rect.height; y++)
                {
                    int rectIndex = y * entry.rect.width + x;
                    if (rectIndex >= entry.text.Length)
                        continue;
                    int screenIndex = (entry.rect.y + y) * screenSize.x + entry.rect.x + x;                    
                    if (entry.text[rectIndex] == ' ' && entry.blackIsTransparency)
                        continue;
                    charBuffer[screenIndex].Attributes = (short)entry.color;
                    charBuffer[screenIndex].Char.UnicodeChar = entry.text[rectIndex];
                    charBuffer[screenIndex].Char.AsciiChar = (byte)entry.text[rectIndex];
                }
            }
        }
    }
}

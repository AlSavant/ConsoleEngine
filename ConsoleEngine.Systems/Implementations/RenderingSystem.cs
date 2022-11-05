using ConsoleEngine.Services.Util.Rendering;
using DataModel.Interop.Kernel32;
using DataModel.Math.Structures;
using DataModel.Rendering;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
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

        private readonly IGUIRenderingService guiRenderingService;
        private readonly SafeFileHandle handle;
        private CharInfo[] charBuffer;
        private float[] depthBuffer;
        private SmallRect bufferRect;
        private Vector2Int screenSize;

        public RenderingSystem(IGUIRenderingService guiRenderingService)        
        {
            this.guiRenderingService = guiRenderingService;
            handle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            UpdateCharBuffer();
        }

        private void UpdateCharBuffer()
        {
            charBuffer = new CharInfo[screenSize.x * screenSize.y];
            depthBuffer = new float[screenSize.x];
            bufferRect = new SmallRect() { Left = 0, Top = 0, Right = (short)screenSize.x, Bottom = (short)screenSize.y };
        }

        public override void Update()
        {
            guiRenderingService.PrintText(new Vector2Int(10,10), "Hello world!", ConsoleColor.Green);
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
            RenderGUI();
            WriteConsoleOutput(handle, charBuffer,
                     new Coord() { X = (short)screenSize.x, Y = (short)screenSize.y },
                     new Coord() { X = 0, Y = 0 },
                     ref bufferRect);
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

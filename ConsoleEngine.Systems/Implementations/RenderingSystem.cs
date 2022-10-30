using DataModel.Interop.Kernel32;
using DataModel.Math.Structures;
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

        private readonly SafeFileHandle handle;
        private CharInfo[] charBuffer;
        private float[] depthBuffer;
        private SmallRect bufferRect;
        private Vector2Int screenSize;

        public RenderingSystem()
        {
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
            if (handle.IsInvalid)
                return;
        }
    }
}

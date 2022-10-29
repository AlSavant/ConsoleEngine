﻿using System.Runtime.InteropServices;

namespace DataModel.Interop.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
}

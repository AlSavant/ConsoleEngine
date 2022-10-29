using System.Runtime.InteropServices;

namespace DataModel.Interop.Kernel32
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CharInfo
    {
        [FieldOffset(0)] public CharUnion Char;
        [FieldOffset(2)] public short Attributes;
    }
}

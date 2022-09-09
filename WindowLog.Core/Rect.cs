using System.Runtime.InteropServices;

namespace WindowLog.Core;

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    public int left;
    public int top;
    public int right;
    public int bottom;
}
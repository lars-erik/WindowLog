using System.Runtime.InteropServices;

namespace WindowLog.Core;

[StructLayout(LayoutKind.Sequential)]
public struct GUIThreadInfo
{
    public int cbSize;
    public uint flags;
    public IntPtr hwndActive;
    public IntPtr hwndFocus;
    public IntPtr hwndCapture;
    public IntPtr hwndMenuOwner;
    public IntPtr hwndMoveSize;
    public IntPtr hwndCaret;
    public Rect rcCaret;
}
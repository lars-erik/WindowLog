using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowLog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var watcher = new WindowWatcher();
            while(true)
            {
                watcher.TryIt();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }

    public class WindowWatcher
    {
        [DllImport("User32.dll")]
        static extern bool GetGUIThreadInfo(IntPtr idThread, ref GUIThreadInfo lpGui);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static string GetText(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        private string current = null;

        public void TryIt()
        {
            var info = new GUIThreadInfo();
            info.cbSize = Marshal.SizeOf(info);
            var ok = GetGUIThreadInfo(IntPtr.Zero, ref info);
            if (ok)
            {
                var result = GetText(info.hwndActive);
                if (result != current)
                {
                    current = result;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + current);
                }
            }
            else
            {
                if (current != "None")
                {
                    current = "None";
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + current);
                }
            }
        }
    }

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

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}


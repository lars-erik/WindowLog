using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowLog.Core;

public class WindowWatcher
{
    public DateTime Start { get; private set; } = DateTime.MinValue;
    public string Title { get; private set; } = "";
    public long PID { get; private set; } = 0;
    public string Executable { get; private set; } = "";

    public bool Update()
    {
        var info = new GUIThreadInfo();
        info.cbSize = Marshal.SizeOf(info);
        var ok = GetGUIThreadInfo(IntPtr.Zero, ref info);
        if (ok)
        {
            GetWindowThreadProcessId(info.hwndActive, out var pid);
            var result = GetText(info.hwndActive);
            if (result == "") result = "Windows";
            if (result != Title)
            {
                PID = pid;
                var process = Process.GetProcessById((int)pid);
                Executable = process.ProcessName;
                Title = result;
                Start = DateTime.Now;
                return true;
            }
        }
        else
        {
            if (Title != "Unknown")
            {
                PID = 0;
                Executable = "Unknown";
                Title = "Unknown";
                Start = DateTime.Now;
                return true;
            }
        }

        return false;
    }

    public static string GetText(IntPtr hWnd)
    {
        // Allocate correct string length first
        int length = GetWindowTextLength(hWnd);
        StringBuilder sb = new StringBuilder(length + 1);
        GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }


    [DllImport("user32.dll")]
    static extern IntPtr GetWindowModuleFileName(IntPtr hWnd, StringBuilder text, uint count);

    [DllImport("User32.dll")]
    static extern bool GetGUIThreadInfo(IntPtr idThread, ref GUIThreadInfo lpGui);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}
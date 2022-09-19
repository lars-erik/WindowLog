namespace WindowLog.Core;

public class Entry
{
    public long PID { get; set; }
    public string Executable { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public TimeSpan Duration => (End ?? DateTime.Now) - Start;

    public override string ToString()
    {
        return "[" + PID + "] " + Title + 
               " (" + Executable.Substring(Math.Max(0, Executable.LastIndexOf("\\", StringComparison.CurrentCultureIgnoreCase))) + ")";
    }
}
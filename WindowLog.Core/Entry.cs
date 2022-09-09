namespace WindowLog.Core;

public class Entry
{
    public long PID { get; set; }
    public string Executable { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public TimeSpan Duration => (End ?? DateTime.Now) - Start;
}
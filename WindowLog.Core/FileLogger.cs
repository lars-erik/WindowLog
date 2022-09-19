using System.Globalization;

namespace WindowLog.Core
{
    public class FileLogger : WindowLogger
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        private string rootPath = "";

        public FileLogger()
        {
            EnsureRootDir();
            LoadExistingEntries();

            EntryComplete += AppendEntry;
        }

        private void LoadExistingEntries()
        {
            if (File.Exists(LogPath))
            {
                var lines = File.ReadAllLines(LogPath);
                foreach (var line in lines.Select(x => x.Split(';')))
                {
                    Entries.Add(new Entry
                    {
                        PID = Convert.ToInt64(line[0]),
                        Executable = line[1],
                        Title = line[2],
                        Start = DateTime.Parse(line[3]),
                        End = line[4] == "" ? null : DateTime.Parse(line[4])
                    });
                }
            }
        }

        private string RootPath =>
            rootPath == ""
                ? rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WindowLog")
                : rootPath;

        private string LogPath => Path.Combine(RootPath, $"WindowLog-{DateTime.Now:yyyy-MM-dd}.log");

        private void EnsureRootDir()
        {
            if (!Directory.Exists(RootPath))
            {
                Directory.CreateDirectory(RootPath);
            }
        }

        private void AppendEntry(object? sender, Entry e)
        {
            try
            {
                File.AppendAllText(
                    LogPath,
                    String.Join(
                        ";",
                        e.PID,
                        e.Executable.Replace(";", "-"),
                        e.Title.Replace(";", "-"),
                        e.Start.ToString("s", Culture),
                        e.End?.ToString("s", Culture) ?? ""
                    ) + Environment.NewLine
                );
            }
            catch
            {
                throw;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowLog.Core
{
    public class FileLogger : WindowLogger
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        private readonly string logPath;

        public FileLogger()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var rootPath = Path.Combine(appDataPath, "WindowLog");
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            var fileName = $"WindowLog-{DateTime.Now:yyyy-MM-dd}.log";
            logPath = Path.Combine(rootPath, fileName);

            if (File.Exists(logPath))
            {
                var lines = File.ReadAllLines(logPath);
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

            EntryComplete += AppendEntry;
        }

        private void AppendEntry(object? sender, Entry e)
        {
            try
            {
                File.AppendAllText(
                    logPath,
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowLog.Core
{
    public class WindowLogger
    {
        private readonly object lockObj = new object();
        private WindowWatcher watcher = new WindowWatcher();
        private List<Entry> entries = new List<Entry>();
        public IList<Entry> Entries => entries;
        public Entry Current => entries.Any() ? entries[^1] : new Entry();

        public bool Update()
        {
            if (watcher.Update())
            {
                lock (lockObj)
                {
                    Debug.WriteLine("Checking for PID " + watcher.PID + " entry count " + entries.Count);
                    if (entries.Any())
                    {
                        entries[^1].End = DateTime.Now;
                    }
                }
                var entry = new Entry
                {
                    PID = watcher.PID,
                    Executable = watcher.Executable,
                    Title = watcher.Title,
                    Start = watcher.Start
                };
                lock (lockObj)
                {
                    entries.Add(entry);
                }
                return true;
            }

            return false;
        }

    }
}

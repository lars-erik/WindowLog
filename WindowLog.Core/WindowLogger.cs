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
        public event EventHandler<Entry>? EntryComplete;

        public bool Update()
        {
            if (watcher.Update())
            {
                var entryComplete = false;
                lock (lockObj)
                {
                    if (entries.Any() && entries[^1].End == null)
                    {
                        entries[^1].End = DateTime.Now;
                        entryComplete = true;
                    }
                }

                if (entryComplete)
                {
                    EntryComplete?.Invoke(this, entries[^1]);
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

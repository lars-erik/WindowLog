using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowLog.Core;

namespace WindowLog.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ViewModel ViewModel { get; } = new ViewModel();

        private BackgroundWorker worker;
        private WindowLogger logger;

        protected override void OnStartup(StartupEventArgs e)
        {
            logger = new WindowLogger();
            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            worker.DoWork += (sender, args) =>
            {
                while(true) {
                    var changed = logger.Update();
                    worker.ReportProgress(0, changed);
                    Thread.Sleep(100);
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                }
            };
            worker.ProgressChanged += (sender, args) =>
            {
                var changed = true.Equals(args.UserState);
                if (changed)
                {
                    if (ViewModel.Entries.Any())
                    {
                        ViewModel.Entries.OrderByDescending(x => x.Entry.Start).First().Entry.End = logger.Entries.OrderByDescending(x => x.Start).Skip(1).First().End;
                    }
                    ViewModel.Entries.Add(new EntryModel{ Entry = logger.Current });
                    ViewModel.Current = new EntryModel { Entry = logger.Current };
                }
                if (ViewModel.Entries.Any())
                {
                    foreach(var entry in ViewModel.Entries.OrderByDescending(x => x.Entry.Start).Take(3))
                    {
                        entry.NotifyChange();
                    }
                }
            };
            worker.RunWorkerAsync();

            base.OnStartup(e);
        }
    }
}

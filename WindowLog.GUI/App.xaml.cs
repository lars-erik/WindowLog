using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
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
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            logger = new FileLogger();
            foreach (var entry in logger.Entries)
            {
                ViewModel.Entries.Add(new EntryModel(entry));
            }
            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            worker.DoWork += (sender, args) =>
            {
                while(true) {
                    Thread.Sleep(100);
                    var changed = logger.Update();
                    worker.ReportProgress(0, changed);
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
                    if (ViewModel.Current.Entry != null)
                    {
                        ViewModel.Current.Entry.End = logger.Current.Start;
                        ViewModel.Current.NotifyChange();
                    }

                    var newModel = new EntryModel(logger.Current);
                    ViewModel.Entries.Add(newModel);
                    ViewModel.Current = newModel;
                }
                ViewModel.Current.NotifyChange();
            };
            worker.RunWorkerAsync();

            base.OnStartup(e);
        }
    }
}

using System;
using System.Drawing;
using WindowLog.Core;

namespace WindowLog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var watcher = new WindowWatcher();
            while(true)
            {
                if (watcher.Update())
                {
                    Console.WriteLine(watcher.Start.ToString("yyyy-MM-dd HH:mm:ss") + ": " + watcher.Title);
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}


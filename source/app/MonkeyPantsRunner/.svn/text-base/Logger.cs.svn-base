using System;
using System.IO;

namespace MonkeyPants
{
    // a junky little thing to dump info somewhere
    internal static class Logger
    {
        public static void Log(Exception exception)
        {
            Dump(exception);
        }

        public static void Log(string message)
        {
            Dump(message);
        }

        private static void Dump(object x)
        {
            string logFile = Path.Combine(Environment.CurrentDirectory, "MonkeyPants.log");
            using(TextWriter tw = File.AppendText(logFile))
            {
                tw.WriteLine(x);                
            }
        }
    }
}
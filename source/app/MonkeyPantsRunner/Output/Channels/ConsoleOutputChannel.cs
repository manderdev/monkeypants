using System;
using System.IO;

namespace MonkeyPants.Output.Channels
{
    public class ConsoleOutputChannel : IOutputChannel
    {
        private readonly TextWriter writer;

        public ConsoleOutputChannel()
        {
            writer = Console.Out;
        }

        public void Open(Action<TextWriter> accept)
        {
            accept(writer);
        }

        public void Close() { }

        public void Append(string s)
        {            
            writer.Write(s);
        }

        public void AppendFailure(string s)
        {
            using (ConsoleColoring.Factory.Failure)
            {
                writer.Write(s);
            }
        }

        public void AppendError(string s)
        {
            using (ConsoleColoring.Factory.Error)
            {
                writer.Write(s);
            }
        }

        public void AppendLine(string s)
        {
            writer.WriteLine(s);
        }

        public void AppendFailureLine(string s)
        {
            using (ConsoleColoring.Factory.Failure)
            {
                Console.Error.WriteLine(s);
            }
        }

        public void AppendErrorLine(string s)
        {
            using (ConsoleColoring.Factory.Error)
            {
                Console.Error.WriteLine(s);
            }
        }
    }
}
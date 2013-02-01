using System;
using System.IO;

namespace MonkeyPants.Output.Channels
{
    public class StringOutputChannel : IOutputChannel
    {
        private readonly StringWriter writer;

        public StringOutputChannel()
        {
            writer = new StringWriter();
        }

        public void Open(Action<TextWriter> accept)
        {
            accept(writer);
        }

        public void Close()
        {
            writer.WriteLine();
        }

        public void Append(string s)
        {
            writer.Write(s);               
        }

        public void AppendFailure(string s)
        {
            Append(s);
        }

        public void AppendError(string s)
        {
            Append(s);
        }

        public void AppendLine(string s)
        {
            writer.WriteLine(s);               
        }

        public void AppendFailureLine(string s)
        {
            AppendLine(s);
        }

        public void AppendErrorLine(string s)
        {
            AppendLine(s);
        }

        public string Content
        {
            get { return writer.ToString(); }
        }
    }
}
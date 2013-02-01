using System;
using System.IO;

namespace MonkeyPants.Output.Channels
{
    public class FileOutputChannel : IOutputChannel
    {
        private readonly string file;
        private StreamWriter writer;

        public FileOutputChannel(string file)
        {
            this.file = file;
        }

        public void Open(Action<TextWriter> accept)
        {
            FileStream stream = File.OpenWrite(file);
            writer = new StreamWriter(stream);
            accept(writer);
        }

        public void Close()
        {
            writer.Flush();
            writer.Close();
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
    }
}
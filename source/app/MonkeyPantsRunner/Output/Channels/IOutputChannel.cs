using System;
using System.IO;

namespace MonkeyPants.Output.Channels
{
    public interface IOutputChannel
    {
        void Open(Action<TextWriter> accept);

        void Close();

        void Append(string s);
        void AppendFailure(string s);
        void AppendError(string s);

        void AppendLine(string s);
        void AppendFailureLine(string s);
        void AppendErrorLine(string s);
    }
}
using MonkeyPants.Execution;
using MonkeyPants.Output;
using MonkeyPants.Output.Channels;

namespace MonkeyPants.Configuration
{
    public class OutputConfiguration
    {
        public string OutputWriter { get; set; }
        public string OutputWriterAssembly { get; set; }
        public ChannelConfiguration Channel { get; set; }

        public IResultsWriter LoadWriter()
        {
            IOutputChannel channel = Channel.LoadChannel();
            return Assemblies.Instantiate<IResultsWriter>(OutputWriterAssembly, OutputWriter, channel);
        }
    }
}
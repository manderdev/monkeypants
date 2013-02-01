using MonkeyPants.Output.Channels;

namespace MonkeyPants.Configuration
{
    public class ChannelConfiguration
    {
        public string Type { get; set; }
        public string File { get; set; }

        public IOutputChannel LoadChannel()
        {
            // todo: validation
            // todo: support custom channels - even this mapping could be configurable
            if ("file".Equals(Type.ToLower()))
            {
                return new FileOutputChannel(File);
            }
            else if ("console".Equals(Type.ToLower()))
            {
                return new ConsoleOutputChannel();
            }
            throw new MonkeyPantsApplicationException("unsupported channel in config: " + Type);
        }
    }
}
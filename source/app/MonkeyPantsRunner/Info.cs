using System;
using System.IO;
using MonkeyPants.Configuration;

namespace MonkeyPants
{
    public class Info
    {
        public static void Usage()
        {
            WriteLine("usage: MonkeyPants {directory|file}");
        }
        
        public static void Intro()
        {
            WriteLine();
            WriteLine("MONKEY PANTS (C) 2010 Martin Andersen, John Finlay");
            WriteLine();
        }

        public static void Echo(string projectConfigurationFile, ProjectConfiguration projectConfiguration)
        {
            // WriteLine(" ") instead of WriteLine() so that it formats the same in a nant exec also (which otherwise skips blank lines)
            WriteLine("using:");
            WriteLine("   config: " + Path.GetFullPath(projectConfigurationFile));

            WriteLine();
            WriteLine("   outputs: ");
            foreach (OutputConfiguration output in projectConfiguration.Outputs)
            {
                Format("      {0}", output.OutputWriter);
                Format("         in assembly {0}", output.OutputWriterAssembly);
                Format("         writing to {0} {1}", output.Channel.Type, output.Channel.File);
                WriteLine();
            }

            WriteLine(" ");
            WriteLine("   fixture assemblies: ");
            foreach (string assembly in projectConfiguration.FixtureAssemblies)
            {
                WriteLine("      " + assembly);
            }
            WriteLine(" ");
            WriteLine("----------");
            WriteLine(" ");
        }

        private static void Format(string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }

        public static void Exception(Exception ex)
        {
            WriteLine();
            Console.WriteLine(ex);
        }

        /// <remarks>
        /// Use WriteLine(" ") instead of WriteLine() so that it formats the same in a nant exec also (which otherwise skips blank lines)
        /// </remarks>
        private static void WriteLine()
        {
            Console.WriteLine(" ");
        }

        private static void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}
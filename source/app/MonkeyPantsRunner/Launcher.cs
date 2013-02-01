using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonkeyPants.Configuration;
using MonkeyPants.Execution;
using MonkeyPants.Locating;

namespace MonkeyPants
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            if (!ValidArgs(args))
            {
                Info.Usage();
                return;
            }

            try
            {
                Debugger.Launch();
                Launch(args);
            }
            catch (Exception ex)
            {
                Info.Exception(ex);
                Logger.Log(ex);
            }

            if (ShouldWaitToClose(args))
            {
                Console.ReadKey();
            }
        }

        // todo: firm up
        private static bool ValidArgs(string[] args)
        {
            return args.Length == 1 || args.Length == 2;
        }

        private static void Launch(string[] args)
        {
            Info.Intro();
            string inputPath = args[0];

            string configurationFile = new ProjectConfigurationLocater(inputPath, "*monkeyPants.config").Locate();
            ProjectConfiguration configuration = ProjectConfiguration.Load(configurationFile);

            Info.Echo(configurationFile, configuration);

            Session session = configuration.CreateSession();
            session.Run(inputPath);
        }

        private static bool ShouldWaitToClose(string[] args)
        {
            return !new List<string>(args).Contains("nowait");
        }
    }
}
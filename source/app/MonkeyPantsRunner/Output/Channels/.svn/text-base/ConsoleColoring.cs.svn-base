using System;

namespace MonkeyPants.Output.Channels
{
    public class ConsoleColoring
    {
        public static class Colors
        {
            public static ConsoleColor Failure = ConsoleColor.DarkRed;
            public static ConsoleColor Error = ConsoleColor.DarkYellow;
        }

        public static class Factory
        {
            public static Scope FailureOtherwiseError(bool isit)
            {
                return isit ? Failure : Error;
            }

            public static Scope Error
            {
                get { return new Scope(ConsoleColor.Yellow); }
            }

            public static Scope Failure
            {
                get { return new Scope(ConsoleColor.Red); }
            }
        }

        public class Scope : IDisposable
        {
            private readonly ConsoleColor originalColor;

            public Scope(ConsoleColor newForegroundColor)
            {
                originalColor = Console.ForegroundColor;
                Console.ForegroundColor = newForegroundColor;
            }

            public void Dispose()
            {
                Console.ForegroundColor = originalColor;
            }
        }
    }
}
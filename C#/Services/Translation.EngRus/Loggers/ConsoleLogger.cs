using System;

namespace Translation.EngRus.Logger {
    public class ConsoleLogger : ILogger {
        public void Log(string text) {
            Console.WriteLine(text);
        }
    }
}
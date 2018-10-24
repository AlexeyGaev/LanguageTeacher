using System;

namespace Translation.EngRus {
    class ConsoleLogger : ILogger {
        public void Log(string text) {
            Console.WriteLine(text);
        }
    }
}
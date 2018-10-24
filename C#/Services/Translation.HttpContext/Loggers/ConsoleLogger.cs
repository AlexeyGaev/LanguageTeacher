using System;

namespace Translation.Http.Logger {
    public class ConsoleLogger : ILogger<string> {
        public void Log(string text) {
            Console.WriteLine(text);
        }
    }
}
using Nancy;
using System;

namespace Translation.EngRus {
    // Test module
    public class HelloModule : NancyModule {
        public HelloModule() {
            Get("/", a => DateTime.UtcNow);
        }
    }
}

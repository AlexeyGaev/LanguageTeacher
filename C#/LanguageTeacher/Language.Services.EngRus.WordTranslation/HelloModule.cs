using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Language.Services.EngRus.WordTranslation {
    // Test module
    public class HelloModule : NancyModule {
        public HelloModule() {
            Get("/", _ => DateTime.UtcNow);
        }
    }
}

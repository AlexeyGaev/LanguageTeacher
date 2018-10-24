using Nancy;
using System;
using Translation.EngRus.Model;
using Nancy.ModelBinding;

namespace Translation.EngRus {
    public class AddTranslationModule : NancyModule {
        public AddTranslationModule(ITranslationStorage translations, IEventStorage events) 
            : base("/translation/Add") {
        }

        //public HelloModule() {
        //    Get("/", a => DateTime.UtcNow);
        //}
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Translation.Http.Converter;
using Translation.Http.Logger;

namespace Translation.Http {
    public class Startup {
        ILogger<string> consoleLogger = new ConsoleLogger();
        IToStringConverter<HttpContext> converter = new HttpContextConverter();

        public void ConfigureServices(IServiceCollection services) {
        }

        public void Configure(IApplicationBuilder application, IHostingEnvironment environment) {
            if (environment.IsDevelopment()) {
                application.UseDeveloperExceptionPage();
            }

            application.Run(async context => {
                await GetTask(context);
            });
        }

        Task GetTask(HttpContext context) {
            consoleLogger.Log("Got request");
            return context.Response.WriteAsync(converter.Convert(context));
        }
    }
}

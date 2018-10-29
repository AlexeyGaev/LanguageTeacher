using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Translation.Http {
    public class Startup {
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
            Logger.ConsoleLog("Got request");
            return context.Response.WriteAsync(PropertyTreeExporter.Convert(context));
            //return context.Response.WriteAsync(Converter.Convert(String.Empty, context));
        }
    }
}

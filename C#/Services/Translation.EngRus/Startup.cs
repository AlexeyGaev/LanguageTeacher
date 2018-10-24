using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using System;
using System.Threading.Tasks;
using Translation.EngRus.Logger;

namespace Translation.EngRus {
    public class Startup {
        ConsoleLogger consoleLogger = new ConsoleLogger();
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder application, IHostingEnvironment environment) {
            //app.UseOwin(buildFunc => {
            //    buildFunc(next => env => {
            //        logger.Log("Got request");
            //        return next(env);
            //    });
            //    buildFunc.UseNancy();
            //});
            if (environment.IsDevelopment()) {
                application.UseDeveloperExceptionPage();
            }

            application.Run(async context => {
                await GetTask(context);
            });
        }

        Task GetTask(HttpContext context) {
            consoleLogger.Log("Got request");
            return context.Response.WriteAsync("!!!");
        }
    }
}

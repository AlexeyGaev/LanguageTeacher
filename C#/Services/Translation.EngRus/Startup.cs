using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;

namespace Translation.EngRus {
    public class Startup {
        readonly ConsoleLogger logger = new ConsoleLogger();
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment e) {
            app.UseOwin(buildFunc => {
                buildFunc(next => env => {
                    logger.Log("Got request");
                    return next(env);
                });
                buildFunc.UseNancy();
            });
            //if (env.IsDevelopment()) {
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) => {
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Reflection.Utils.PropertyTree;

namespace Reflection.Services {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) => {
                //PropertyField field = new PropertyField(new object(), typeof(object));
                //string info = PropertyStringSerializer.Serialize("PropertyField", typeof(PropertyField), field);
                //string info = PropertyStringSerializer.Serialize("Object", typeof(object), new object());
                string info = PropertyStringSerializer.Serialize("HttpContext", typeof(HttpContext), context);
                await context.Response.WriteAsync(info);
            });
        }
    }
}

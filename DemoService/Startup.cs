using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;

using DemoService.Configuration;
using DemoService.Data;

namespace DemoService
{
    /// <summary>
    /// startup service
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services">services collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // set up dependency injection concrete services
            services.AddTransient<IDataProcessor, CouchbaseProcessor>();
            services.AddTransient<IDataClient, CouchbaseDataClient>();

            services.AddCors();
            services.AddMvc();

            // register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "DemoService",
                    Version = "v1",
                    Description = "Service for CouchBase Demo"
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "DemoService.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">the application</param>
        /// <param name="lifetime">application lifetime</param>
        /// <param name="env">the application environment</param>
        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, IHostingEnvironment env)
        {
            lifetime.ApplicationStopping.Register(OnShutdown);

            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials());

            app.UseStaticFiles();

            // set up the couchbase ClusterHelper and configuration
            CouchbaseConfigManager.Instance.Initialize();

            // enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "help";

                c.InjectStylesheet("/styles/custom.css");

                //c.DocumentTitle = "Concord";  // doesn't work yet

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoService V1");
            });

            app.UseMvc();
        }

        private void OnShutdown()
        {
            CouchbaseConfigManager.Instance.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Course.CORS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {   
                //allow all site
                /*
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
                */

                options.AddPolicy("AllowSites", builder =>
                {
                    builder.WithOrigins("https://localhost:44355", "https://anywebsites.com", "etc.")
                        .AllowAnyHeader().AllowAnyMethod();
                });

                //add new rule
                options.AddPolicy("AllowSites2", builder =>
                {
                    builder.WithOrigins("https://www.mysites.com")
                        .WithHeaders(HeaderNames.ContentType, "x-custom-header");
                });
                //allow subdomains
                options.AddPolicy("AllowSites3", builder =>
                {
                    builder.WithOrigins("https://*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader().AllowAnyMethod();

                });
                //allow specific methods
                options.AddPolicy("AllowSites4", builder =>
                {
                    builder.WithOrigins("https://www.mysites2.com").WithMethods("POST", "GET").AllowAnyHeader();

                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            //cors must be in under routing and above authorization
            //for allow all sites
            //app.UseCors();
            //just allowSites
            app.UseCors("AllowSites");
            app.UseCors("AllowSites2");
            app.UseCors("AllowSites3");
            app.UseCors("AllowSites4");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

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
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using KeyCrawler.WebApi.V1.Requests;
using KeyCrawler.Service.Services;
using KeyCrawler.Service.Utils;
using KeyCrawler.Persistence.Repositories;
using System.Net.Http;
using Hangfire;
using Hangfire.PostgreSql;
using KeyCrawler.Persistence;
using KeyCrawler.Persistence.Extentions;

namespace KeyCrawler.WebApi
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
            services.AddApiVersioning(x =>  
            {  
                x.DefaultApiVersion = new ApiVersion(1, 0);  
                x.AssumeDefaultVersionWhenUnspecified = true;  
                x.ReportApiVersions = true;  
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            }); 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KeyCrawler.WebApi", Version = "v1" });
            });

            services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(Configuration.GetConnectionString("Hangfire")));
            services.AddHangfireServer();

            services.AddMvc().AddFluentValidation();

            services.AddSingleton<HttpClient>();

            services.AddScoped<IUriReportRepository, UriReportRepository>();
            services.AddScoped<IPageFetcher, PageFetcher>();
            services.AddScoped<ISearchService, SearchService>();

            services.AddTransient<IValidator<SearchRequest>, SearchRequestValidator>();

            services.AddPersistanceLayer(Configuration.GetConnectionString("KeyCrawler"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.EnvironmentName.ToLower() == "local")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeyCrawler.WebApi v1");
                });
                app.UseHangfireDashboard();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            MigrateDb(app);
        }

        private void MigrateDb(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<KeyCrawlerContext>();
            db.Migrate();
        }
    }
}

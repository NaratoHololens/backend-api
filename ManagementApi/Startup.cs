using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Narato.Common.Factory;
using AutoMapper;
using Common.Configurations;
using Narato.Common.Interfaces;
using Narato.Common.Mappers;
using Narato.Common.Exceptions;
using Domain.Interfaces;
using Domain.Managers;
using DataProvider.Interfaces;
using DataProvider.DataProviders;
using Narato.Common.ActionFilters;
using Newtonsoft.Json.Serialization;
using Domain.Mappers;
using System;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace ManagementApi
{
    public class Startup
    {

        private MapperConfiguration MapperConfig;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            MapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfileConfiguration>();
            });
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<DbConfiguration>(Configuration.GetSection("DbConfiguration"));
            services.Configure<TwitterConfiguration>(Configuration.GetSection("TwitterConfiguration"));
            services.Configure<CognitiveServiceConfiguration>(Configuration.GetSection("CognitiveServiceConfiguration"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("ManagementAPI", new Info
                { Title = "HololensApplication - ManagementAPI",
                  Version = "v1",
                 Description = "Describes the endpoints used by the hololens Application and register WebApplication ",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Nadya Essebbar and Daryl van Loon"},
                });

                //Set the comments path for the swagger json and ui.
             //   var basePath = PlatformServices.Default.Application.ApplicationBasePath;
              //  var xmlPath = Path.Combine(basePath, "ManagementApi.xml");
              //  c.IncludeXmlComments(xmlPath);
            });

            // DI
            services.AddSingleton(MapperConfig.CreateMapper());

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ILogManager, LogManager>();
            services.AddScoped<IStorageProvider, DocumentDBProvider>();
            services.AddScoped<ILogDocumentDBProvider, LogDocumentDBProvider>();
            services.AddScoped<IUserDocumentDBProvider, UserDocumentDBProvider>();
            services.AddScoped<ICognitiveFaceProvider, CognitiveFaceProvider>();
            services.AddScoped<ISocialMediaProvider, TwitterProvider>();


            // Add framework services.
            services.AddMvc(
               //Add this filter globally so every request runs this filter to record execution time
               config =>
               {
                   config.Filters.Add(new ExecutionTimingFilter());
                   config.Filters.Add(new ModelValidationFilter());
               })
               //Add formatter for JSON output to client and to format received objects        
               .AddJsonOptions(x =>
               {
                   x.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
               }
           );

            //verfijn
            services.AddCors(o => o.AddPolicy("EnableCors", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddAuthentication();


            services.AddTransient<IResponseFactory, ResponseFactory>();
            services.AddTransient<IExceptionHandler, ExceptionHandler>();
            services.AddTransient<IExceptionToActionResultMapper, ExceptionToActionResultMapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                Authority = String.Format(Configuration["AzureAd:AadInstance"], Configuration["AzureAD:Tenant"]),
                Audience = Configuration["AzureAd:Audience"],
            });

            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/ManagementAPI/swagger.json", "ManagementAPI V1");
            });


        }
    }
}

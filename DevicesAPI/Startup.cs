using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DevicesAPI.Data;
using DevicesAPI.Models;
using DevicesAPI.Security;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DevicesAPI.Data;
using DevicesAPI.Models;
using DevicesAPI.Models.DAOs;
using DevicesAPI.Repository;
using DevicesAPI.Security;
using DevicesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DevicesAPI
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

            services.AddDbContext<DeviceAppDbContext>(options => options.UseInMemoryDatabase("devicesDb"));

            services.AddAuthentication();

            services.AddHttpClient()
                    .AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                                        .AddRequirements(new ApiRequirement())
                                        .Build();

                options.DefaultPolicy = defaultAuthPolicy;
            });

            //ApiRequirementHandler
            services.AddScoped<IAuthorizationHandler, ApiRequirementHandler>();

            //AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //Repositories
            services.AddScoped<IRepository<Device>, DeviceRepository>();
            services.AddScoped<IRepository<DeviceUsage>, DeviceUsageRepository>();
            services.AddScoped<IRepository<Address>, AddressRepository>();

            //IApplicationUserService
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                SeedData.InitializeDb(app.ApplicationServices);
            }

            app.UseExceptionHandler(appBuilder =>
                                    {
                                        appBuilder.Run(async httpcontext =>
                                                       {
                                                           httpcontext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                                                           httpcontext.Response.ContentType = "application/json";
                                                           var contextFeature = httpcontext.Features.Get<IExceptionHandlerFeature>();
                                                           if (contextFeature != null)
                                                           {
                                                               var errorMessage = new
                                                                                  {
                                                                                      Message = (contextFeature.Error != null) ? contextFeature.Error.Message : "Internal Server Error", StackTrace = (contextFeature.Error != null) ? contextFeature.Error.StackTrace : string.Empty
                                                                                  };
                                                               var jsonString = JsonConvert.SerializeObject(errorMessage);

                                                               await httpcontext.Response.WriteAsync(jsonString);
                                                           }
                                                       });
                                    });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

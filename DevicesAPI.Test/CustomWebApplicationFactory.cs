using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevicesAPI.Data;
using DevicesAPI.Security;
using DevicesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevicesAPI.Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //use a testing dbcontext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<DeviceAppDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<DeviceAppDbContext>(options =>
                                                          {
                                                              options.UseInMemoryDatabase("InMemoryDbForTesting");
                                                          });

                //replace the dbcontext with a new dbcontext
                descriptor = services.FirstOrDefault(d => d.ServiceType ==
                                                         typeof(DeviceAppDbContext) );
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                    services.AddDbContext<DeviceAppDbContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
                }


                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        Utilities.InitializeDbForTests(db);

                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }

                //replace the RequirementHandler with a TestRequirementHandler
                descriptor = services.FirstOrDefault(d => d.ServiceType ==
                                            typeof(IAuthorizationHandler) && d.ImplementationType== typeof(ApiRequirementHandler));
 
                services.Remove(descriptor);

                services.AddScoped<IAuthorizationHandler, TestApiRequirementHandler>();

                //replace the ApplicationUserService with a TestApplicationUserService
                descriptor = services.FirstOrDefault(d => d.ServiceType ==
                                                         typeof(IApplicationUserService) && d.ImplementationType == typeof(ApplicationUserService));

                services.Remove(descriptor);

                services.AddScoped<IApplicationUserService, TestApplicationUserService>();

            });
        }

    }
}

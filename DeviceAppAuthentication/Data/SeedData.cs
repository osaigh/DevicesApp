using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeviceAppAuthentication.Models;
using Microsoft.AspNetCore.Identity;

namespace DeviceAppAuthentication.Data
{
    public class SeedData
    {
        public static void InitializeDb(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<DeviceAppDbContext>();
                if (!dbContext.Users.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = new ApplicationUser()
                               {
                                   UserName = "john@yahoo.com",
                                   Email = "john@yahoo.com",
                                   FirstName = "john",
                                   LastName = "stack",
                               };

                    var rt = userManager.CreateAsync(user, "Qwert@1").GetAwaiter().GetResult();
                }
            }
        }
    }
}

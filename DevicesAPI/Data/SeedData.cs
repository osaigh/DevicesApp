using DevicesAPI.Models.DAOs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesAPI.Models;

namespace DevicesAPI.Data
{
    public class SeedData
    {
        public static void InitializeDb(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<DeviceAppDbContext>();
                if (!dbContext.Devices.Any())
                {
                    //create the address where the device is located
                    Address address = new Address()
                    {
                        Street1 = "109 George Street",
                        Street2 = "Apartment 2",
                        City = "St Catharines",
                        State = "Ontario",
                        Country = "Canada",
                        PostalCode = "L2M5P2"
                    };

                    Address address2 = new Address()
                                      {
                                          Street1 = "109 George Street",
                                          Street2 = "Apartment 2",
                                          City = "St Catharines",
                                          State = "Ontario",
                                          Country = "Canada",
                                          PostalCode = "L2M5P2"
                                      };

                    Address address3 = new Address()
                                      {
                                          Street1 = "109 George Street",
                                          Street2 = "Apartment 2",
                                          City = "St Catharines",
                                          State = "Ontario",
                                          Country = "Canada",
                                          PostalCode = "L2M5P2"
                                      };
                    dbContext.Addresses.Add(address);
                    dbContext.Addresses.Add(address2);
                    dbContext.Addresses.Add(address3);
                    dbContext.SaveChanges();

                    //Devices
                    Device device1 = new Device()
                    {
                        UserId = "john@yahoo.com",
                        Name = "Sony Display Board",
                        Temperature = 35,
                        Status = Status.Available,
                        Icon = "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",
                        AddressId = address.Id,
                    };

                    Device device2 = new Device()
                    {
                        UserId = "john@yahoo.com",
                        Name = "Dony speaker",
                        Temperature = 45,
                        Status = Status.Available,
                        Icon = "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",
                        AddressId = address2.Id,
                    };

                    Device device3 = new Device()
                    {
                        UserId = "john@yahoo.com",
                        Name = "Hp display Board",
                        Temperature = 23,
                        Status = Status.NotAvailable,
                        Icon = "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",
                        AddressId = address3.Id,
                    };

                    dbContext.Devices.Add(device1);
                    dbContext.Devices.Add(device2);
                    dbContext.Devices.Add(device3);
                    dbContext.SaveChanges();

                    //DeviceUsages

                    var deviceUsage1 = new DeviceUsage()
                    {
                        DeviceId = device1.Id,
                        Date = DateTime.UtcNow,
                        Metric1 = 2,
                        Metric2 = 3,
                        Metric3 = 4
                    };

                    var deviceUsage2 = new DeviceUsage()
                    {
                        DeviceId = device1.Id,
                        Date = DateTime.UtcNow,
                        Metric1 = 24,
                        Metric2 = 13,
                        Metric3 = 24
                    };

                    var deviceUsage3 = new DeviceUsage()
                    {
                        DeviceId = device1.Id,
                        Date = DateTime.UtcNow,
                        Metric1 = 28,
                        Metric2 = 53,
                        Metric3 = 74
                    };

                    var deviceUsage4 = new DeviceUsage()
                    {
                        DeviceId = device2.Id,
                        Date = DateTime.UtcNow,
                        Metric1 = 23,
                        Metric2 = 33,
                        Metric3 = 43
                    };

                    var deviceUsage5 = new DeviceUsage()
                    {
                        DeviceId = device2.Id,
                        Date = DateTime.UtcNow,
                        Metric1 = 12,
                        Metric2 = 13,
                        Metric3 = 14
                    };
                    var deviceUsage6 = new DeviceUsage()
                                       {
                                           DeviceId = device2.Id,
                                           Date = DateTime.UtcNow,
                                           Metric1 = 23,
                                           Metric2 = 33,
                                           Metric3 = 43
                                       };

                    var deviceUsage7 = new DeviceUsage()
                                       {
                                           DeviceId = device3.Id,
                                           Date = DateTime.UtcNow,
                                           Metric1 = 12,
                                           Metric2 = 13,
                                           Metric3 = 14
                                       };
                    var deviceUsage8 = new DeviceUsage()
                                       {
                                           DeviceId = device3.Id,
                                           Date = DateTime.UtcNow,
                                           Metric1 = 23,
                                           Metric2 = 33,
                                           Metric3 = 43
                                       };

                    var deviceUsage9 = new DeviceUsage()
                                       {
                                           DeviceId = device3.Id,
                                           Date = DateTime.UtcNow,
                                           Metric1 = 12,
                                           Metric2 = 13,
                                           Metric3 = 14
                                       };

                    dbContext.DeviceUsages.Add(deviceUsage1);
                    dbContext.DeviceUsages.Add(deviceUsage2);
                    dbContext.DeviceUsages.Add(deviceUsage3);
                    dbContext.DeviceUsages.Add(deviceUsage4);
                    dbContext.DeviceUsages.Add(deviceUsage5);
                    dbContext.DeviceUsages.Add(deviceUsage6);
                    dbContext.DeviceUsages.Add(deviceUsage7);
                    dbContext.DeviceUsages.Add(deviceUsage8);
                    dbContext.DeviceUsages.Add(deviceUsage9);

                    dbContext.SaveChanges();
                }
            }
        }
    }
}

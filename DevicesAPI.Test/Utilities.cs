using System;
using System.Collections.Generic;
using System.Text;
using DevicesAPI.Data;
using DevicesAPI.Models;
using DevicesAPI.Models.DAOs;
using Microsoft.Extensions.DependencyInjection;

namespace DevicesAPI.Test
{
    public class Utilities
    {
        public static string User_Id = "john@yahoo.com";
        public static void InitializeDbForTests(DeviceAppDbContext dbContext)
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
            Address address4 = new Address()
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
            dbContext.Addresses.Add(address4);
            dbContext.SaveChanges();


            //Devices
            Device device1 = new Device()
            {
                UserId = Config.UserId,
                Name = "Sony Display Board",
                Temperature = 35,
                Status = Status.Available,
                Icon = "",
                AddressId = address.Id,
            };

            Device device2 = new Device()
            {
                UserId = Config.UserId,
                Name = "Dony speaker",
                Temperature = 45,
                Status = Status.Available,
                Icon = "",
                AddressId = address2.Id,
            };

            Device device3 = new Device()
            {
                UserId = Config.UserId,
                Name = "Hp display Board",
                Temperature = 23,
                Status = Status.NotAvailable,
                Icon = "",
                AddressId = address3.Id,
            };
            Device device4 = new Device()
                             {
                                 UserId = Config.UserId,
                                 Name = "Hp display Board",
                                 Temperature = 23,
                                 Status = Status.NotAvailable,
                                 Icon = "",
                                 AddressId = address4.Id,
                             };

            dbContext.Devices.Add(device1);
            dbContext.Devices.Add(device2);
            dbContext.Devices.Add(device3);
            dbContext.Devices.Add(device4);
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
                DeviceId = device3.Id,
                Date = DateTime.UtcNow,
                Metric1 = 12,
                Metric2 = 13,
                Metric3 = 14
            };

            var deviceUsage6 = new DeviceUsage()
                               {
                                   DeviceId = device1.Id,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 12,
                                   Metric2 = 13,
                                   Metric3 = 14
                               };

            var deviceUsage7 = new DeviceUsage()
                               {
                                   DeviceId = device1.Id,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 12,
                                   Metric2 = 13,
                                   Metric3 = 14
                               };
            var deviceUsage8 = new DeviceUsage()
                               {
                                   DeviceId = device4.Id,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 12,
                                   Metric2 = 13,
                                   Metric3 = 14
                               };
            var deviceUsage9 = new DeviceUsage()
                               {
                                   DeviceId = device4.Id,
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

        public static List<Device> GetTestDevices()
        {
            List<Device> devices = new List<Device>();

            Address address = new Address()
                              {
                                  Id=1,
                                  Street1 = "109 George Street",
                                  Street2 = "Apartment 2",
                                  City = "St Catharines",
                                  State = "Ontario",
                                  Country = "Canada",
                                  PostalCode = "L2M5P2"
                              };

            Address address2 = new Address()
                               {
                                   Id = 2,
                                   Street1 = "109 George Street",
                                   Street2 = "Apartment 2",
                                   City = "St Catharines",
                                   State = "Ontario",
                                   Country = "Canada",
                                   PostalCode = "L2M5P2"
                               };

            Address address3 = new Address()
                               {
                                   Id = 3,
                                   Street1 = "109 George Street",
                                   Street2 = "Apartment 2",
                                   City = "St Catharines",
                                   State = "Ontario",
                                   Country = "Canada",
                                   PostalCode = "L2M5P2"
                               };

            Device device1 = new Device()
                             {
                                 Id = 1,
                                UserId = Config.UserId,
                                 Name = "Sony Display Board",
                                 Temperature = 35,
                                 Status = Status.Available,
                                 Icon = "",
                                 AddressId = address.Id,
                                 Address = address
            };

            Device device2 = new Device()
                             {
                                 Id = 2,
                                UserId = Config.UserId,
                                 Name = "Dony speaker",
                                 Temperature = 45,
                                 Status = Status.Available,
                                 Icon = "",
                                 AddressId = address2.Id,
                                 Address = address2
            };

            Device device3 = new Device()
                             {
                                 Id = 3,
                                UserId = Config.UserId,
                                 Name = "Hp display Board",
                                 Temperature = 23,
                                 Status = Status.NotAvailable,
                                 Icon = "",
                                 AddressId = address2.Id,
                                 Address = address2
                             };

            devices.Add(device1);
            devices.Add(device2);
            devices.Add(device3);

            return devices;
        }

        public static List<DeviceUsage> GetTestDeviceUsages()
        {
            List<DeviceUsage> deviceUsages = new List<DeviceUsage>();

            Address address = new Address()
            {
                Id = 1,
                Street1 = "109 George Street",
                Street2 = "Apartment 2",
                City = "St Catharines",
                State = "Ontario",
                Country = "Canada",
                PostalCode = "L2M5P2"
            };

            Device device1 = new Device()
            {
                Id = 1,
                UserId = Config.UserId,
                Name = "Sony Display Board",
                Temperature = 35,
                Status = Status.Available,
                Icon = "",
                AddressId = address.Id,
                Address = address,
                DeviceUsages = new List<DeviceUsage>()
            };

            var deviceUsage1 = new DeviceUsage()
                               {
                                   Id = 1,
                                   DeviceId = device1.Id,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 2,
                                   Metric2 = 3,
                                   Metric3 = 4,
                                   Device = device1,
            };

            var deviceUsage2 = new DeviceUsage()
                               {
                                   Id = 2,
                                   DeviceId = device1.Id,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 24,
                                   Metric2 = 13,
                                   Metric3 = 24,
                                   Device = device1,
            };

            var deviceUsage3 = new DeviceUsage()
                               {
                                   Id = 3,
                                   DeviceId = device1.Id,
                                   Device = device1,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 28,
                                   Metric2 = 53,
                                   Metric3 = 74
                               };

            deviceUsages.Add(deviceUsage1);
            deviceUsages.Add(deviceUsage2);
            deviceUsages.Add(deviceUsage3);

            device1.DeviceUsages.Add(deviceUsage1);
            device1.DeviceUsages.Add(deviceUsage2);
            device1.DeviceUsages.Add(deviceUsage3);

            return deviceUsages;
        }
    }
}

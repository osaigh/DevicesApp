using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevicesAPI.Controllers.api;
using DevicesAPI.Models;
using DevicesAPI.Repository;
using DevicesAPI.Services;
using Moq;
using Xunit;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Test.UnitTests
{
    public class UsersControllerUnitTest
    {
        #region Fields

        #endregion

        #region Constructor

        #endregion

        #region Methods

        [Fact]
        public async Task GetDevicesForUser_NoArgument_ReturnsCollectionOfDevicesForUser()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Stock Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var appUserServiceMock = new Mock<IApplicationUserService>();
            var addressRepo = new Mock<IRepository<DAOs.Address>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allDevices);
            deviceRepo.Setup(repo => repo.SearchForAsync(It.IsAny<Expression<Func<DAOs.Device, bool>>>())).ReturnsAsync((Expression<Func<DAOs.Device, bool>> predicate) =>
                                                                                                                      {
                                                                                                                          var selectedDevices = allDevices.Where(predicate.Compile());
                                                                                                                          return selectedDevices;
                                                                                                                      });

            //Controller
            var usersController = new UsersController(appUserServiceMock.Object, deviceRepo.Object, addressRepo.Object, mapper);

            //Act
            var devices = await usersController.GetDevicesForUser(Utilities.User_Id);

            //Assert
            Assert.NotNull(devices);
            Assert.Equal(allDevices.Count, devices.Count());
        }

        [Theory]
        [InlineData("john@yahoo.com")]
        public async Task AddDevice_ValidId_ReturnsDeviceUsage(string id)
        {
            //Arrange
            DTOs.Address address = new DTOs.Address()
                                   {
                                       Street1 = "109 George Street",
                                       Street2 = "Apartment 2",
                                       City = "St Catharines",
                                       State = "Ontario",
                                       Country = "Canada",
                                       PostalCode = "L2M5P2"
                                   };

            DTOs.Device device = new DTOs.Device()
            {
                UserId = id,
                Name = "Device 1",
                Temperature = 20.8,
                Status = Status.NotAvailable,
                Address = address
            };

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            var addressRepo = new Mock<IRepository<DAOs.Address>>();
            var appUserServiceMock = new Mock<IApplicationUserService>();
            appUserServiceMock.Setup(s => s.GetApplicationUser(It.IsAny<string>())).ReturnsAsync((string userId) =>
                                                                                                 {
                                                                                                     return new ApplicationUserDTO()
                                                                                                            {
                                                                                                                UserName = userId,
                                                                                                                Email = userId
                                                                                                     };
                                                                                                 });
            deviceRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.Device>())).ReturnsAsync((DAOs.Device device) =>
            {
                return device;
            });

            addressRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.Address>()))
                       .ReturnsAsync((DAOs.Address address) =>
                                     {
                                         return address;
                                     });

            //Controller
            var usersController = new UsersController(appUserServiceMock.Object, deviceRepo.Object, addressRepo.Object, mapper);

            //Act
            var result = await usersController.AddDevice(id, device);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(device.Id, result.Id);
        }

        [Theory]
        [InlineData("wer")]
        public async Task AddDevice_InValidId_ReturnsNull(string id)
        {
            //Arrange
            DTOs.Address address = new DTOs.Address()
                                   {
                                       Street1 = "109 George Street",
                                       Street2 = "Apartment 2",
                                       City = "St Catharines",
                                       State = "Ontario",
                                       Country = "Canada",
                                       PostalCode = "L2M5P2"
                                   };
            DTOs.Device device = new DTOs.Device()
                                 {
                                     UserId = id,
                                     Name = "Device 1",
                                     Temperature = 20.8,
                                     Status = Status.NotAvailable,
                                     Address = address
                                 };

            //AutoMapper
            var mapper = GetMapper();

            // Repo
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            var appUserServiceMock = new Mock<IApplicationUserService>();
            var addressRepo = new Mock<IRepository<DAOs.Address>>();
            appUserServiceMock.Setup(s => s.GetApplicationUser(It.IsAny<string>())).ReturnsAsync((string userId) =>
                                                                                                 {
                                                                                                     return null;
                                                                                                 });
            deviceRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.Device>())).ReturnsAsync((DAOs.Device device) =>
                                                                                          {
                                                                                              return device;
                                                                                          });

            //Controller
            var usersController = new UsersController(appUserServiceMock.Object, deviceRepo.Object, addressRepo.Object, mapper);

            //Act
            var result = await usersController.AddDevice(id, device);

            //Assert
            Assert.Null(result);
        }

        //[Theory]
        //[InlineData("john@yahoo.com",1)]
        //public async Task DeleteDevice_ValidId_ReturnsTrue(string id,int deviceid)
        //{
        //    //Arrange

        //    //AutoMapper
        //    var mapper = GetMapper();

        //    //Stock Repo
        //    var allDevices = Utilities.GetTestDevices().ToList();
        //    var deviceRepo = new Mock<IRepository<DAOs.Device>>();
        //    var appUserServiceMock = new Mock<IApplicationUserService>();
        //    appUserServiceMock.Setup(s => s.GetApplicationUser(It.IsAny<string>())).ReturnsAsync((string userId) =>
        //    {
        //        return new ApplicationUserDTO()
        //        {
        //            UserName = userId,
        //            Email = userId
        //        };
        //    });
        //    deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
        //                                                                          {
        //                                                                              var device = allDevices.FirstOrDefault(d => d.Id == id);
        //                                                                              return device;
        //                                                                          });

        //    deviceRepo.Setup(repo => repo.DeleteAsync(It.IsAny<DAOs.Device>())).ReturnsAsync((DAOs.Device device) =>
        //                                                                                     {
        //                                                                                         if (device != null)
        //                                                                                         {
        //                                                                                             return true;
        //                                                                                         }

        //                                                                                         return false;
        //                                                                                     });

        //    //Controller
        //    var usersController = new UsersController(appUserServiceMock.Object, deviceRepo.Object, mapper);

        //    //Act
        //    var result = await usersController.DeleteDevice(id, deviceid);

        //    //Assert
        //    Assert.True(result);
        //}

        //[Theory]
        //[InlineData("john@yahoo.com", 0)]
        //public async Task DeleteDevice_InValidId_ThrowsArgumentException(string id, int deviceid)
        //{
        //    //Arrange

        //    //AutoMapper
        //    var mapper = GetMapper();

        //    //Stock Repo
        //    var allDevices = Utilities.GetTestDevices().ToList();
        //    var deviceRepo = new Mock<IRepository<DAOs.Device>>();
        //    var appUserServiceMock = new Mock<IApplicationUserService>();
        //    appUserServiceMock.Setup(s => s.GetApplicationUser(It.IsAny<string>())).ReturnsAsync((string userId) =>
        //    {
        //        return new ApplicationUserDTO()
        //        {
        //            UserName = userId,
        //            Email = userId
        //        };
        //    });
        //    deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
        //    {
        //        var device = allDevices.FirstOrDefault(d => d.Id == id);
        //        return device;
        //    });

        //    deviceRepo.Setup(repo => repo.DeleteAsync(It.IsAny<DAOs.Device>())).ReturnsAsync((DAOs.Device device) =>
        //    {
        //        if (device != null)
        //        {
        //            return true;
        //        }

        //        return false;
        //    });

        //    //Controller
        //    var usersController = new UsersController(appUserServiceMock.Object, deviceRepo.Object, mapper);

        //    //Act

        //    //Assert
        //    Assert.Throws<ArgumentException>(() =>
        //                                     {
        //                                         usersController.DeleteDevice(id, deviceid).GetAwaiter().GetResult();
        //                                     });
        //}


        private IMapper GetMapper()
        {
            //AutoMapper
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<DTOs.Address>(It.IsAny<DAOs.Address>())).Returns((DAOs.Address input) =>
                                                                                               {
                                                                                                   var addressDTO = new DTOs.Address()
                                                                                                                  {
                                                                                                                      Id = input.Id,
                                                                                                                      Street1 = input.Street1,
                                                                                                                      Street2 = input.Street2,
                                                                                                                      City = input.City,
                                                                                                                      State = input.State,
                                                                                                                      Country = input.Country,
                                                                                                                      PostalCode = input.PostalCode
                                                                                                   };

                                                                                                   return addressDTO;
                                                                                               });

            mockMapper.Setup(mapper => mapper.Map<List<DTOs.Address>>(It.IsAny<List<DAOs.Address>>())).Returns((List<DAOs.Address> inputs) =>
                                                                                                               {
                                                                                                                   return inputs.Select(input => new DTOs.Address()
                                                                                                                                                 {
                                                                                                                                                     Id = input.Id,
                                                                                                                                                     Street1 = input.Street1,
                                                                                                                                                     Street2 = input.Street2,
                                                                                                                                                     City = input.City,
                                                                                                                                                     State = input.State,
                                                                                                                                                     Country = input.Country,
                                                                                                                                                     PostalCode = input.PostalCode
                                                                                                                   })
                                                                                                                                .ToList();
                                                                                                               });

            mockMapper.Setup(mapper => mapper.Map<DTOs.Device>(It.IsAny<DAOs.Device>())).Returns((DAOs.Device input) =>
                                                                                                 {
                                                                                                     var deviceDTO = new DTOs.Device()
                                                                                                                      {
                                                                                                                          Id = input.Id,
                                                                                                                          Name = input.Name,
                                                                                                                          Temperature = input.Temperature,
                                                                                                                          Status = input.Status,
                                                                                                                          UserId = input.UserId,
                                                                                                                      };

                                                                                                     return deviceDTO;
                                                                                                 });

            mockMapper.Setup(mapper => mapper.Map<List<DTOs.Device>>(It.IsAny<List<DAOs.Device>>())).Returns((List<DAOs.Device> inputs) =>
                                                                                                             {
                                                                                                                 return inputs.Select(input => new DTOs.Device()
                                                                                                                                               {
                                                                                                                                                   Id = input.Id,
                                                                                                                                                   Name = input.Name,
                                                                                                                                                   Temperature = input.Temperature,
                                                                                                                                                   Status = input.Status,
                                                                                                                                                   UserId = input.UserId,
                                                                                                                 })
                                                                                                                              .ToList();
                                                                                                             });

            return mockMapper.Object;
        }
        #endregion
    }
}

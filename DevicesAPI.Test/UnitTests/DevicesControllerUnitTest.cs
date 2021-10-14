using AutoMapper;
using DevicesAPI.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevicesAPI.Controllers.api;
using DevicesAPI.Models;
using Xunit;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Test.UnitTests
{
    public class DevicesControllerUnitTest
    {
        #region Fields

        #endregion

        #region Constructor

        #endregion

        #region Methods

        [Fact]
        public async Task GetAllDevice_NoArgument_ReturnsCollectionOfDevices()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            deviceRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allDevices);

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var devices = await devicesController.GetAllDevices();

            //Assert
            Assert.NotNull(devices);
            Assert.Equal(allDevices.Count, devices.Count());
        }

        [Theory]
        [InlineData(1)]
        public async Task GetRelatedDevices_ValidId_ReturnsCollectionOfRelatedDevices(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var device = allDevices.FirstOrDefault(d => d.Id == id);
                                                                                      return device;
                                                                                  });
            deviceRepo.Setup(repo => repo.SearchForAsync(It.IsAny<Expression<Func<DAOs.Device, bool>>>())).ReturnsAsync((Expression<Func<DAOs.Device, bool>> predicate) =>
                                                                                                                        {
                                                                                                                            var selectedDevices = allDevices.Where(predicate.Compile());
                                                                                                                            return selectedDevices;
                                                                                                                        });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var devices = await devicesController.GetRelatedDevices(id);

            //Assert
            Assert.NotNull(devices);
            Assert.Equal(allDevices.Count-1, devices.Count());
        }

        [Theory]
        [InlineData(0)]
        public void GetRelatedDevices_InValidId_ThrowsArgumentException(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() =>
                          {
                              devicesController.GetRelatedDevices(id).GetAwaiter().GetResult();
                          });
        }

        [Theory]
        [InlineData(1)]
        public async Task GetDevice_ValidId_ReturnsDevice(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var device = allDevices.FirstOrDefault(d => d.Id == id);
                                                                                      return device;
                                                                                  });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var device = await devicesController.GetDevice(id);

            //Assert
            Assert.NotNull(device);
            Assert.Equal(id, device.Id);
        }

        [Theory]
        [InlineData(0)]
        public async Task GetDevice_InValidId_ThrowsArgumentException(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var device = allDevices.FirstOrDefault(d => d.Id == id);
                                                                                      return device;
                                                                                  });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                devicesController.GetDevice(id).GetAwaiter().GetResult();
            });
        }

        [Theory]
        [InlineData(1,1)]
        public async Task GetDeviceUsage_ValidIds_ReturnsDeviceUsage(int id, int deviceusageid)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == deviceusageid);

                return (deviceUsage != null)? deviceUsage.Device:null;
            });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var deviceUsage = await devicesController.GetDeviceUsage(id,deviceusageid);

            //Assert
            Assert.NotNull(deviceUsage);
            Assert.Equal(deviceusageid, deviceUsage.Id);
        }

        [Theory]
        [InlineData(0, 1)]
        public async Task GetDeviceUsage_InValidId_ThrowsArgumentException(int id, int deviceusageid)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            
            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.GetDeviceUsage(id, deviceusageid).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1, 0)]
        public async Task GetDeviceUsage_InValidDeviceUsageId_ThrowsArgumentException(int id, int deviceusageid)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.GetDeviceUsage(id, deviceusageid).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1)]
        public async Task GetDeviceUsages_ValidId_ReturnsCollectionOfDeviceUsage(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == id);

                                                                                      return (deviceUsage != null) ? deviceUsage.Device : null;
                                                                                  });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var deviceUsages = await devicesController.GetDeviceUsages(id);

            //Assert
            Assert.NotNull(deviceUsages);
            Assert.Equal(allDeviceUsages.Count, deviceUsages.Count());
        }

        [Theory]
        [InlineData(0)]
        public async Task GetDeviceUsages_InValidId_ThrowsArgumentException(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
           
            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.GetDeviceUsages(id).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1,1)]
        public async Task DeleteDeviceUsage_ValidIds_ReturnsTrue(int id,int deviceusageid)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == deviceusageid);

                                                                                      return (deviceUsage != null) ? deviceUsage.Device : null;
                                                                                  });
            deviceUsageRepo.Setup(repo => repo.DeleteAsync(It.IsAny<DAOs.DeviceUsage>())).ReturnsAsync((DAOs.DeviceUsage deviceUsage) =>
                                                                                                       {
                                                                                                           var _deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == deviceUsage.Id);
                                                                                                           return _deviceUsage != null;
                                                                                                       });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var result = await devicesController.DeleteDeviceUsage(id,deviceusageid);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0, 1)]
        public void DeleteDeviceUsage_InValidId_ThrowsArgumentException(int id, int deviceusageid)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == deviceusageid);

                return (deviceUsage != null) ? deviceUsage.Device : null;
            });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
           

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.DeleteDeviceUsage(id, deviceusageid).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1, 0)]
        public void DeleteDeviceUsage_InValidDeviceUsageId_ThrowsArgumentException(int id, int deviceusageid)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == deviceusageid);

                                                                                      return (deviceUsage != null) ? deviceUsage.Device : null;
                                                                                  });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act


            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.DeleteDeviceUsage(id, deviceusageid).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteDevice_ValidId_ReturnsTrue(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == id);

                                                                                      return (deviceUsage != null) ? deviceUsage.Device : null;
                                                                                  });

            deviceRepo.Setup(repo => repo.DeleteAsync(It.IsAny<DAOs.Device>())).ReturnsAsync((DAOs.Device device) =>
                                                                                  {
                                                                                      if (device != null)
                                                                                      {
                                                                                          return true;
                                                                                      }

                                                                                      return false;
                                                                                  });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var result = await devicesController.DeleteDevice(id);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        public async Task DeleteDevice_InValidId_ReturnsTrue(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
   

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.DeleteDevice(id).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1)]
        public async Task AddDeviceUsage_ValidId_ReturnsDeviceUsage(int id)
        {
            //Arrange
            DTOs.DeviceUsage deviceUsage = new DTOs.DeviceUsage()
                                           {
                                               DeviceId = id,
                                               Metric1 = 1,
                                               Metric2 = 2,
                                               Metric3 = 3
                                           };

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                  {
                                                                                      var device = allDevices.FirstOrDefault(d => d.Id == id);
                                                                                      return device;
                                                                                  });

            deviceUsageRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.DeviceUsage>())).ReturnsAsync((DAOs.DeviceUsage deviceUsage) =>
                                                                                                    {
                                                                                                        return deviceUsage;
                                                                                                    });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var result = await devicesController.AddDeviceUsage(id, deviceUsage);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(deviceUsage.DeviceId, result.DeviceId);
        }

        [Theory]
        [InlineData(0)]
        public async Task AddDeviceUsage_InValidId_ThrowsArgumentException(int id)
        {
            //Arrange
            DTOs.DeviceUsage deviceUsage = new DTOs.DeviceUsage()
            {
                DeviceId = id,
                Metric1 = 1,
                Metric2 = 2,
                Metric3 = 3
            };

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                var device = allDevices.FirstOrDefault(d => d.Id == id);
                return device;
            });

            deviceUsageRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.DeviceUsage>())).ReturnsAsync((DAOs.DeviceUsage deviceUsage) =>
            {
                return deviceUsage;
            });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.AddDeviceUsage(id, deviceUsage).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateDevice_ValidId_ReturnsDevice(int id)
        {
            //Arrange
            DTOs.Device device = new DTOs.Device()
            {
                Id = id,
                Name = "Device1",
                Temperature = 20,
                Status = Status.NotAvailable
            };

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();
            deviceRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                var device = allDevices.FirstOrDefault(d => d.Id == id);
                return device;
            });

            deviceRepo.Setup(repo => repo.UpdateAsync(It.IsAny<DAOs.Device>())).ReturnsAsync((DAOs.Device device) =>
                                                                                               {
                                                                                                   return device;
                                                                                               });

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act
            var result = await devicesController.UpdateDevice(id, device);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(device.Id, result.Id);
        }

        [Theory]
        [InlineData(0)]
        public async Task UpdateDevice_InValidId_ThrowsArgumentException(int id)
        {
            //Arrange
            DTOs.Device device = new DTOs.Device()
                                 {
                                     Id = id,
                                     Name = "Device1",
                                     Temperature = 20,
                                     Status = Status.NotAvailable
                                 };

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDevices = Utilities.GetTestDevices().ToList();
            var deviceUsageRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            var deviceRepo = new Mock<IRepository<DAOs.Device>>();

            //Controller
            var devicesController = new DevicesController(deviceRepo.Object, deviceUsageRepo.Object, mapper);

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 devicesController.UpdateDevice(id, device).GetAwaiter().GetResult();
                                             });
        }

        private IMapper GetMapper()
        {
            //AutoMapper
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<DTOs.DeviceUsage>(It.IsAny<DAOs.DeviceUsage>())).Returns((DAOs.DeviceUsage input) =>
            {
                var deviceUsageDTO = new DTOs.DeviceUsage()
                {
                    Id = input.Id,
                    Metric1 = input.Metric1,
                    Metric2 = input.Metric2,
                    Metric3 = input.Metric3,
                    DeviceId = input.DeviceId,
                };

                return deviceUsageDTO;
            });

            mockMapper.Setup(mapper => mapper.Map<List<DTOs.DeviceUsage>>(It.IsAny<List<DAOs.DeviceUsage>>())).Returns((List<DAOs.DeviceUsage> inputs) =>
            {
                return inputs.Select(input => new DTOs.DeviceUsage()
                {
                    Id = input.Id,
                    Metric1 = input.Metric1,
                    Metric2 = input.Metric2,
                    Metric3 = input.Metric3,
                    DeviceId = input.DeviceId,
                })
                             .ToList();
            });

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

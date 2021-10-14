using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevicesAPI.Controllers.api;
using DevicesAPI.Repository;
using Moq;
using Xunit;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Test.UnitTests
{
    public class DeviceUsagesControllerUnitTest
    {
        #region Fields

        #endregion

        #region Constructor

        #endregion

        #region Methods

        [Fact]
        public async Task GetAllDeviceUsages_NoArgument_ReturnsCollectionOfDeviceUsages()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var mockRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allDeviceUsages);

            //Controller
            var deviceUsagesController = new DeviceUsagesController(mockRepo.Object, mapper);

            //Act
            var deviceUsages = await deviceUsagesController.GetAllDeviceUsages();

            //Assert
            Assert.NotNull(deviceUsages);
            Assert.Equal(allDeviceUsages.Count, deviceUsages.Count());
        }

        [Theory]
        [InlineData(1)]
        public async Task GetDeviceUsage_ValidId_ReturnsDeviceUsage(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var mockRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                {
                                                                                    var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == id);
                                                                                    return deviceUsage;
                                                                                });

            //Controller
            var deviceUsagesController = new DeviceUsagesController(mockRepo.Object, mapper);

            //Act
            var deviceUsage = await deviceUsagesController.GetDeviceUsage(id);

            //Assert
            Assert.NotNull(deviceUsage);
            Assert.Equal(id, deviceUsage.Id);
        }

        [Theory]
        [InlineData(0)]
        public async Task GetDeviceUsage_InValidId_ThrowsArgumentException(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var mockRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                {
                                                                                    var deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == id);
                                                                                    return deviceUsage;
                                                                                });

            //Controller
            var deviceUsagesController = new DeviceUsagesController(mockRepo.Object, mapper);

            //Act


            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 deviceUsagesController.GetDeviceUsage(id).GetAwaiter().GetResult();
                                             });
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteDeviceUsage_ValidId_ReturnsTrue(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var mockRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<DAOs.DeviceUsage>())).ReturnsAsync((DAOs.DeviceUsage deviceUsage) =>
                                                                                               {
                                                                                                   var _deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == deviceUsage.Id);
                                                                                                   return _deviceUsage != null;
                                                                                               });

            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                                {
                                                                                                    var _deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == id);
                                                                                                    return _deviceUsage;
                                                                                                });

            //Controller
            var deviceUsagesController = new DeviceUsagesController(mockRepo.Object, mapper);

            //Act
            var result = await deviceUsagesController.DeleteDeviceUsage(id);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        public async Task DeleteDeviceUsage_InValidId_ThrowsArgumentException(int id)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //Repo
            var allDeviceUsages = Utilities.GetTestDeviceUsages().ToList();
            var mockRepo = new Mock<IRepository<DAOs.DeviceUsage>>();
            mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<DAOs.DeviceUsage>())).ReturnsAsync((DAOs.DeviceUsage deviceUsage) =>
                                                                                                {
                                                                                                    var _deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == deviceUsage.Id);
                                                                                                    return _deviceUsage != null;
                                                                                                });

            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                                                                                {
                                                                                    var _deviceUsage = allDeviceUsages.FirstOrDefault(d => d.Id == id);
                                                                                    return _deviceUsage;
                                                                                });

            //Controller
            var deviceUsagesController = new DeviceUsagesController(mockRepo.Object, mapper);

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() =>
                                             {
                                                 deviceUsagesController.DeleteDeviceUsage(id).GetAwaiter().GetResult();
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

            return mockMapper.Object;
        }
        #endregion
    }
}

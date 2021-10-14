using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DevicesAPI.Data;
using DevicesAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace DevicesAPI.Test.IntegrationTests
{
    public class DeviceUsageControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<DevicesAPI.Startup>>
    {
        #region Fields

        private string url = "api/deviceusages";
        private CustomWebApplicationFactory<DevicesAPI.Startup> _Factory;
        #endregion

        #region Constructor
        public DeviceUsageControllerIntegrationTests(CustomWebApplicationFactory<DevicesAPI.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods

        [Fact]
        public async Task GetAllDeviceUsages_NoArgument_ReturnsCollectionOfDeviceUsages()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            var response = await client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            var deviceusages = JsonConvert.DeserializeObject<List<Models.DTOs.DeviceUsage>>(jsonString);

            //Assert
            Assert.NotNull(deviceusages);
            Assert.NotEmpty(deviceusages);
        }

        [Fact]
        public async Task GetDeviceUsage_ValidId_ReturnsDeviceUsage()
        {
            //Arrange
            int id = 0;
            var client = _Factory.CreateClient();
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            id = db.DeviceUsages.Last().Id;
            client.SetBearerToken(Config.Access_Token);

            //Act
            var response = await client.GetAsync(url+"/"+ id);
            var jsonString = await response.Content.ReadAsStringAsync();
            var deviceusage = JsonConvert.DeserializeObject<Models.DTOs.DeviceUsage>(jsonString);

            //Assert
            Assert.NotNull(deviceusage);
            Assert.Equal(id,deviceusage.Id);
        }

        [Theory]
        [InlineData(0)]
        public async Task GetDeviceUsage_InValidId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            var response = await client.GetAsync(url + "/" + id);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDeviceUsage_ValidId_ReturnsDeviceUsage()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.DeviceUsages.Last().Id;

            //Act
            var response = await client.DeleteAsync(url + "/" + id);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = bool.Parse(jsonString);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        public async Task DeleteDeviceUsage_InValidId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            var response = await client.DeleteAsync(url + "/" + id);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }
        #endregion
    }
}

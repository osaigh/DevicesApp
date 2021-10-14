using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DevicesAPI.Data;
using DevicesAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Test.IntegrationTests
{
    public class DeviceControllerIntegrationTests: IClassFixture<CustomWebApplicationFactory<DevicesAPI.Startup>>
    {
        #region Fields

        private string url = "api/devices";
        private CustomWebApplicationFactory<DevicesAPI.Startup> _Factory;
        #endregion

        #region Constructor
        public DeviceControllerIntegrationTests(CustomWebApplicationFactory<DevicesAPI.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods

        [Fact]
        public async Task GetAllDevice_NoArgument_ReturnsCollectionOfDevices()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            var response1 = await client.GetAsync(url);
            var jsonString1 = await response1.Content.ReadAsStringAsync();
            var devices = JsonConvert.DeserializeObject<List<Models.DTOs.Device>>(jsonString1);

            //Assert
            Assert.NotNull(devices);
            Assert.NotEmpty(devices);
        }

        [Fact]
        public async Task GetDevice_ValidId_ReturnsDevice()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.First().Id;

            //Act
            string endPoint = url + "/" + id;
            var response2 = await client.GetAsync(endPoint);
            var jsonString2 = await response2.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Models.DTOs.Device>(jsonString2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Theory]
        [InlineData(0)]
        public async Task GetDevice_InValidId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint = url + "/" + id;
            var response3 = await client.GetAsync(endPoint);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response3.StatusCode);
        }

        [Fact]
        public async Task GetRelatedDevices_ValidId_ReturnsCollectionOfRelatedDevices()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.First().Id;

            //Act
            string endPoint = url + "/" + id + "/related-devices";
            var response4 = await client.GetAsync(endPoint);
            var jsonString4 = await response4.Content.ReadAsStringAsync();
            var result4 = JsonConvert.DeserializeObject<List<Models.DTOs.Device>> (jsonString4);

            //Assert
            Assert.NotNull(result4);
            Assert.True(result4.Count > 0);
        }

        [Theory]
        [InlineData(0)]
        public async Task GetRelatedDevices_InValidId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint = url + "/" + id + "/related-devices";
            var response5 = await client.GetAsync(endPoint);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError,response5.StatusCode);
        }

        [Fact]
        public async Task GetDeviceUsages_ValidId_ReturnsCollectionOfDeviceUsages()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.First().Id;

            //Act
            string endPoint = url + "/" + id + "/deviceusages";
            var response6 = await client.GetAsync(endPoint);
            var jsonString6 = await response6.Content.ReadAsStringAsync();
            var result6 = JsonConvert.DeserializeObject<List<Models.DTOs.DeviceUsage>>(jsonString6);

            //Assert
            Assert.NotNull(result6);
            Assert.True(result6.Count > 0);
        }

        [Theory]
        [InlineData(0)]
        public async Task GetDeviceUsages_InValidId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint7 = url + "/" + id + "/deviceusages";
            var response7 = await client.GetAsync(endPoint7);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response7.StatusCode);
        }

        [Fact]
        public async Task GetDeviceUsage_ValidIds_ReturnsDeviceUsage()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.First().Id;
            var testdeviceUsage = db.DeviceUsages.FirstOrDefault(d => d.DeviceId == id);
            int deviceusageid = testdeviceUsage.Id;

            //Act
            string endPoint = url + "/" + id + "/deviceusages/"+ deviceusageid;
            var response8 = await client.GetAsync(endPoint);
            var jsonResultString = await response8.Content.ReadAsStringAsync();

            var deviceUsage = JsonConvert.DeserializeObject<Models.DTOs.DeviceUsage>(jsonResultString);
            
            //Assert
            Assert.NotNull(deviceUsage);
            Assert.Equal(deviceusageid, deviceUsage.Id);
        }

        [Theory]
        [InlineData(0, 1)]
        public async Task GetDeviceUsage_InValidId_ReturnsInternalServerError(int id, int deviceusageid)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint = url + "/" + id + "/deviceusages/" + deviceusageid;
            var response9 = await client.GetAsync(endPoint);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response9.StatusCode);
        }

        [Theory]
        [InlineData(1, 0)]
        public async Task GetDeviceUsage_InValidDeviceUsageId_ReturnsInternalServerError(int id, int deviceusageid)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint10 = url + "/" + id + "/deviceusages/" + deviceusageid;
            var response10 = await client.GetAsync(endPoint10);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response10.StatusCode);
        }

        [Fact]
        public async Task AddDeviceUsage_ValidDeviceId_ReturnsTrue()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.First().Id;

            var deviceUsage4 = new DTOs.DeviceUsage()
                               {
                                   DeviceId = id,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 23,
                                   Metric2 = 33,
                                   Metric3 = 43
                               };

            string jsonIn = JsonConvert.SerializeObject(deviceUsage4);
            StringContent content = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            string endPoint = url + "/" + id + "/deviceusages/";
            var response = await client.PostAsync(endPoint, content);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Models.DTOs.DeviceUsage>(jsonString);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.DeviceId);
        }

        [Theory]
        [InlineData(0)]
        public async Task AddDevice_InvalidId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var deviceUsage4 = new DTOs.DeviceUsage()
                               {
                                   DeviceId = id,
                                   Date = DateTime.UtcNow,
                                   Metric1 = 23,
                                   Metric2 = 33,
                                   Metric3 = 43
                               };

            string jsonIn = JsonConvert.SerializeObject(deviceUsage4);
            StringContent content = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            string endPoint = url + "/" + id + "/deviceusages/";
            var response = await client.PostAsync(endPoint, content);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task UpdateDevice_ValidDeviceId_ReturnsTrue()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.ToList()[2].Id;

            DTOs.Device device = new DTOs.Device()
                                 {
                                     UserId = Config.UserId,
                                     Name = "Device 1",
                                     Temperature = 50.8,
                                     Status = Status.Available,
                                     
                                 };

            string jsonIn = JsonConvert.SerializeObject(device);
            StringContent content = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            string endPoint = url + "/" + id;
            var response = await client.PutAsync(endPoint, content);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Models.DTOs.Device>(jsonString);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Theory]
        [InlineData(0)]
        public async Task UpdateDevice_InValidDeviceId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            DTOs.Device device = new DTOs.Device()
                                 {
                                     UserId = Config.UserId,
                                     Name = "Device 1",
                                     Temperature = 50.8,
                                     Status = Status.Available,

                                 };

            string jsonIn = JsonConvert.SerializeObject(device);
            StringContent content = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            string endPoint = url + "/" + id;
            var response = await client.PutAsync(endPoint, content);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDeviceUsage_ValidIds_ReturnsTrue()
        {
            //Arrange
            var client1 = _Factory.CreateClient();
            client1.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.ToList()[4].Id;
            var testdeviceUsage = db.DeviceUsages.FirstOrDefault(d => d.DeviceId == id);
            int deviceusageid = testdeviceUsage.Id;

            //Act
            string endPoint11 = url + "/" + id + "/deviceusages/" + deviceusageid;
            var response11 = await client1.DeleteAsync(endPoint11);
            var resultString = await response11.Content.ReadAsStringAsync();
            var resultOutput = bool.Parse(resultString);

            //Assert
            Assert.True(resultOutput);
        }

        [Theory]
        [InlineData(0, 1)]
        public async Task DeleteDeviceUsage_InValidId_ReturnsInternalServerError(int id, int deviceusageid)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint = url + "/" + id + "/deviceusages/" + deviceusageid;
            var response = await client.GetAsync(endPoint);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData(2, 0)]
        public async Task DeleteDeviceUsage_InValidDeviceUsageId_ReturnsInternalServerError(int id, int deviceusageid)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint = url + "/" + id + "/deviceusages/" + deviceusageid;
            var response = await client.GetAsync(endPoint);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDevice_ValidId_ReturnsTrue()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
            var scope = _Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DeviceAppDbContext>();
            int id = db.Devices.Last().Id;

            //Act
            string endPoint = url + "/" + id;
            var response = await client.DeleteAsync(endPoint);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = bool.Parse(jsonString);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        public async Task DeleteDevice_InValidId_ReturnsInternalServerError(int id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            string endPoint = url + "/" + id;
            var response = await client.GetAsync(endPoint);


            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        #endregion
    }
}

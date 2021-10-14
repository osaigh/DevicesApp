using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DevicesAPI.Models;
using Newtonsoft.Json;
using Xunit;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Test.IntegrationTests
{
    public class UserControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<DevicesAPI.Startup>>
    {
        #region Fields

        private string url = "api/users";
        private CustomWebApplicationFactory<DevicesAPI.Startup> _Factory;
        #endregion

        #region Constructor
        public UserControllerIntegrationTest(CustomWebApplicationFactory<DevicesAPI.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods

        [Fact]
        public async Task GetDevicesForUser_NoArgument_ReturnsCollectionOfDevicesForUser()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);

            //Act
            var response = await client.GetAsync(url+"/"+ Config.UserId+"/devices");
            var jsonString = await response.Content.ReadAsStringAsync();
            var devices = JsonConvert.DeserializeObject<List<DTOs.Device>>(jsonString);

            //Assert
            Assert.NotNull(devices);
            Assert.NotEmpty(devices);
        }

        [Theory]
        [InlineData("john@yahoo.com")]
        public async Task AddDevice_ValidId_ReturnsDeviceUsage(string id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
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

            string jsonIn = JsonConvert.SerializeObject(device);
            StringContent content = new StringContent(jsonIn,Encoding.UTF8,"application/json");

            //Act
            string endPoint = url + "/" + id + "/devices";
            var response = await client.PostAsync(endPoint, content);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DTOs.Device>(jsonString);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(device.UserId, result.UserId);
        }

        [Theory]
        [InlineData(Config.WrongUserId)]
        public async Task AddDevice_WrongId_ReturnsNull(string id)
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.SetBearerToken(Config.Access_Token);
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

            string jsonIn = JsonConvert.SerializeObject(device);
            StringContent content = new StringContent(jsonIn, Encoding.UTF8, "application/json");

            //Act
            string endPoint = url + "/" + id + "/devices";
            var response = await client.PostAsync(endPoint, content);
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DTOs.Device>(jsonString);

            //Assert
            Assert.Null(result);
        }


        #endregion
    }
}

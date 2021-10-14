using DevicesAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevicesAPI.Services
{
    public class ApplicationUserService:IApplicationUserService
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public ApplicationUserService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets ApplicationUser from AuthenticationServer given a userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApplicationUserDTO> GetApplicationUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId is invalid");
            }

            ApplicationUserDTO applicationUserDTO = null;
            using(var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("ApiKey", _configuration["AuthenticationServer:ApiKey"]);

                var baseUrl = _configuration["AuthenticationServer:BaseUrl"];
                
                var response = await httpClient.GetAsync(new Uri($"{baseUrl}/api/applicationusers/{userId}"));
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    applicationUserDTO = JsonConvert.DeserializeObject<ApplicationUserDTO>(json);
                }
            }

            return applicationUserDTO;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevicesAPI.Data;
using DevicesAPI.Repository;
using DevicesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Fields

        private readonly IMapper _Mapper;
        private readonly IRepository<DAOs.Device> _deviceRepository;
        private readonly IRepository<DAOs.Address> _addressRepository;
        private readonly IApplicationUserService _applicationUserService;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public UsersController(
            IApplicationUserService applicationUserService, 
            IRepository<DAOs.Device> deviceRepository,
            IRepository<DAOs.Address> addressRepository,
            IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _addressRepository = addressRepository;
            _deviceRepository = deviceRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        [HttpGet("{id}/devices")]
        public async Task<IEnumerable<DTOs.Device>> GetDevicesForUser(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                //get the results
                var devicesDAOs = await _deviceRepository.SearchForAsync(d => d.UserId.ToLower() == id.ToLower());

                //convert them to data transfer objects (DTOs)
                if (devicesDAOs.Any())
                {
                    var devicesDTOs = _Mapper.Map<List<DTOs.Device>>(devicesDAOs.ToList());
                    return devicesDTOs;
                }

            }

            return new List<DTOs.Device>();
        }

        [HttpPost("{id}/devices")]
        public async Task<DTOs.Device> AddDevice(string id, [FromBody] DTOs.Device deviceDTO)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id is invalid");
            }

            if (deviceDTO == null)
            {
                throw new ArgumentException("Device is null");
            }

            if (deviceDTO.Address == null)
            {
                throw new ArgumentException("Address is null");
            }

            if (string.IsNullOrEmpty(deviceDTO.Address.PostalCode))
            {
                throw new ArgumentException("Address PostalCode is not defined");
            }

            try
            {
                var appUser = await _applicationUserService.GetApplicationUser(id);

                if (appUser != null && string.Compare(appUser.UserName, id, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    //Add the address
                    var address = new DAOs.Address()
                                  {
                                      City = deviceDTO.Address.City,
                                      State = deviceDTO.Address.State,
                                      Country = deviceDTO.Address.Country,
                                      Street2 = deviceDTO.Address.Street2,
                                      Street1 = deviceDTO.Address.Street1,
                                      PostalCode = deviceDTO.Address.PostalCode
                    };

                    address = await _addressRepository.AddAsync(address);

                    var device = new DAOs.Device()
                                    {
                                        Name = deviceDTO.Name,
                                        UserId = appUser.UserName,
                                        Temperature = deviceDTO.Temperature,
                                        Status = deviceDTO.Status,
                                        AddressId = address.Id,
                                        Icon = deviceDTO.Icon
                                    };

                    var result = await _deviceRepository.AddAsync(device);
                    deviceDTO = _Mapper.Map<DTOs.Device>(result);

                    return deviceDTO;
                }
                
            }
            catch (Exception e)
            {
                //log.I Should use ILogger here. I will implement if i have more time
                Debug.WriteLine($"The following error occured while trying to add a device: {e.Message}");
            }
            

            return null;
        }

        //[HttpDelete("{id}/devices/{deviceid}")]
        //public async Task<bool> DeleteDevice(string id,int deviceid)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        throw new ArgumentException("id is invalid");
        //    }

        //    if (deviceid <= 0)
        //    {
        //        throw new ArgumentException("deviceid is invalid");
        //    }

        //    try
        //    {
        //        var appUser = await _applicationUserService.GetApplicationUser(id);

        //        if (appUser != null && string.Compare(appUser.UserName, id, StringComparison.OrdinalIgnoreCase) == 0)
        //        {
        //            //get the device with the given id
        //            var deviceDAO = await _deviceRepository.GetAsync(id);

        //            if (deviceDAO != null)
        //            {
        //                //delete
        //                bool result = await _deviceRepository.DeleteAsync(deviceDAO);
        //                return result;
        //            }

        //            return true;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        //log.I Should use ILogger here. I will implement if i have more time
        //        Debug.WriteLine($"The following error occured while trying to remove a device: {e.Message}");
        //    }


        //    return false;
        //}
        #endregion
    }
}

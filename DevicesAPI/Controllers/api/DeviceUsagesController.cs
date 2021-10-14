using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesAPI.Data;
using AutoMapper;
using DevicesAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevicesAPI.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceUsagesController : ControllerBase
    {
        #region Fields

        private readonly IMapper _Mapper;
        private readonly IRepository<DAOs.DeviceUsage> _deviceUsageRepository;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public DeviceUsagesController(IRepository<DAOs.DeviceUsage> deviceUsageRepository, IMapper mapper)
        {
            _deviceUsageRepository = deviceUsageRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        [HttpGet]
        public async Task<IEnumerable<DTOs.DeviceUsage>> GetAllDeviceUsages()
        {
            //get the deviceUsages
            var deviceUsageDAOs = await _deviceUsageRepository.GetAllAsync();

            //convert them to data transfer objects (DTOs)
            if (deviceUsageDAOs.Any())
            {
                var deviceUsageDTOs = _Mapper.Map<List<DTOs.DeviceUsage>>(deviceUsageDAOs.ToList());
                return deviceUsageDTOs;
            }

            return new List<DTOs.DeviceUsage>();
        }

        [HttpGet("{id}")]
        public async Task<DTOs.DeviceUsage> GetDeviceUsage(int id)
        {
            if (id > 0)
            {
                //get the device usage with the given id
                var deviceUsageDAO = await _deviceUsageRepository.GetAsync(id);
                    ;

                //convert to data transfer object (DTO)
                if (deviceUsageDAO != null)
                {
                    var deviceUsageDTO = _Mapper.Map<DTOs.DeviceUsage>(deviceUsageDAO);
                    return deviceUsageDTO;
                }

            }
            else
            {
                //invalid id
                throw new ArgumentException("id is invalid");
            }
            return null;
        }


        [HttpDelete("{id}")]
        public async Task<bool> DeleteDeviceUsage(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            //get the device with the given id
            var deviceUsageDAO = await _deviceUsageRepository.GetAsync(id);

            if (deviceUsageDAO != null)
            {
                //delete
                bool result = await _deviceUsageRepository.DeleteAsync(deviceUsageDAO);
                return result;
            }


            return false;
        }

        #endregion
    }
}

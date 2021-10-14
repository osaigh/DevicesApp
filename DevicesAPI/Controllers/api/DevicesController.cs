using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevicesAPI.Data;
using DevicesAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        #region Fields

        private readonly IMapper _Mapper;
        private readonly IRepository<DAOs.Device> _deviceRepository;
        private readonly IRepository<DAOs.DeviceUsage> _deviceUsageRepository;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public DevicesController(IRepository<DAOs.Device> deviceRepository, IRepository<DAOs.DeviceUsage> deviceUsageRepository, IMapper mapper)
        {
            _deviceRepository = deviceRepository;
            _deviceUsageRepository = deviceUsageRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        [HttpGet]
        public async Task<IEnumerable<DTOs.Device>> GetAllDevices()
        {
            //get the results
            var devicesDAOs = await _deviceRepository.GetAllAsync();

            //convert them to data transfer objects (DTOs)
            if (devicesDAOs.Any())
            {
                var devicesDTOs = _Mapper.Map<List<DTOs.Device>>(devicesDAOs.ToList());
                return devicesDTOs;
            }

            return new List<DTOs.Device>();
        }

        [HttpGet("{id}")]
        public async Task<DTOs.Device> GetDevice(int id)
        {
            if (id > 0)
            {
                //get the device with the given id
                var deviceDAO = await _deviceRepository.GetAsync(id);

                //convert to data transfer objects (DTO)
                if (deviceDAO != null)
                {
                    var deviceDTO = _Mapper.Map<DTOs.Device>(deviceDAO);
                    return deviceDTO;
                }

            }
            else
            {
                //invalid id
                throw new ArgumentException("id is invalid");
            }
            return null;
        }

        [HttpGet("{id}/related-devices")]
        public async Task<IEnumerable<DTOs.Device>> GetRelatedDevices(int id)
        {
            if (id > 0)
            {
                //get the device with the given id
                var deviceDAO = await _deviceRepository.GetAsync(id);

                if (deviceDAO != null)
                {
                    //get related devices based on address postal code
                    var deviceDAOs = await _deviceRepository
                        .SearchForAsync(d => d.Address.PostalCode == deviceDAO.Address.PostalCode && d.Id != id);

                    if (deviceDAOs.Any())
                    {
                        var deviceDTOs = _Mapper.Map<List<DTOs.Device>>(deviceDAOs.ToList());
                        return deviceDTOs;
                    }

                }

            }
            else
            {
                //invalid id
                throw new ArgumentException("id is invalid");
            }

            return new List<DTOs.Device>();
        }

        [HttpGet("{id}/deviceusages")]
        public async Task<IEnumerable<DTOs.DeviceUsage>> GetDeviceUsages(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            //get the device with the given id
            var deviceDAO = await _deviceRepository.GetAsync(id);

            if (deviceDAO != null)
            {
                //get devices usages

                if (deviceDAO.DeviceUsages != null && deviceDAO.DeviceUsages.Any())
                {
                    var deviceUsagesDTOs = _Mapper.Map<List<DTOs.DeviceUsage>>(deviceDAO.DeviceUsages.ToList());
                    return deviceUsagesDTOs;
                }

            }


            return new List<DTOs.DeviceUsage>();
        }

        [HttpGet("{id}/deviceusages/{deviceusageid}")]
        public async Task<DTOs.DeviceUsage> GetDeviceUsage(int id, int deviceusageid)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            if (deviceusageid <= 0)
            {
                throw new ArgumentException("deviceusageid is invalid");
            }

            //get the device with the given id
            var deviceDAO = await _deviceRepository.GetAsync(id);

            if (deviceDAO != null)
            {
                //get the devices usage
                if (deviceDAO.DeviceUsages != null && deviceDAO.DeviceUsages.Any())
                {
                    var deviceUsage = deviceDAO.DeviceUsages.FirstOrDefault(d => d.Id == deviceusageid);

                    if (deviceUsage != null)
                    {
                        var deviceUsageDTO = _Mapper.Map<DTOs.DeviceUsage>(deviceUsage);
                        return deviceUsageDTO;
                    }
                }

            }


            return null;
        }

        [HttpPut("{id}")]
        public async Task<DTOs.Device> UpdateDevice(int id, [FromBody] DTOs.Device deviceDTO)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            if (deviceDTO == null)
            {
                throw new ArgumentException("deviceDTO is null");
            }

            //get the device with the given id
            var deviceDAO = await _deviceRepository.GetAsync(id);

            if (deviceDAO != null)
            {
                //update
                deviceDAO.Temperature = deviceDTO.Temperature;
                deviceDAO.Status = deviceDTO.Status;
                deviceDAO.Name = deviceDTO.Name;

                await _deviceRepository.UpdateAsync(deviceDAO);

                deviceDTO = _Mapper.Map<DTOs.Device>(deviceDAO);

                return deviceDTO;
            }


            return null;
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteDevice(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            //get the device with the given id
            var deviceDAO = await _deviceRepository.GetAsync(id);

            if (deviceDAO != null)
            {
                //delete
                bool result=await _deviceRepository.DeleteAsync(deviceDAO);
                return result;
            }


            return false;
        }

        [HttpDelete("{id}/deviceusages/{deviceusageid}")]
        public async Task<bool> DeleteDeviceUsage(int id, int deviceusageid)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            if (deviceusageid <= 0)
            {
                throw new ArgumentException("deviceusageid is invalid");
            }

            //get the device with the given id
            var deviceDAO = await _deviceRepository.GetAsync(id);

            if (deviceDAO != null && deviceDAO.DeviceUsages != null)
            {
                var deviceUsage = deviceDAO.DeviceUsages.FirstOrDefault(d => d.Id == deviceusageid);
                if (deviceUsage != null)
                {
                    bool result = await _deviceUsageRepository.DeleteAsync(deviceUsage);
                    return result;
                }

            }
            

            return false;
        }

        [HttpPost("{id}/deviceusages")]
        public async Task<DTOs.DeviceUsage> AddDeviceUsage(int id, [FromBody] DTOs.DeviceUsage deviceUsageDTO)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            if (deviceUsageDTO == null)
            {
                throw new ArgumentException("Device Usage is null");
            }

            //get the device with the given id
            var deviceDAO = await _deviceRepository.GetAsync(id);

            if (deviceDAO != null)
            {
                var deviceUsageDAO = new DAOs.DeviceUsage()
                                     {
                                         Date = deviceUsageDTO.Date,
                                         DeviceId = id,
                                         Metric1 = deviceUsageDTO.Metric1,
                                         Metric2 = deviceUsageDTO.Metric2,
                                         Metric3 = deviceUsageDTO.Metric3,
                                     };

                var result = await _deviceUsageRepository.AddAsync(deviceUsageDAO);

                deviceUsageDTO = _Mapper.Map<DTOs.DeviceUsage>(result);
                return deviceUsageDTO;
            }

            return null;
        }

        
        #endregion
    }
}

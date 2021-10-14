using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAOs = DevicesAPI.Models.DAOs;
using DTOs = DevicesAPI.Models.DTOs;

namespace DevicesAPI.Models
{
    public class MappingProfile : Profile
    {
        #region Constructors
        public MappingProfile()
        {
            //devices
            CreateMap<DAOs.Device, DTOs.Device>();
            CreateMap<DTOs.Device, DAOs.Device>()
                .ForMember(dest => dest.DeviceUsages, opt => opt.Ignore());

            //device usages
            CreateMap<DAOs.DeviceUsage, DTOs.DeviceUsage>();
            CreateMap<DTOs.DeviceUsage, DAOs.DeviceUsage>();

            //addresses
            CreateMap<DAOs.Address, DTOs.Address>();
            CreateMap<DTOs.Address, DAOs.Address>()
                .ForMember(dest => dest.Device, opt => opt.Ignore());
        }
        #endregion
    }
}

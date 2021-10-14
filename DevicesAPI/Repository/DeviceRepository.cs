using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DevicesAPI.Data;
using DevicesAPI.Models.DAOs;
using Microsoft.EntityFrameworkCore;

namespace DevicesAPI.Repository
{
    public class DeviceRepository : IRepository<Device>
    {
        #region fields
        private readonly DeviceAppDbContext _deviceAppDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public DeviceRepository(DeviceAppDbContext deviceAppDbContext, IMapper mapper)
        {
            _deviceAppDbContext = deviceAppDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<Device> AddAsync(Device device)
        {
            //check to see if the Device already exist
            Device _device = await _deviceAppDbContext.Devices.FirstOrDefaultAsync(s => s.Id == device.Id);

            //If the device does not exist, add
            if (_device == null)
            {
                await _deviceAppDbContext.Devices.AddAsync(device);
                await _deviceAppDbContext.SaveChangesAsync();
            }
            return device;
        }

        public async Task<bool> DeleteAsync(Device device)
        {
            var _device = await GetAsync(device.Id);

            if (_device != null)
            {
                _deviceAppDbContext.Devices.Remove(_device);
                await _deviceAppDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Device>> SearchForAsync(Expression<Func<Device, bool>> predicate)
        {
            var result = await _deviceAppDbContext.Devices
                                            .Include(d => d.Address).ToListAsync();

            return result.Where(predicate.Compile());
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await _deviceAppDbContext.Devices
                                            .Include(d => d.Address)
                                        .ToListAsync();

        }

        public async Task<Device> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _deviceAppDbContext.Devices
                                            .Include(d => d.Address)
                                            .Include(d => d.DeviceUsages)
                                        .FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<Device> UpdateAsync(Device device)
        {
            var _device = await GetAsync(device.Id);

            if (_device == null)
            {
                return null;
            }

            if (!ReferenceEquals(device, _device))
            {
                _Mapper.Map(device, _device);
            }

            await _deviceAppDbContext.SaveChangesAsync();

            return _device;
        }
        #endregion
    }
}

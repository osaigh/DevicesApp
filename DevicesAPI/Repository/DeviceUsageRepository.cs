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
    public class DeviceUsageRepository : IRepository<DeviceUsage>
    {
        #region fields
        private readonly DeviceAppDbContext _deviceAppDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public DeviceUsageRepository(DeviceAppDbContext deviceAppDbContext, IMapper mapper)
        {
            _deviceAppDbContext = deviceAppDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<DeviceUsage> AddAsync(DeviceUsage deviceUsage)
        {
            //check to see if the DeviceUsage already exist
            DeviceUsage _deviceUsage = await _deviceAppDbContext.DeviceUsages.FirstOrDefaultAsync(s => s.Id == deviceUsage.Id);

            //If the device usage does not exist, add
            if (_deviceUsage == null)
            {
                await _deviceAppDbContext.DeviceUsages.AddAsync(deviceUsage);
                await _deviceAppDbContext.SaveChangesAsync();
            }
            return deviceUsage;
        }

        public async Task<bool> DeleteAsync(DeviceUsage deviceUsage)
        {
            var _deviceUsage = await GetAsync(deviceUsage.Id);

            if (_deviceUsage != null)
            {
                _deviceAppDbContext.DeviceUsages.Remove(_deviceUsage);
                await _deviceAppDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<DeviceUsage>> SearchForAsync(Expression<Func<DeviceUsage, bool>> predicate)
        {
            return await _deviceAppDbContext.DeviceUsages
                                            .Include(d => d.Device)
                                            .Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<DeviceUsage>> GetAllAsync()
        {
            return await _deviceAppDbContext.DeviceUsages
                                            .Include(d => d.Device)
                                        .ToListAsync();

        }

        public async Task<DeviceUsage> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _deviceAppDbContext.DeviceUsages
                                            .Include(d => d.Device)
                                        .FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<DeviceUsage> UpdateAsync(DeviceUsage deviceUsage)
        {
            var _deviceUsage = await GetAsync(deviceUsage.Id);

            if (_deviceUsage == null)
            {
                return null;
            }

            if (!ReferenceEquals(deviceUsage, _deviceUsage))
            {
                _Mapper.Map(deviceUsage, _deviceUsage);
            }

            await _deviceAppDbContext.SaveChangesAsync();

            return _deviceUsage;
        }
        #endregion
    }
}

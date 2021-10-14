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
    public class AddressRepository : IRepository<Address>
    {
        #region fields
        private readonly DeviceAppDbContext _deviceAppDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public AddressRepository(DeviceAppDbContext deviceAppDbContext, IMapper mapper)
        {
            _deviceAppDbContext = deviceAppDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<Address> AddAsync(Address address)
        {
            //check to see if the Address already exist
            Address _address = await _deviceAppDbContext.Addresses.FirstOrDefaultAsync(s => s.Id == address.Id);

            //If the address does not exist, add
            if (_address == null)
            {
                await _deviceAppDbContext.Addresses.AddAsync(address);
                await _deviceAppDbContext.SaveChangesAsync();
            }
            return address;
        }

        public async Task<bool> DeleteAsync(Address address)
        {
            var _address = await GetAsync(address.Id);

            if (_address != null)
            {
                _deviceAppDbContext.Addresses.Remove(_address);
                await _deviceAppDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Address>> SearchForAsync(Expression<Func<Address, bool>> predicate)
        {
            return await _deviceAppDbContext.Addresses
                                            .Include(a => a.Device)
                                            .Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _deviceAppDbContext.Addresses
                                            .Include(a => a.Device)
                                        .ToListAsync();

        }

        public async Task<Address> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _deviceAppDbContext.Addresses
                                            .Include(a => a.Device)
                                        .FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<Address> UpdateAsync(Address address)
        {
            var _address = await GetAsync(address.Id);

            if (_address == null)
            {
                return null;
            }

            if (!ReferenceEquals(address, _address))
            {
                _Mapper.Map(address, _address);
            }

            await _deviceAppDbContext.SaveChangesAsync();

            return _address;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceAppAuthentication.Data;
using DeviceAppAuthentication.Filters;
using DeviceAppAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceAppAuthentication.Controllers.api
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        #region Fields

        private readonly DeviceAppDbContext _deviceAppDbContext;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public ApplicationUsersController(DeviceAppDbContext deviceAppDbContext)
        {
            _deviceAppDbContext = deviceAppDbContext;
        }
        #endregion

        #region Methods
        [HttpGet("{id}")]
        public async Task<ApplicationUserDTO> GetApplicationUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id is invalid");
            }

            var appUser = await _deviceAppDbContext.Users
                                             .FirstOrDefaultAsync(u => string.Compare(u.UserName, id, StringComparison.OrdinalIgnoreCase) == 0);

            if (appUser != null)
            {
                ApplicationUserDTO appUserDTO = new ApplicationUserDTO()
                                                {
                                                    UserName = appUser.UserName,
                                                    Email = appUser.Email
                                                };

                return appUserDTO;
            }

            return null;
        }
        #endregion

    }
}

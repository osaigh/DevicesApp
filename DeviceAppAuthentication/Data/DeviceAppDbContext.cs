using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceAppAuthentication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeviceAppAuthentication.Data
{
    public class DeviceAppDbContext:IdentityDbContext<ApplicationUser>
    {
        #region Fields
        #endregion

        #region Constructor

        public DeviceAppDbContext(DbContextOptions<DeviceAppDbContext> options) : base(options)
        {

        }
        #endregion
    }
}

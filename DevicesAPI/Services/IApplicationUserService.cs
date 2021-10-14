using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesAPI.Models;

namespace DevicesAPI.Services
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserDTO> GetApplicationUser(string userId);
    }
}

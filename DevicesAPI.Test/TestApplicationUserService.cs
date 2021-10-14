using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevicesAPI.Models;
using DevicesAPI.Services;

namespace DevicesAPI.Test
{
    public class TestApplicationUserService : IApplicationUserService
    {
        public async Task<ApplicationUserDTO> GetApplicationUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            if (String.Compare(userId, Config.WrongUserId, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return null;
            }

            return new ApplicationUserDTO()
                   {
                       UserName = userId,
                       Email = Config.UserId
                   };
        }
    }
}

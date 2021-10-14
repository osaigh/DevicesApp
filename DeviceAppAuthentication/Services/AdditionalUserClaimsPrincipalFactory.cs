using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeviceAppAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DeviceAppAuthentication.Services
{
    public class AdditionalUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        #region Fields
        UserManager<ApplicationUser> _userManager;
        #endregion

        public AdditionalUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            _userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var claimsIdentity = await base.GenerateClaimsAsync(user);

            var additionalClaims = GetAdditionalClaims(user);
            foreach (Claim claim in additionalClaims)
            {
                claimsIdentity.AddClaim(claim);
            }

            return claimsIdentity;
        }

        protected List<Claim> GetAdditionalClaims(ApplicationUser user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("UserName",user.UserName));
            claims.Add(new Claim("FirstName", user.FirstName));
            claims.Add(new Claim("LastName", user.LastName));
            claims.Add(new Claim("Email", user.Email));

            //We add the DevicesApi as a claim to the user but ideally, this scope should be setup for the Client SPA
            claims.Add(new Claim("DevicesAPI", "DevicesApi"));

            return claims;
        }
    }
}

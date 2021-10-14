using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceAppAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using DeviceAppAuthentication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;

namespace DeviceAppAuthentication.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Fields
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public AuthenticationController(
            SignInManager<ApplicationUser> signManager,
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IConfiguration configuration)
        {
            _signInManager = signManager;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _configuration = configuration;
        }
        #endregion

        #region Methods
        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel loginViewModel = new LoginViewModel();

            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            //validate the inputs
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            //login user
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            var usr = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (result.Succeeded)
            {
                //get the user 
                var user = await _userManager.FindByNameAsync(loginViewModel.Username);

                if (user != null)
                {
                    //get ClaimsPrincipal
                    var claimsPrincipal = await _userClaimsPrincipalFactory.CreateAsync(user);

                    var issuer = _configuration["OAuth:Authority"];
                    var audience = _configuration["OAuth:Audience"];
                    var secret = _configuration["OAuth:Secret"];

                    //generate a token
                    var secretBytes = Encoding.UTF8.GetBytes(secret);
                    var key = new SymmetricSecurityKey(secretBytes);
                    var alg = SecurityAlgorithms.HmacSha256;
                    var signingCredentials = new SigningCredentials(key, alg);
                    var token = new JwtSecurityToken(
                        issuer,
                        audience,
                        claimsPrincipal.Claims.ToList(),
                        DateTime.Now,
                        DateTime.Now.AddDays(7),
                        signingCredentials);

                    var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

                    LoginResult loginResult = new LoginResult()
                                              {
                                                  Username = user.UserName,
                                                  AccessToken = tokenJson,
                                              };

                    return Ok(loginResult);

                }
            }
            else
            {
                ModelState.AddModelError("", "Sign-in failed");
                return View(loginViewModel);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();

            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            ApplicationUser user = new ApplicationUser()
                                   {
                                       FirstName = registerViewModel.FirstName,
                                       LastName = registerViewModel.LastName,
                                       UserName = registerViewModel.Username,
                                       Email = registerViewModel.Email
                                   };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");

            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }

        public IActionResult Validate()
        {
            var isValid = HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader);
            if (isValid)
            {
                var access_token = authHeader.ToString().Split(' ')[1];

                var issuer = _configuration["OAuth:Authority"];
                var audience = _configuration["OAuth:Audience"];
                var secret = _configuration["OAuth:Secret"];

                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    tokenHandler.ValidateToken(access_token, new TokenValidationParameters
                                                             {
                                                                 ValidateIssuerSigningKey = true,
                                                                 ValidateIssuer = true,
                                                                 ValidateAudience = true,
                                                                 ValidIssuer = issuer,
                                                                 ValidAudience = audience,
                                                                 IssuerSigningKey = securityKey
                                                             }, out SecurityToken validatedToken);
                    if (validatedToken != null)
                    {
                        if (validatedToken.ValidTo.Subtract(DateTime.Now).Ticks > 0)
                        {
                            return Ok();
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"The following error occured while trying to validate access token: {e.Message}");
                }
            }

            return BadRequest();
        }
        #endregion
    }
}

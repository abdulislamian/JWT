using JWTAuthentication.Data;
using JWTAuthentication.Models;
using JWTAuthentication.Models.DTO;
using JWTAuthentication.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenRepository _tokenRepository, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            tokenRepository = _tokenRepository;
            _signInManager = signInManager;
        }
        //POST :  /api/Auth/Register

        [HttpPost]
        [Route("Register")]
        [Authorize(policy: "OnlyAdmin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new ApplicationUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username,
                PasswordExpirationDate = DateTime.Now
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if (identityResult.Succeeded)
            {
                //Add roles  to this User
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was Registed! Please Login");
                    }
                }

            }
            else
            {
                List<string> errorlist = new List<string>();
                foreach (var error in identityResult.Errors)
                {
                    errorlist.Add(error.Description);
                }

                return BadRequest(errorlist);
            }
            return BadRequest("Something went Wrong.");

        }

        //POST : /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var IsUserExist = await _userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (IsUserExist != null)
            {
                var login = await _signInManager.PasswordSignInAsync(loginRequestDTO.Username, loginRequestDTO.Password, false, true);

                if (login.Succeeded)
                {
                    if (!isPasswordValid(IsUserExist))
                    {
                        return BadRequest("Due to inactivity, you need to Reset your Password");
                    }

                    var roles = await _userManager.GetRolesAsync(IsUserExist);
                    if (roles != null)
                    {

                        //Create Token
                        var JWTToken = tokenRepository.CreateJWTToken(IsUserExist, roles.ToList());

                        await tokenRepository.StoreToken(IsUserExist.Id, JWTToken);

                        var response = new LoginResponseDTO
                        {
                            JwtToken = JWTToken
                        };
                        return Ok(response);
                    }

                    return Ok("Login Successful");
                }
                if (login.IsLockedOut)
                {
                    return BadRequest("Your account is Lockout for 15mins due to invalid attempts");
                }
            }
            return BadRequest("Username or Password is incorrect.");

        }

        private bool isPasswordValid(ApplicationUser isUserExist)
        {
            bool isPasswordValid = true;
            if (isUserExist.PasswordExpirationDate.AddDays(5) <= DateTime.Now)
            {
                isPasswordValid = false;
            }
            return isPasswordValid;
        }

        //POST : /api/Auth/ForgetPassword
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var Response = new ForgetPasswordModel
                {
                    UserId = user.Id,
                    Token = token
                };
                return Ok(Response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        user.PasswordExpirationDate = DateTime.Now;
                        await _userManager.UpdateAsync(user);

                        return Ok("Password Reset Successfully, Please Login Again");
                    }
                    else
                    {
                        List<string> errorlist = new List<string>();
                        foreach (var error in result.Errors)
                        {
                            errorlist.Add(error.Description);
                        }
                        return BadRequest(errorlist);
                    }

                }
                return NotFound();

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                List<string> errorlist = new List<string>();
                foreach (var error in errors)
                {
                    errorlist.Add(error.ErrorMessage);
                }
                return BadRequest(errorlist);
            }
        }
        
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> logout()
        {
            //var currentUser = User.Identity.Name;
            //if(currentUser != null)
            //{
            //    await _signInManager.SignOutAsync();
            //    return Ok("Logout Successfully");
            //}
            await _signInManager.SignOutAsync();

            return BadRequest("Something went wrong...");
        }
    }
}

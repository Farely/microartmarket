using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Controllers.RequestModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedData;

namespace Auth.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/users/auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IOptions<UserValidation> _settings;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IOptions<UserValidation> settings, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<AuthController> logger)
        {
            _settings = settings;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        /// <summary>
        ///  Получает картину по Id.
        /// </summary>
        /// <returns>Возвращает токен</returns>
        /// <param name="authModel"></param>
        /// >Возвращает токен
        /// </returns>
        /// <response code="200">Возвращает токен</response>
        /// <response code="401">Авторизация не удалась</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] LoginUser authModel)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var signInResult =
                await _signInManager.PasswordSignInAsync(authModel.Login, authModel.Password, false, false);
            if (!signInResult.Succeeded) return Unauthorized("Неверный логин/пароль");
            var user = await _userManager.FindByNameAsync(authModel.Login);
            var roles = await _userManager.GetRolesAsync(user);
            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            return Ok(GenerateJwt(authModel, claims, roles));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        [HttpGet("about")]
        public IActionResult GetInfo()
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                return Json(new UserInfo
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Claims = User.Claims.ToDictionary(claim => claim.Type, claim => claim.Value)
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        private LoginResult GenerateJwt(LoginUser user, List<Claim> claims, IList<string>? roles)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_settings.Value.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(6),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);

                var jwtToken = jwtTokenHandler.WriteToken(token);

                return new LoginResult
                {
                    Login = user.Login,
                    AccessToken = jwtToken,
                    Roles = roles
                };
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }
    }
}
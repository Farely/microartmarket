using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Controllers.RequestModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using SharedData;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly DataContext _dataContext;
        private IOptions<UserValidation> _settings;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ILogger<UsersController> _logger;
        public UsersController(DataContext dataContext, UserManager<ApplicationUser> userManager,ILogger<UsersController> logger)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _logger = logger;

        }


        /// <summary>
        ///     Создает пользователя.
        /// </summary>
        /// <returns>Возвращает токен</returns>
        /// <response code="200">Возвращает токен</response>
        /// <response code="401">Авторизация не удалась</response>
        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUser newUser)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var user = new ApplicationUser();
                user.DisplayName = newUser.Login;
                user.UserName = newUser.Login;
                var task = await _userManager.CreateAsync(user, newUser.Password);
                if (!task.Succeeded) return BadRequest(task.Errors);
                if (!newUser.ArtTags.Select(x => x.Id)
                    .All(id => _dataContext.ArtTags.Select(validId => validId.ArtTagId).Contains(id)))
                    return BadRequest("Ошибка в Id тегов");

                var currentUser = await _userManager.FindByNameAsync(newUser.Login);

                currentUser.Tags = newUser.ArtTags.Select(tag => new UserArtTag
                    {Rate = tag.Rate, ArtTag = _dataContext.ArtTags.Find(tag.Id)}).ToList();

                if (newUser.IsArtist)
                {
                    await _userManager.AddClaimAsync(currentUser, new Claim("app_usertype", "Artist"));
                    await _userManager.AddToRoleAsync(currentUser, "Artist");
                }
                else
                {
                    await _userManager.AddClaimAsync(currentUser, new Claim("app_usertype", "Customer"));
                    await _userManager.AddToRoleAsync(currentUser, "Customer");
                }

                return Ok(task.Succeeded);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }


        /// <response code="401">Авторизация не удалась</response>
        [HttpGet("search")]
        [Authorize(Roles = "admin")]
        public ActionResult<List<ApplicationUser>> GetAllUsers()
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            return _userManager.Users.ToList();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }
    }
}
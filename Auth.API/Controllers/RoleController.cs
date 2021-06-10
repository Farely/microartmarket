using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedData;

namespace Auth.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/users/roles")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RolesController> _logger;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                return Ok(await _roleManager.Roles.ToListAsync());
            }
        
        catch (Exception e)
        {
            _logger.LogCritical(e.ToString());
            throw;
        }
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return Ok();
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(name);
        }
        
        catch (Exception e)
        {
            _logger.LogCritical(e.ToString());
            throw;
        }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    IdentityResult result = await _roleManager.DeleteAsync(role);
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }


        [HttpPut]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return Ok();
            }

            return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }
    }
}
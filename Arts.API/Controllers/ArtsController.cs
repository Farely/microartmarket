using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Arts.API.Controllers.MapModels;
using Arts.API.Controllers.RequestModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using SharedData;

namespace Arts.API.Controllers
{
    [ApiController]
    [Route("api/arts/gallery")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ArtsController : Controller
    {
        private readonly IWebHostEnvironment _env;


        private readonly ILogger<ArtsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;


        public ArtsController(ILogger<ArtsController> logger, IWebHostEnvironment env, DataContext dataContext,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _logger = logger;
            _env = env;
            _mapper = mapper;
            _dataContext = dataContext;
            _userManager = userManager;
        }


        /// <summary>
        ///     Получает работу по Id.
        /// </summary>
        /// <response code="200">Возвращает картину</response>
        /// <response code="400">Если картины нет</response>
        /// <response code="401">Нет авторизации</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public  async Task<ActionResult<ArtWorkView>> GetWork(int id)
        { 
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var work = await _dataContext.Works.SingleOrDefaultAsync(i => i.Id == id);
                if (work == null)
                {
                    return NoContent();
                }

                return Ok(work);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        /// <summary>
        ///     Удаляет работу по Id.
        /// </summary>
        /// <response code="200">Возвращает картину</response>
        /// <response code="400">Если картины нет</response>
        /// <response code="401">Нет авторизации</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ArtWorkView>> DeleteWork(int id)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var work = await _dataContext.Works.SingleOrDefaultAsync(i => i.Id == id);
            
                if (work == null)
                {
                    return NoContent();
                }
                _dataContext.Works.Remove(work);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }


        /// <summary>
        ///     Получает галерею
        /// </summary>
        /// <response code="200">Возвращает галерею</response>
        /// <response code="401">Нет авторизации</response>
        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ArtWorkView), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<ArtWorkView>>> Search([FromQuery] SearchGalley searchRequest)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var works = await _dataContext.Works.OrderBy(w => w.Id).Skip(searchRequest.startAt).Skip(searchRequest.skip)
                .Take(searchRequest.count).Include(u => u.Author).ToListAsync();
            return _mapper.Map<List<ArtWork>, List<ArtWorkView>>(works);
             }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        /// <summary>
        ///     Создает новый элемент галереи.
        /// </summary>
        /// <response code="200">Работа создана</response>
        /// <response code="401">Нет авторизации</response>
        [HttpPost("new")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Artist")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult> AddWork([FromForm] AddArtWork addRequest)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var originalFilename = Path.GetFileName(addRequest.Art.FileName);
            string fileId = Guid.NewGuid().ToString().Replace("-", "");
            var user = await GetCurrentUserAsync();
            var fileFormat = "." + addRequest.Art.FileName.Split(".")[1];
            var path = Path.Combine(_env.WebRootPath, user.UserName);
            Directory.CreateDirectory(path);
            path = Path.Combine(path, fileId + fileFormat);
            await using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await addRequest.Art.CopyToAsync(fileStream);
            }

            var artWork = new ArtWork
            {
                ArtState = addRequest.ArtState,
                Author = user,
                Label = addRequest.Label,
                Description = addRequest.Description,
                FileId = fileId,
                OriginalFileName = originalFilename,
                Path = path
            };
            var artWorkView = _mapper.Map<ArtWork, ArtWorkView>(artWork);
            await _dataContext.Works.AddAsync(artWork);
            await _dataContext.SaveChangesAsync();
            return Created(nameof(ArtWorkView), artWorkView);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        /// <summary>
        ///     Создает новый элемент галереи.
        /// </summary>
        /// <response code="200">Работа создана</response>
        /// <response code="401">Нет авторизации</response>
        [HttpPost("{id}/finish")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Artist")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult> FinishOrderedWork(int id, [FromForm] IFormFile artFile)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var ArtWork = await _dataContext.Works.Include(a => a.Author).SingleOrDefaultAsync(i => i.Id == id);


            var originalFilename = Path.GetFileName(artFile.FileName);
            string fileId = Guid.NewGuid().ToString().Replace("-", "");
            var user = await GetCurrentUserAsync();
            var fileFormat = "." + artFile.FileName.Split(".")[1];
            var path = Path.Combine(_env.WebRootPath, user.UserName);
            Directory.CreateDirectory(path);
            path = Path.Combine(path, fileId + fileFormat);
            await using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await artFile.CopyToAsync(fileStream);
            }

            ArtWork.FileId = fileId;
            ArtWork.OriginalFileName = originalFilename;
            ArtWork.Path = path;


            var artWorkView = _mapper.Map<ArtWork, ArtWorkView>(ArtWork);
            _dataContext.Works.Update(ArtWork);
            await _dataContext.SaveChangesAsync();
            return Created(nameof(ArtWorkView), artWorkView);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.ToString());
            throw;
        }
        }


        /// <summary>
        ///     Создает новый элемент галереи из заказа.
        /// </summary>
        /// <response code="200">Работа создана</response>
        /// <response code="401">Нет авторизации</response>
        [HttpPost("new/fromorder")]
        [Consumes("application/json")]
        //[Authorize(Roles = "Artist")]
        [Authorize(Policy = "OnlyFromOrders")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult> AddWorkFromOrder([FromBody] AddArtWorkFromOrder addRequest)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var user = await GetCurrentUserAsync();
            var artWork = new ArtWork
            {
                Author = user,
                Label = addRequest.Label,
                Description = addRequest.Description
            };
            var artWorkView = _mapper.Map<ArtWork, ArtWorkView>(artWork);
            await _dataContext.Works.AddAsync(artWork);
            await _dataContext.SaveChangesAsync();
            return Created(nameof(ArtWorkView), artWorkView);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }


        /// <summary>
        ///     Изменяет работу в галерее.
        /// </summary>
        /// <response code="200">Работа изменена</response>
        /// <response code="401">Нет авторизации</response>
        [HttpPut("work")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Artist")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult> PutWork([FromForm] AddArtWork addRequest)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var work = _mapper.Map<AddArtWork, ArtWork>(addRequest);
                _dataContext.Works.Update(work);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
        
        
        /// <summary>
        ///     Получает галерею
        /// </summary>
        /// <response code="200">Возвращает галерею</response>
        /// <response code="401">Нет авторизации</response>
        [HttpGet("stats")]
        [Authorize(Policy = "OnlyFromStats")]
        
        [ProducesResponseType(typeof(ArtWorkView), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<int>> GetStats()
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
               
                return _dataContext.Works.Count();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }
    }
}
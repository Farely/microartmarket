using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arts.API.Controllers.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using SharedData;

namespace Arts.API.Controllers
{
    [ApiController]
    [Route("api/arts/tags")]
    //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller
    {
        private readonly ILogger<ArtsController> _logger;

        private readonly DataContext _dataContext;


        public TagsController(ILogger<ArtsController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }


        /// <summary>
        /// Получает тег по Id.
        /// </summary>
        /// <response code="200">Возвращает тег</response>
        /// <response code="400">Если тега нет</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArtTag>> GetTag(int id)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var work = await _dataContext.ArtTags.SingleOrDefaultAsync(i => i.ArtTagId == id);
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
        ///     Удаляет тег по Id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWork(int id)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var work = await _dataContext.ArtTags.SingleOrDefaultAsync(i => i.ArtTagId == id);
            
                if (work == null)
                {
                    return NoContent();
                }
                _dataContext.ArtTags.Remove(work);
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
        ///     Обновляет тег.
        /// </summary>
        [HttpPut]
        [AllowAnonymous]
        public async Task <ActionResult> UpdateWork(ArtTag tag)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                _dataContext.ArtTags.Update(tag);
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
        ///     Получает теги работ.
        /// </summary>
        /// <response code="200">Возвращает теги</response>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ArtTag>>> Search([FromQuery] SearchGalley searchRequest)
        {
            var a = await _dataContext.ArtTags.OrderBy(tag => tag.ArtTagId).Skip(searchRequest.startAt)
                .Skip(searchRequest.skip)
                .Take(searchRequest.count)
                .ToListAsync();
            return a;
            ;
        }

        /// <summary>
        ///     Создает тег.
        /// </summary>
        /// <response code="200">Тег создан</response>
        /// <response code="401">Нет авторизации</response>
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> AddTag([FromBody] AddTag addRequest)
        {
            await _dataContext.AddAsync(new ArtTag {Description = addRequest.Description, Label = addRequest.Label});
            await _dataContext.SaveChangesAsync();
            return Ok();
        }
    }
}
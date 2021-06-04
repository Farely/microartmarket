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
        ///     Получает тег по Id.
        /// </summary>
        /// <response code="200">Возвращает картину</response>
        /// <response code="400">Если тега нет</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<ArtWork> GetTag(int id)
        {
            return StatusCode(500, "Нет реализации");
        }

        /// <summary>
        ///     Удаляет тег по Id.
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<ArtWork> DeleteWork(int id)
        {
            return StatusCode(500, "Нет реализации");
        }

        /// <summary>
        ///     Обновляет тег.
        /// </summary>
        [HttpPut]
        [AllowAnonymous]
        public ActionResult UpdateWork(ArtTag work)
        {
            return StatusCode(500, "Нет реализации");
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
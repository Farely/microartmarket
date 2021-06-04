using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using SharedData;

namespace Stats.API.Controllers
{
    [ApiController]
    [Route("api/stats/tags")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller
    {
        private readonly ILogger<TagsController> _logger;

        private readonly DataContext _dataContext;

        public TagsController(ILogger<TagsController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }


        /// <summary>
        ///     Получает статистику по работам.
        /// </summary>
        /// <response code="200">Возвращает картину</response>
        /// <response code="400">Если тега нет</response>
        [HttpGet("info/orders")]
        [AllowAnonymous]
        public ActionResult<ArtWork> GetOrders()
        {
            var workscount = _dataContext.Orders.Count();
            return Json(new
            {
                count = workscount
            });
        }


        /// <summary>
        ///     Получает статистику по работам.
        /// </summary>
        /// <response code="200">Возвращает картину</response>
        /// <response code="400">Если тега нет</response>
        [HttpGet("info/works")]
        [AllowAnonymous]
        public ActionResult<ArtWork> GetWorks()
        {
            var workscount = _dataContext.Works.Count();
            return Json(new
            {
                count = workscount
            });
        }


        /// <summary>
        ///     Получает статистику по тегам.
        /// </summary>
        /// <response code="200">Возвращает картину</response>
        /// <response code="400">Если тега нет</response>
        [HttpGet("info/tags")]
        [AllowAnonymous]
        public ActionResult<ArtWork> GetTag()
        {
            var tagcount = _dataContext.ArtTags.Count();
            return Json(new
            {
                count = tagcount
            });
        }
    }
}
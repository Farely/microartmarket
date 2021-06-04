using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Model;
using Newtonsoft.Json;
using Orders.API.Controllers.MapModels;
using SharedData;

namespace Orders.API.Controllers
{
    [Authorize(Policy = "JwtAuth")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;

        public OrdersController(ILogger<OrdersController> logger, IWebHostEnvironment env, DataContext dataContext,
            UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _env = env;
            _mapper = mapper;
            _dataContext = dataContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        ///     Получает заказ по Id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает последнюю картину</returns>
        /// <response code="201">Возвращает картину</response>
        /// <response code="400">Если картины нет</response>
        [HttpPut]
        public ActionResult<OrderNewView> Put()
        {
            return StatusCode(500, "Нет реализации");
        }


        /// <summary>
        ///     Получает заказ по Id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает последнюю картину</returns>
        /// <response code="201">Возвращает картину</response>
        /// <response code="400">Если картины нет</response>
        [HttpGet]
        public ActionResult<OrderNewView> Get()
        {
            return StatusCode(500, "Нет реализации");
        }


        /// <summary>
        ///     Создает заказ.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="orderArtWorkModel"></param>
        [HttpPost("new")]
        [Authorize(Roles = "Artist,Customer")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult> CreateOrder([FromBody] OrderNewView orderArtWorkModel)
        {
            var newOrder = new Order();
            var user = await GetCurrentUserAsync();
            newOrder.Label = orderArtWorkModel.Label;
            newOrder.DescriptionFromCustomer = orderArtWorkModel.DescriptionFromCustomer;
            newOrder.Customer = user;
            await SendToGalleryStartedWork(orderArtWorkModel);
            await _dataContext.Orders.AddAsync(newOrder);
            await _dataContext.SaveChangesAsync();
            return Created(nameof(OrderNewView), _mapper.Map<Order, OrderNewView>(newOrder));
        }

        /// <summary>
        ///     Завершает заказ.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="artFile"></param>
        [HttpPut("{id}/finish")]
        [Authorize(Roles = "Artist")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult> FinishOrder(int id, [FromForm] OrderEndedView artFile)
        {
            var order = await _dataContext.Orders.Include(a => a.Art).Include(
                q => q.Customer).SingleOrDefaultAsync(i => i.OrderId == id);
            if (order == null) return NotFound(new {Message = $"Заказа с id {id} нет."});

            if (order.IsOver) return BadRequest(new {Message = $"Заказ с id {id} уже закрыт"});
            var artWork = order.Art;
            await SendToGalleryEndedWork(artWork.Id, artFile.Art);
            //  order.IsOver = true;
            _dataContext.Orders.Update(order);
            await _dataContext.SaveChangesAsync();
            return Created(nameof(OrderNewView), _mapper.Map<Order, OrderNewView>(order));
        }


        private async Task SendToGalleryEndedWork(int id, IFormFile file)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            var test = accessToken.ToString();
            var client = new HttpClient();
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(id.ToString()), "id");
            byte[] filebytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                filebytes = ms.ToArray();
            }

            form.Add(new StreamContent(new MemoryStream(filebytes)));
            var url = _configuration.GetSection("GalleryUri").Value;
            var request = new HttpRequestMessage(HttpMethod.Post, $"{url}/{id}/finish");
            request.Content = form;
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(accessToken.ToString());

            var responce = await client.SendAsync(request);
            var result = await responce.Content.ReadAsStringAsync();
            client.Dispose();
        }


        private async Task SendToGalleryStartedWork(OrderNewView orderNewView)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            var test = accessToken.ToString();
            var client = new HttpClient();

            var addArtWork = new
            {
                orderNewView.Tags,
                orderNewView.Label,
                Description = orderNewView.DescriptionFromCustomer
            };
            var url = _configuration.GetSection("GalleryUri").Value;
            var request = new HttpRequestMessage(HttpMethod.Post, $"{url}new/fromorder");
            var json = JsonConvert.SerializeObject(addArtWork);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(accessToken.ToString());

            var responce = await client.SendAsync(request);
            var result = await responce.Content.ReadAsStringAsync();
            client.Dispose();
        }

        /*[HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<OrderEndedView>> Put([FromBody] OrderEndedView testToUpdate)
        {
            
            var artTags = await _dataContext.ArtTags.Select(ids => ids.ArtTagId).ToListAsync();
            if (!(testToUpdate.Tags
                .TrueForAll(tag => artTags.Contains(tag.Id))));
            {
                return  NotFound("Один из тегов ArtTags неправильно задан :(");
            }
           
        }*/


        [HttpDelete("{id}")]
        public ActionResult<Order> DeleteOrderByAsync(string id)
        {
            //ТУТ БУДЕТ УДАЛЕНИЕ
            var rng = new Random();
            return new Order {OrderId = 2};
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
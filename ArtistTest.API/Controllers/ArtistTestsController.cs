using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ArtistTest.API.Controllers.MapModels;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using SharedData;
using SharedData.TestModel;

namespace ArtistTest.API.Controllers
{
    [Authorize(Policy = "JwtAuth")]
    [Route("api/tests")]
    [ApiController]
    public class ArtistTestsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ArtistTestsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;

        public ArtistTestsController(ILogger<ArtistTestsController> logger, IWebHostEnvironment env,
            DataContext dataContext, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _logger = logger;
            _env = env;
            _mapper = mapper;
            _dataContext = dataContext;
            _userManager = userManager;
        }

        /// <summary>
        ///     Обновляет тест по Id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает последнюю картину</returns>
        /// <response code="201">Возвращает картину</response>
        /// <response code="400">Если картины нет</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult<TestView>> Put([FromBody] TestView testToUpdate)
        {
            var test = await _dataContext.Tests.SingleOrDefaultAsync(i => i.Id == testToUpdate.Id);

            if (test == null) return NotFound(new {Message = $"Тега с id {test.Id} нет."});
            var updatedTest = _mapper.Map<TestView, Test>(testToUpdate);
            var artTags = await _dataContext.ArtTags.Select(ids => ids.ArtTagId).ToListAsync();
            if (!test.Questions
                .ToList().TrueForAll(question => question.Answers.All(ans => artTags.Contains(ans.AnswerId))))
                return NotFound("Один из тегов ArtTags неправильно задан :(");

            _dataContext.Tests.Update(updatedTest);
            var nameOf = nameof(Test);
            await _dataContext.SaveChangesAsync();
            return CreatedAtAction(nameOf, new {id = test.Id}, null);
        }


        /// <summary>
        ///     Получает тест по Id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает последнюю картину</returns>
        /// <response code="201">Возвращает картину</response>
        /// <response code="400">Если картины нет</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TestView), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<TestView>> Get(int id)
        {
            var test = await _dataContext.Tests.SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            return _mapper.Map<Test, TestView>(test);
        }

        /// <summary>
        ///     Получает полный тест по Id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает последнюю картину</returns>
        /// <response code="201">Возвращает картину</response>
        /// <response code="400">Если картины нет</response>
        [HttpGet("{id:int}/full")]
        [ProducesResponseType(typeof(TestView), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<TestView>> GetFull(int id)
        {
            var test = await _dataContext.Tests.Include(e => e.Questions)
                .ThenInclude<Test, Question, ICollection<Answer>>(q => q.Answers)
                .ThenInclude<Test, Answer, ArtTag>(answer => answer.ArtTag).SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            return _mapper.Map<Test, TestView>(test);
        }


        
        /// <summary>
        ///     Создает тест
        /// </summary>
        /// <param name="item"></param>
        /// <param name="test"></param>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult> CreateTest([FromBody] TestView test)
        {
            var newTest = _mapper.Map<TestView, Test>(test);
            var artTags = await _dataContext.ArtTags.Select(ids => ids.ArtTagId).ToListAsync();
            
            
            
            
            if (!newTest.Questions
                .ToList().TrueForAll(question => question.Answers.ToList().TrueForAll(ans => artTags.Contains(ans.ArtTagId))))
                return NotFound("Один из тегов ArtTags неправильно задан :(");
            await _dataContext.Tests.AddAsync(newTest);
            await _dataContext.SaveChangesAsync();
            return Created(nameof(Test), newTest.Id);
        }
    }
}
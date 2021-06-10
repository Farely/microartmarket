using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ArtistTest.API.Controllers.MapModels;
using AutoMapper;
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
    public class ArtistTestsInteractionContoller : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ArtistTestsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;

        public ArtistTestsInteractionContoller(ILogger<ArtistTestsController> logger, IWebHostEnvironment env,
            DataContext dataContext, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _logger = logger;
            _env = env;
            _mapper = mapper;
            _dataContext = dataContext;
            _userManager = userManager;
        }

        /// <summary>
        /// Получает результат тест по Id.
        /// </summary>
        /// <param name="testId">Id теста</param>
        /// <param name="testResultId">Id результата теста</param>
        /// <returns>Возвращает последнюю картину</returns>
        /// <response code="200">Возвращает результаты теста</response>
        /// <response code="404">Если теста нет</response>
        /// <response code="204">Если тест есть, а результата нет</response>
        [HttpGet("{testId}/{testResultId}/result")]
        [ProducesResponseType(typeof(TestResult), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<TestResult>> GetResult(int testId, int testResultId)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var test = await _dataContext.Tests.SingleOrDefaultAsync(t => t.Id == testId);
            if (test == null) return NotFound(new {Message = $"Теста с id {testId} нет."});
            var testResult = await _dataContext.TestResult.SingleOrDefaultAsync(t => t.TestResultId == testResultId);
            if (testResult != null) return testResult;
            return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Создает ответ на вопрос теста
        /// </summary>
        ///  <param name="testId">Id теста</param>
        /// <param name="testResultId">Id результата теста</param>
        /// <param name="newAnswer">Новый ответ</param>
        [HttpPost("{testid}/{testResultId}/answer")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<ActionResult> CreateAnswerResult(int testId, int testResultId,
            [FromBody] AnswerResult newAnswer)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var test = await _dataContext.Tests.SingleOrDefaultAsync(t => t.Id == testId);
            if (test == null) return NotFound(new {Message = $"Теста с id {testId} нет."});
            var testResult = await _dataContext.TestResult.SingleOrDefaultAsync(t => t.TestResultId == testResultId);
            if (testResult == null) return NotFound(new {Message = $"Текущего теста с id {testResultId} нет."});
            if (testResult.IsEnded) return BadRequest(new {Message = $"Текущий тест с id {testResultId} закрыт."});
            newAnswer.TestResultId = testResultId;
            _dataContext.AnswerResult.Add(newAnswer);
            await _dataContext.SaveChangesAsync();
            return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Начинает тест
        /// </summary>
        /// <param name="id">Id теста</param>
        [HttpPost("{id}/start")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult> StartTest(int id)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var test = await _dataContext.Tests.Include(t => t.Questions)
                .ThenInclude<Test, Question, ICollection<Answer>>(q => q.Answers).SingleOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound(new {Message = $"Теста с id {test.Id} нет."});

            var testedUser = await GetCurrentUserAsync();
            if (testedUser == null) return StatusCode(500,"Нет такого пользователя (возможно, попытка запуска в режиме дебага)");

            var startedTest = new TestResult
            {
                Test = test,
                IsEnded = false,
                TestedUser = testedUser
            };
            _dataContext.TestResult.Add(startedTest);

            await _dataContext.SaveChangesAsync();
            var startedTestView = _mapper.Map<Test, TestView>(test);
            var resultTestView = _mapper.Map<TestResult, TestResultView>(startedTest);
            return Created(nameof(Test), new {resultTestView, startedTestView});
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                return StatusCode(500);
            }
        }

        /// <summary>
        ///  Заканчивает тест(закрывает результат теста и указывает лучшего автора)
        /// </summary>
        /// <param name="testId">Id теста</param>
        /// <param name="testResultId">Id результата теста</param>
        [HttpPut("{testId}/{testResultId}/end")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult> EndTest(int testId, int testResultId)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            var test = await _dataContext.Tests.SingleOrDefaultAsync(t => t.Id == testId);
            if (test == null) return NotFound(new {Message = $"Теста с id {testId} нет."});
            var testResult = await _dataContext.TestResult.Include(r => r.AnswerResults)
                .SingleOrDefaultAsync(t => t.TestResultId == testResultId);
            if (testResult == null) return NotFound(new {Message = $"Текущего теста с id {testResultId} нет."});
            if (testResult.IsEnded) return BadRequest(new {Message = $"Текущий тест с id {testResultId} закрыт."});

            if (testResult.AnswerResults == null)
                return BadRequest(new {Message = "Пользователь не ответил ни на один вопрос("});
            testResult.SuggestedAuthor = await GetBestArtist(testResult.AnswerResults.ToList());
            testResult.IsEnded = true;
            _dataContext.TestResult.Update(testResult);
            await _dataContext.SaveChangesAsync();
            var testResultView = _mapper.Map<TestResult, TestResultViewAnswers>(testResult);

            return Created(nameof(TestResultViewAnswers), testResultView);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.ToString());
            return StatusCode(500);
        }
        }


        /// <summary>
        /// Получает результаты по результатам теста
        /// </summary>
        /// <param name="results">Результат</param>
        private async Task<ApplicationUser> GetBestArtist(List<AnswerResult> results)
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
                var artists = new List<ApplicationUser>();
                var data = await _userManager.GetUsersInRoleAsync("Artist");
                foreach (var item in data)
                {
                    var newUser = _userManager.Users.Include(a => a.Tags).FirstOrDefault(u => u.Id == item.Id);
                    artists.Add(newUser);
                }

                var finalScore = results
                    .Select(result => new UserArtTag {Rate = result.Answer.Rate, ArtTag = result.Answer.ArtTag})
                    .ToList();
                var topScore = finalScore
                    .OrderBy(a => a.Rate)
                    .FirstOrDefault();
                if (topScore == null)
                {
                    var rand = new Random();
                    var toSkip = rand.Next(1, _dataContext.Users.Count());
                    return await _userManager.Users.Skip(toSkip).Take(1).FirstAsync();
                }

                var topUsers = artists
                    .Where(artist => artist.Tags.Contains(topScore));
                var topUser = topUsers.OrderBy(user =>
                    user.Tags.Where(tag => tag.ArtTag == topScore.ArtTag).Select(userRate => userRate.Rate)).First();
                return topUser;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }

        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            _logger.LogInformation(Request.Host.Value + Request.Path);
            try
            {
            return await _userManager.GetUserAsync(User);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.ToString());
                throw;
            }
        }
    }
}
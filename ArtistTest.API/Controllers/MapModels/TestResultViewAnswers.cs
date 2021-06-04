using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtistTest.API.Controllers.MapModels
{
    public class TestResultViewAnswers
    {
        [Key] public int TestResultId { get; set; }

        public int TestId { get; set; }

        public List<AnswerResultView> AnswerResults { get; set; }
        public bool IsEnded { get; set; }

        public UserView SuggestedAuthor { get; set; }
    }
}
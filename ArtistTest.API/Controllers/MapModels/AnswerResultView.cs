using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtistTest.API.Controllers.MapModels
{
    public class AnswerResultView
    {
        [Key] public int Id { get; set; }

        public AnswerView Answer { get; set; }

        [InverseProperty("AnswerResults")] public int TestResultId { get; set; }
    }
}
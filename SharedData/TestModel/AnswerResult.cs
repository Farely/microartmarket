using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData.TestModel
{
    public class AnswerResult
    {
        [Key] public int Id { get; set; }

        public Answer Answer { get; set; }

        [InverseProperty("AnswerResults")] public int TestResultId { get; set; }
    }
}
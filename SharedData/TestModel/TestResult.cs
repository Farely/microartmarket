using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData.TestModel
{
    public class TestResult
    {
        [Key] public int TestResultId { get; set; }

       public Test Test { get; set; }

        public ICollection<AnswerResult> AnswerResults { get; set; }
        public bool IsEnded { get; set; }
        public ApplicationUser TestedUser { get; set; }
        public ApplicationUser SuggestedAuthor { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData.TestModel
{
    public class Question
    {
        [Key] public int QuestionId { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual string Text { get; set; }
    }
}
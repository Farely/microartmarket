using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedData.TestModel
{
    public class Test
    {
        [Key] public int Id { get; set; }

        public virtual string Label { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<TestResult> Results { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedData.TestModel
{
    public class Answer
    {
        [Key] public int AnswerId { get; set; }
        public int ArtTagId { get; set; }
        public virtual ArtTag ArtTag { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        [Range(1, 5)] public virtual int Rate { get; set; }
        public virtual string Text { get; set; }
    }
}
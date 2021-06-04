using System.Collections.Generic;

namespace ArtistTest.API.Controllers.MapModels
{
    public class QuestionView
    {
        public int QuestionId { get; set; }

        public ICollection<AnswerView> Answers { get; set; }

        public string Text { get; set; }
    }
}
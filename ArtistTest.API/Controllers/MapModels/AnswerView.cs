using System.ComponentModel.DataAnnotations;

namespace ArtistTest.API.Controllers.MapModels
{
    public class AnswerView
    {
        public int AnswerId { get; set; }

        public int ArtTagId { get; set; }

        [Range(1, 5)] public int Rate { get; set; }

        public string Text { get; set; }
    }
}
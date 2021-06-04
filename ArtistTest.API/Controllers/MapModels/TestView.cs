using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtistTest.API.Controllers.MapModels
{
    public class TestView
    {
        public int Id { get; set; }

        [Required] public string Label { get; set; }

        [Required] public string Description { get; set; }

        [Required] public List<QuestionView> Questions { get; set; }
    }
}
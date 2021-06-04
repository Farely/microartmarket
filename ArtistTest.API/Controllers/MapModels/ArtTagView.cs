using System.ComponentModel.DataAnnotations;

namespace ArtistTest.API.Controllers.MapModels
{
    public class ArtTagView
    {
        [Required] public int ArtTagId { get; set; }
        
        public string Label { get; set; }
        public string Description { get; set; }
        
    }
}
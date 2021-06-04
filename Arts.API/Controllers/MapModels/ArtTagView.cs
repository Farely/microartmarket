using System.ComponentModel.DataAnnotations;

namespace Arts.API.Controllers.MapModels
{
    public class ArtTagView
    {
        [Required] public int Id { get; set; }
    }
}
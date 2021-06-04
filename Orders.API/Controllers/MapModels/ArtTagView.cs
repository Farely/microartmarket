using System.ComponentModel.DataAnnotations;

namespace Orders.API.Controllers.MapModels
{
    public class ArtTagView
    {
        [Required] public int Id { get; set; }
    }
}
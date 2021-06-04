using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Arts.API.Controllers.MapModels;

namespace Arts.API.Controllers.RequestModels
{
    public class AddArtWorkFromOrder
    {
        [Required] public List<ArtTagView> Tags { get; set; }

        [Required] public string Label { get; set; }

        [Required] public string Description { get; set; }
    }
}
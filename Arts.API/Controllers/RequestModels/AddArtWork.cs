using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Arts.API.Controllers.MapModels;
using Microsoft.AspNetCore.Http;
using SharedData;

namespace Arts.API.Controllers.RequestModels
{
    public class AddArtWork
    {
        public IFormFile Art { get; set; }

        [Required] public List<ArtTagView> Tags { get; set; }

        [Required] public string Label { get; set; }

        [Required] public string Description { get; set; }

        [Range((int) ArtState.Done, (int) ArtState.Started)]
        public ArtState ArtState { get; set; }
    }
}
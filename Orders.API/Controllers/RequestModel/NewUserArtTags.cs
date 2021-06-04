using System.ComponentModel.DataAnnotations;

namespace Orders.API.Controllers.RequestModel
{
    public class NewUserArtTags
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Label { get; set; }

        [StringLength(60, MinimumLength = 1)]
        [Required]

        public string Description { get; set; }
    }
}
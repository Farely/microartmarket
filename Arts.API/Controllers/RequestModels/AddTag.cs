using System.ComponentModel.DataAnnotations;

namespace Arts.API.Controllers.RequestModels
{
    public class AddTag
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Label { get; set; }

        [StringLength(60, MinimumLength = 1)]
        [Required]

        public string Description { get; set; }
    }
}
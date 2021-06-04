using System.ComponentModel.DataAnnotations;

namespace Arts.API.Controllers.RequestModels
{
    public class SearchGalley
    {
        public int startAt { get; set; } = 0;

        public int skip { get; set; } = 0;

        [Range(0, 50, ErrorMessage = "Value for {0} must  be between {1} and {2}.")]
        public int count { get; set; } = 1;
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Orders.API.Controllers.MapModels
{
    public class OrderEndedView
    {
        [Required] public IFormFile Art { get; set; }
    }
}
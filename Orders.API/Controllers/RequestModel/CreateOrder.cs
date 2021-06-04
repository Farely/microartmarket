using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orders.API.Controllers.RequestModel
{
    public class CreateOrder
    {
        [Required(ErrorMessage = "Не указан Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан Пароль")]
        public string Password { get; set; }

        public bool IsArtist { get; set; } = false;

        public List<NewUserArtTags> ArtTags { get; set; } = new();
    }
}
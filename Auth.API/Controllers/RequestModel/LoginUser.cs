using System.ComponentModel.DataAnnotations;

namespace Auth.Controllers.RequestModel
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Не указан Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан Пароль")]
        public string Password { get; set; }
    }
}
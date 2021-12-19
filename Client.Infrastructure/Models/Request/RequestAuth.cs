using System.ComponentModel.DataAnnotations;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestAuth
    {
        [Required(ErrorMessage = "Укажите логин")]
        [EmailAddress(ErrorMessage = "Укажите корректный адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        public string Password { get; set; }
    }
}

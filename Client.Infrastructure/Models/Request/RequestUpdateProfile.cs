using System.ComponentModel.DataAnnotations;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestUpdateProfile
    {
        [Required(ErrorMessage = "Укажите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Укажите Email")]
        [EmailAddress(ErrorMessage = "Укажите корректный Email")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; } = false;
    }
}

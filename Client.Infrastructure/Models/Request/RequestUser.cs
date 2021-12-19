using System.ComponentModel.DataAnnotations;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestUser
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

        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(8, ErrorMessage = "Минимум 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Пароль должен состоять из хотя бы 1 строчной буквы, 1 буквы верхнего регистра, 1 числа и иметь 1 специальный символ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; } = false;

        public string TenantId { get; set; }
    }

    public class RequestUserUpdate
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

        public string TenantId { get; set; }
    }
}
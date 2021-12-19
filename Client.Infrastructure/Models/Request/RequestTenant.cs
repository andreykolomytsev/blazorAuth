using System.ComponentModel.DataAnnotations;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestTenant
    {
        [Required(ErrorMessage = "Укажите наименование")]
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Укажите ИНН")]
        public string INN { get; set; }

        public string OGRN { get; set; }

        public string KPP { get; set; }

        public string OKPO { get; set; }
    }
}

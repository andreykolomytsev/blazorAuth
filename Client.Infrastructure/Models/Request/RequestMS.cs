using System.ComponentModel.DataAnnotations;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestMS
    {
        [Required(ErrorMessage = "Укажите наименование")]
        public string FullName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Укажите URL адрес")]
        [Url(ErrorMessage = "Некорректный адрес")]
        public string URL { get; set; }

        public string IP { get; set; }

        public string Port { get; set; }
    }
}

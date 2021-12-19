using System.ComponentModel.DataAnnotations;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestRole
    {
        [Required(ErrorMessage = "Укажите наименование")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
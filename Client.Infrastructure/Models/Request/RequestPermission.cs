using System.ComponentModel.DataAnnotations;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestPermission
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}

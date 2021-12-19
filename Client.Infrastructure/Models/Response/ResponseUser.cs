using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponseUser
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool ExistChild { get; set; }
    }
}
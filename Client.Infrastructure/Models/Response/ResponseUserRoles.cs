using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponseUserRoles
    {
        public List<UserRoleModel> UserRoles { get; set; } = new();
    }

    public class UserRoleModel
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public bool Selected { get; set; }
    }
}
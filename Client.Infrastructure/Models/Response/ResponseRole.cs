namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponseRole
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ResponseTenant Tenant { get; set; }
    }
}
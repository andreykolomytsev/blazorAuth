namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponsePermission
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public bool Selected { get; set; } = true;
    }
}

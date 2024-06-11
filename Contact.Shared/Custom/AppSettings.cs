namespace Contact.Shared.Custom
{
    public class AppSettings
    {
        public List<string> AllowedDomains { get; set; } = new List<string>();
        public EmailClient EmailClient { get; set; }
    }

    public class EmailClient
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public Credentials Credentials { get; set; }
    }

    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

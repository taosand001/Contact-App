using Contact.Shared.Custom;
using MailKit.Net.Smtp;
using MimeKit;

namespace Contact.Infrastructure.Helpers
{
    public static class EmailService
    {
        private static EmailClient _emailClient;

        public static void InitializeEmailClient(EmailClient emailClient)
        {
            _emailClient = emailClient;
        }
        private static string GeneratePasswordVerificationEmail(string username, string token)
        {
            return $"<h1>Hi {username},</h1><br /><p>This is the code to change your password. Enter this code <strong>{token}</strong></p>";
        }

        public static void EmailClient(MimeMessage message)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(_emailClient.Host, _emailClient.Port, _emailClient.EnableSsl);
                    client.Authenticate(_emailClient.Credentials.Email, _emailClient.Credentials.Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void SendPasswordVerificationEmail(string username, string email, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Contact Application Management", _emailClient.Credentials.Email));
            message.To.Add(new MailboxAddress("Contact App client", email));
            message.Subject = "Password Reset";
            message.Body = new TextPart("html")
            {
                Text = GeneratePasswordVerificationEmail(username, token)
            };
            EmailClient(message);
        }
    }
}

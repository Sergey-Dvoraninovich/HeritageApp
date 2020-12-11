using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace HeritageWebApplication.Services
{
    public class EmailService : IEmailService
    {
        private string sender;
        private string smtpServer;
        private int smtpPort;
        private string username;
        private string password;
        public EmailService(string sender, string smtpServer, int smtpPort, string username, string password)
        {
            this.sender = sender;
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.username = username;
            this.password = password;
        }
        
        [Obsolete("Do not use this in Production code!!!",true)]
        public static void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
        }
        
        public async Task SendEmailAsync(string receiver, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(sender));
            emailMessage.To.Add(new MailboxAddress(receiver));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
             
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        /*
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "heritage-app-adm1n@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
             
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.AuthenticateAsync("heritage-app-adm1n@yandex.ru", "123admin123");
                await client.SendAsync(emailMessage);
 
                await client.DisconnectAsync(true);
            }
        }
        */
    }
}
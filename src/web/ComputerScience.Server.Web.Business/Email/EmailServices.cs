using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace ComputerScience.Server.Web.Business.Email
{
    public class SmtpEmailService : IEmailService
    {
        private SmtpClient Client { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Host { get; set; }

        public SmtpEmailService(string host, string username, string password)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            Client = new SmtpClient();
            Username = username;
            Password = password;
            Host = host;
        }

        public async Task SendEmailAsync(IEmailTemplate template, string subject, string recepient, CancellationToken cancellationToken)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (string.IsNullOrEmpty(recepient))
                throw new ArgumentNullException(nameof(recepient));
            if (!Client.IsConnected)
                await Client.ConnectAsync(Host, 587, true, cancellationToken);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DVHS Computer Science Club", "noreply@dvhscs.com"));
            message.To.Add(new MailboxAddress("", recepient));
            message.Subject = subject;
            var htmlMessage = new BodyBuilder
            {
                HtmlBody = await template.RenderAsync(cancellationToken),
                TextBody = await template.RenderAsync(cancellationToken)
            };
            message.Body = htmlMessage.ToMessageBody();
            await Client.AuthenticateAsync(Username, Password, cancellationToken);
            await Client.SendAsync(message, cancellationToken);
        }
    }
}

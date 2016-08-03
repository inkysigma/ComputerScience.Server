using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Business.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(IEmailTemplate template, string subject, string recepient, CancellationToken cancellationToken);
    }
}
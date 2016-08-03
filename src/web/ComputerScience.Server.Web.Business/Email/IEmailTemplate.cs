using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Business.Email
{
    public interface IEmailTemplate
    {
        void Inject(dynamic properties);
        Task<string> RenderAsync(CancellationToken cancellationToken);
        Task<string> PlainRenderAsync(CancellationToken cancellationToken);
    }
}

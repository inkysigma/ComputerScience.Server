using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Business.Solutions
{
    public interface ISolutionService<TSolution>
    {
        Task<TSolution> FetchSolutionSet(string id, CancellationToken cancellationToken);
        Task<SolutionServiceResult> AddSolutionSet(string guid, TSolution solution, CancellationToken cancellationToken);
        Task<SolutionServiceResult> FinalizeSolutionSet(string id, CancellationToken cancellationToken);
    }
}

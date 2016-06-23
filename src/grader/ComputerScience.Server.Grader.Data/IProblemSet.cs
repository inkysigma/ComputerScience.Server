using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader.Data
{
    public interface IProblemSet<TProblem>
    {
        Task<TProblem> FetchProblemAsync(string id, CancellationToken cancellationToken);
    }
}
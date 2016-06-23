using System.Threading.Tasks;

namespace ComputerScience.Server.Grader.Data
{
    public interface ISolutionCache<TSolution>
    {
        Task<TSolution> Fetch();
    }
}
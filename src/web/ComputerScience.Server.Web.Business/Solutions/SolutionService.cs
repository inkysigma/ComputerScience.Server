using System;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Data.SolutionCache;
using ComputerScience.Server.Web.Data.SolutionSet;
using ComputerScience.Server.Web.Models;
using ComputerScience.Server.Web.Models.Solutions;

namespace ComputerScience.Server.Web.Business.Solutions
{ 
    public class SolutionService<TSolution> : ISolutionService<TSolution>
    {
        public ISolutionCache<TSolution> SolutionCache { get; }
        public ISolutionSet<TSolution> SolutionSet { get; }
        public ISolutionValidator<TSolution> SolutionValidator { get; }
        public SolutionServiceConfiguration Configuration { get; }

        public SolutionService(ISolutionCache<TSolution> solutionCache, ISolutionSet<TSolution> solutionSet,
            ISolutionValidator<TSolution> validator, SolutionServiceConfiguration configuration)
        {
            SolutionCache = solutionCache;
            SolutionSet = solutionSet;
            SolutionValidator = validator;
            Configuration = configuration;
        }

        public async Task<TSolution> FetchSolutionSet(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            cancellationToken.ThrowIfCancellationRequested();
            return await SolutionSet.FetchSolutionMetdataAsync(id, cancellationToken);
        }

        public async Task<SolutionServiceResult> AddSolutionSet(string guid, TSolution solution, CancellationToken cancellationToken)
        {
            if (solution == null)
                throw new ArgumentNullException(nameof(solution));
            cancellationToken.ThrowIfCancellationRequested();
            await SolutionSet.ClearSolutionMetadaAsync(cancellationToken);
            if (await SolutionSet.FetchSizeAsync(cancellationToken) > Configuration.MaxSet)
                return SolutionServiceResult.Full;
            await SolutionSet.AddSolutionMetadataAsync(guid, solution, cancellationToken);
            return SolutionServiceResult.Success;
        }

        public async Task<SolutionServiceResult> FinalizeSolutionSet(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            cancellationToken.ThrowIfCancellationRequested();
            if (await SolutionCache.FetchSizeAsync(cancellationToken) > Configuration.MaxCache)
                return SolutionServiceResult.Full;
            var solution = await SolutionSet.FetchSolutionMetdataAsync(id, cancellationToken);
            var result = SolutionValidator.Validate(solution, cancellationToken);
            if (result == ValidationResult.Incomplete)
                return SolutionServiceResult.Incomplete;
            if (result == ValidationResult.Invalid)
            {
                await SolutionSet.RemoveSolutionMetadataAsync(id, cancellationToken);
                return SolutionServiceResult.StartOver;
            }
            await SolutionCache.PutAsync(solution, cancellationToken);
            await SolutionSet.RemoveSolutionMetadataAsync(id, cancellationToken);
            return SolutionServiceResult.Success;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using ComputerScience.Server.Web.Business.Problems;
using Microsoft.AspNetCore.Mvc;

namespace ComputerScience.Server.Web.Controllers
{
    public class ProblemController : Controller
    {
        public IProblemService<Problem> ProblemService { get; set; }

        public ProblemController(IProblemService<Problem> problemService)
        {
            ProblemService = problemService;
        }

        [HttpPost]
        public async Task<Problem> FetchProblem(string id, CancellationToken cancellationToken)
        {
            return await ProblemService.FetchProblemAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<IEnumerable<Problem>> FetchRandomProblems(CancellationToken cancellationToken)
        {
            return await ProblemService.FetchRandomProblemsAsync(10, cancellationToken);
        }
    }
}

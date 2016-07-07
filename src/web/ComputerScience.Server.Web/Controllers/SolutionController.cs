using ComputerScience.Server.Web.Models.Exception;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Business.Problems;
using ComputerScience.Server.Web.Business.Solutions;
using ComputerScience.Server.Web.Configuration;
using ComputerScience.Server.Web.Models.Problems;
using ComputerScience.Server.Web.Models.Requests;
using ComputerScience.Server.Web.Models.Response;
using ComputerScience.Server.Web.Models.Solutions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ComputerScience.Server.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SolutionController : Controller
    {
        public IHostingEnvironment HostingEnvironment { get; }
        public SolutionService<Solution> SolutionService { get; set; }
        public ProblemService<Problem> ProblemService { get; set; }
        public SolutionConfiguration Configuration { get; set; }

        public SolutionController(SolutionService<Solution> service, IHostingEnvironment environment, SolutionConfiguration configuration)
        {
            SolutionService = service;
            HostingEnvironment = environment;
            Configuration = configuration;
        }

        [HttpGet]
        public string Test()
        {
            return "Hello";
        }

        [HttpPost]
        public async Task<RequestProblemSubmission> RequestSolutionSubmission(SolutionSubmission submission, CancellationToken token)
        {
            if (submission == null)
                throw new WebArgumentException(nameof(submission), nameof(RequestProblemSubmission), null);
            var guid = Guid.NewGuid().ToString();
            var path = Path.Combine(Configuration.FileLocation, guid);

            if (!await ProblemService.Exists(submission.ProblemId, token))
                return new RequestProblemSubmission
                {
                    Result = SolutionServiceResult.StartOver
                };
            var solution = new Solution
            {
                Id = guid,
                ProblemId = submission.ProblemId,
                TimeStamp = (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                FileLocation = path,
                User = User.Identity.Name,
                SolutionType = submission.SolutionType
            };
            var result = await SolutionService.AddSolutionSet(guid, solution, token);
            var requestResult = new RequestProblemSubmission {Result = result};
            if (result == SolutionServiceResult.Success)
                requestResult.Guid = guid;
            return requestResult;
        }

        [HttpPost]
        public async Task<PostFileResponse> PostFile(string guid, IFormFile file, CancellationToken token)
        {
            if (string.IsNullOrEmpty(guid))
                throw new WebArgumentException(nameof(PostFile), nameof(guid), guid);

            if (file.Length > Configuration.FileSize)
                return PostFileResponse.FileSizeError;

            var solutionSet = await SolutionService.FetchSolutionSet(guid, token);

            if (solutionSet == null)
                return PostFileResponse.IdentificationError;

            var problemSet = await ProblemService.FetchProblemAsync(solutionSet.ProblemId, token);

            if (problemSet == null)
                return PostFileResponse.ProblemIdentificationError;

            if (file.Length > problemSet.SolutionSize)
                return PostFileResponse.FileSizeError;

            using (var writer = new FileStream(Path.Combine(Configuration.FileLocation, file.FileName), FileMode.CreateNew))
            {
                await file.CopyToAsync(writer, token);
                await writer.FlushAsync(token);
            }

            var result = await SolutionService.FinalizeSolutionSet(guid, token);

            if (result == SolutionServiceResult.Full)
                return PostFileResponse.Full;
            if (result == SolutionServiceResult.Incomplete || result == SolutionServiceResult.StartOver)
                return PostFileResponse.Failure;

            return PostFileResponse.Success;
        }
    }
}

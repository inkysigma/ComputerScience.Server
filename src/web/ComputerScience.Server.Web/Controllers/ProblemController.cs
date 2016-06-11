using ComputerScience.Server.Web.Models.Exception;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Business.Solutions;
using ComputerScience.Server.Web.Configuration;
using ComputerScience.Server.Web.Models.Requests;
using ComputerScience.Server.Web.Models.Response;
using ComputerScience.Server.Web.Models.Solutions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ComputerScience.Server.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProblemController : Controller
    {
        public IHostingEnvironment HostingEnvironment { get; }
        public SolutionService<Solution> SolutionService { get; set; }
        public SolutionConfiguration Configuration { get; set; }

        public ProblemController(SolutionService<Solution> service, IHostingEnvironment environment, SolutionConfiguration configuration)
        {
            SolutionService = service;
            HostingEnvironment = environment;
        }

        [HttpPost]
        public async Task<RequestProblemSubmission> RequestProblemSubmission(ProblemSubmission submission)
        {
            if (submission == null)
                throw new WebArgumentException(nameof(submission), nameof(RequestProblemSubmission));
            var guid = Guid.NewGuid().ToString();
            var path = Path.Combine(Configuration.FileLocation, guid);
            var solution = new Solution()
            {
                Id = guid,
                ProblemId = submission.ProblemId,
                TimeStamp = (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                FileLocation = path
            };
            var result = new RequestProblemSubmission
            {
                Result = await SolutionService.AddSolutionSet(solution, CancellationToken.None)
            };
            if (result.Result == SolutionServiceResult.Success)
                result.Guid = guid;
            return result;
        }

        [HttpPost]
        public async Task<PostFileResponse> PostFile(string guid, IFormFile file)
        {
            if (string.IsNullOrEmpty(guid))

            if (file.Length > Configuration.FileSize)
                return PostFileResponse.FileSizeError;

            

            return PostFileResponse.Success;
        }
    }
}

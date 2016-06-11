using ComputerScience.Server.Web.Business.Solutions;

namespace ComputerScience.Server.Web.Models.Response
{
    public class RequestProblemSubmission
    {
        public SolutionServiceResult Result { get; set; }
        public string Guid { get; set; }
    }
}

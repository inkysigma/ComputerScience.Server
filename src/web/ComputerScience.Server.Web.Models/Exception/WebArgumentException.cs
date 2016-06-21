using ComputerScience.Server.Common;

namespace ComputerScience.Server.Web.Models.Exception
{
    public class WebArgumentException : CommonException
    {
        public WebArgumentException(string parameter, string action, string value)
            : base(
                400, true, "Something seems to have went wrong. Please check again later.",
                parameter, $"Request for {action} has {parameter} incomplete and has value {value}. " +
                           $"Please check api docs.")
        {

        }
    }
}

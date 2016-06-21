using System.IO;
using System.Threading;

namespace ComputerScience.Server.Web.Models.Solutions
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SolutionValidator : ISolutionValidator<Solution>
    {
        public ValidationResult Validate(Solution solution, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(solution.FileLocation) || string.IsNullOrEmpty(solution.Id) ||
                string.IsNullOrEmpty(solution.ProblemId) || string.IsNullOrEmpty(solution.User) || 
                solution.SolutionType == SolutionType.None)
                return ValidationResult.Invalid;
            if (!File.Exists(solution.FileLocation))
                return ValidationResult.Incomplete;
            return ValidationResult.Valid;
        }
    }
}

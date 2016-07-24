using System.IO;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Web.Models.Problems
{
    public class ProblemValidator
    {
        public ValidationResult Validate(Problem problem)
        {
            if (string.IsNullOrEmpty(problem.Id) || string.IsNullOrEmpty(problem.ProblemPath)
                || string.IsNullOrEmpty(problem.ProblemStatement) || string.IsNullOrEmpty(problem.Title))
                return ValidationResult.Invalid;
            for (var i = 0; i < problem.TestCases; i++)
                if (!File.Exists(Path.Combine(problem.ProblemPath, $"{i}.in")) || !File.Exists(Path.Combine(problem.ProblemPath, $"{i}.out")))
                    return ValidationResult.Incomplete;
            return ValidationResult.Valid;
        }
    }
}

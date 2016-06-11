using System.Threading;

namespace ComputerScience.Server.Web.Models.Solutions
{
    public interface ISolutionValidator<in TSolution>
    {
        ValidationResult Validate(TSolution solution, CancellationToken cancellationToken);
    }
}
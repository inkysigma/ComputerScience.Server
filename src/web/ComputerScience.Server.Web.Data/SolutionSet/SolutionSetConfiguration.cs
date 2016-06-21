using System;

namespace ComputerScience.Server.Web.Data.SolutionSet
{
    public class SolutionSetConfiguration
    {
        public TimeSpan SolutionSetSpan { get; set; } = TimeSpan.FromHours(1);

        public long SolutionSetTime => (long) SolutionSetSpan.TotalSeconds;
    }
}

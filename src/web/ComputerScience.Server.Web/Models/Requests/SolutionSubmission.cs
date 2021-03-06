﻿using ComputerScience.Server.Web.Models.Solutions;

namespace ComputerScience.Server.Web.Models.Requests
{
    public class SolutionSubmission
    {
        public string ProblemId { get; set; }

        public SolutionType SolutionType { get; set; }

        public bool IsValid => !(string.IsNullOrEmpty(ProblemId));
    }
}

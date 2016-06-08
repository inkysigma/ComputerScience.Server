using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Configuration
{
    public class SolutionConfiguration
    {
        public string FileLocation { get; set; }

        public long FileSize { get; set; } = 1500000;
    }
}

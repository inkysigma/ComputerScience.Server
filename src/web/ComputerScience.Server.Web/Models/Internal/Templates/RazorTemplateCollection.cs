using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Models.Internal.Templates
{
    public class RazorTemplateCollection
    {
        public RazorAccountVerificationEmailTemplate VerificationTemplate { get; set; }

        public RazorTemplateCollection(RazorAccountVerificationEmailTemplate verification)
        {
            VerificationTemplate = verification;
        }
    }
}

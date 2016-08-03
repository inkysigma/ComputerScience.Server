using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Routing;

namespace ComputerScience.Server.Web.Models.Internal.Templates
{
    public class RazorEmailTemplate
    {
        protected IRazorViewEngine Engine { get; set; }
        protected ITempDataProvider Provider { get; set; }
        protected IServiceProvider Services { get; set; }

        public RazorEmailTemplate(IRazorViewEngine engine, ITempDataProvider provider, IServiceProvider services)
        {
            Engine = engine;
            Provider = provider;
            Services = services;
        }

        protected ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext {RequestServices = Services};
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}

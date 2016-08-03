using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Business.Email;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ComputerScience.Server.Web.Models.Internal.Templates
{
    public class RazorAccountVerificationEmailTemplate : RazorEmailTemplate, IEmailTemplate
    {
        private string File { get; set; }
        private dynamic Properties { get; set; }

        public RazorAccountVerificationEmailTemplate(IRazorViewEngine engine, 
            ITempDataProvider provider, 
            IServiceProvider services,
            string file) : 
            base(engine, provider, services)
        {
            File = file;
        }

        public void Inject(dynamic properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));
            if (properties.Token == null)
                throw new ArgumentNullException(nameof(properties.Token));
            if (properties.Username == null)
                throw new ArgumentNullException(nameof(properties.Username));
            Properties = properties;
        }

        public async Task<string> RenderAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var actionContext = GetActionContext();

            var viewEngineResult = Engine.FindView(actionContext, File, false);

            if (!viewEngineResult.Success)
            {
                throw new IOException($"Couldn't find view {File}");
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = Properties
                };

                var viewContext = new ViewContext(
                    actionContext,
                    view, 
                    viewDictionary,
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        Provider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }

        public Task<string> PlainRenderAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult($"Please enter {Properties.Token} at dvhscs.com/Account/Activate");
        }
    }
}

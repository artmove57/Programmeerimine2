using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Threading.Tasks;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public class DisableAntiforgeryFilter : IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var antiforgery = context.HttpContext.RequestServices.GetService(typeof(IAntiforgery)) as IAntiforgery;
            if (antiforgery != null)
            {
                context.HttpContext.Features.Set<IAntiforgeryValidationFeature>(new AlwaysValidAntiforgeryFeature());
            }

            return Task.CompletedTask;
        }

        private class AlwaysValidAntiforgeryFeature : IAntiforgeryValidationFeature
        {
            public bool IsValid => true;
            public Exception Error => null;
        }
    }
}

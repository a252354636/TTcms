using TTcms.Infrastructure.Exceptions;
using TTcms.Infrastructure.Utilities;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TTcms.Infrastructure.Core.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TTcmsApiAuthorizeAttribute : AuthorizeAttribute
    {
        public string Name { get; set; }

        private ITTcmsAuthorize _freeBirdAuthorize => EngineContext.Current.Resolve<ITTcmsAuthorize>();

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
            {
                return false;
            }
            if (!_freeBirdAuthorize.IsAuthorized(actionContext, this.Name))
            {
                throw new AuthorizationException();
            }
            return true;
        }

        private static string GetUserId(IIdentity identity)
        {
            Guard.ArgumentNotNull(identity, nameof(identity));
            ClaimsIdentity identity2 = identity as ClaimsIdentity;
            if (identity2 != null)
            {
                return identity2.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return null;
        }

    }
}
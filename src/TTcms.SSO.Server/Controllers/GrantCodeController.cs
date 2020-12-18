using TTcms.Infrastructure.Core;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using TTcms.SSO.Server.Results;
using System.Web.Http;
using static TTcms.SSO.Server.Setting;

namespace TTcms.SSO.Server.Controllers
{
    [RoutePrefix("GrantCode")]
    public class GrantCodeController : ApiController
    {
        [Route("Authorize")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Authorize()
        {
            var OAuthOptions = EngineContext.Current.Resolve<OAuthAuthorizationServerOptions>();
            if (User.Identity == null || User.Identity.IsAuthenticated == false)
            {
                return new ChallengeResult(DefaultAuthenticationTypes.ApplicationCookie, this);
            }
            return Redirect(Url.Content("~/") + EndPointConfig.AuthorizeGrant + Request.RequestUri.Query);
        }

    }
}

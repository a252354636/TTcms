using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TTcms.Infrastructure.Mvc;
using TTcms.Web.Results;

namespace TTcms.Web.Controllers
{
    public class HomeController : BaseMvcController
    {
        private string _provider = "TTcmsSSOAuthentication";

        private const string ExternalCookie = "ExternalCookie";

        public ActionResult Index()
        {
            if (User.Identity == null || User.Identity.IsAuthenticated == false)
            {

            }
            var indentity = Authentication.GetExternalIdentity(ExternalCookie);
            if (indentity == null)
            {
                //return new ChallengeResult(_provider, this);
                return Json("未登陆sso");
            }
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(indentity as ClaimsIdentity);
            if (externalLogin == null)
            {
                return Json("未登陆");
            }

            //注销凭证
           // Authentication.SignOut("TTcmsSSOLoginData");

            //登录逻辑
            //...

            return Json($"测试应用通过{externalLogin.LoginProvider}获取到用户:{externalLogin.UserName}信息");
        
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, UserID, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                var idClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (idClaim == null || String.IsNullOrEmpty(idClaim.Issuer)
                    || String.IsNullOrEmpty(idClaim.Value))
                {
                    return null;
                }

                if (idClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = idClaim.Issuer,
                    UserID = idClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

    }
}
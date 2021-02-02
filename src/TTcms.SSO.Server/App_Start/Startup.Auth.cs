﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TTcms.Infrastructure.Core;
using TTcms.Infrastructure.OAuth;
using TTcms.SSO.Server.Code.OAuth;
using static TTcms.SSO.Server.Setting;

namespace TTcms.SSO.Server
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // 有关配置身份验证的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // 将数据库上下文和用户管理器配置为对每个请求使用单个实例

            // 使应用程序可以使用 Cookie 来存储已登录用户的信息
            // 并使用 Cookie 来临时存储有关使用第三方登录提供程序登录的用户的信息
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                CookieName = "TTcmsSSOLoginData",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login")
            });
            //app.Use(async (context, next) => {
            //    context.Response.Headers.Set("Access-Control-Allow-Origin", "*");
            //    await next();
            //});
            // 针对基于 OAuth 的流配置应用程序
            OAuthOptions = EngineContext.Current.Resolve<OAuthAuthorizationServerOptions>();

            // 使应用程序可以使用不记名令牌来验证用户身份
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}

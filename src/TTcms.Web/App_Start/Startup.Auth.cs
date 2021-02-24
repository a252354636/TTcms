﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TTcms.Owin.Security.SSO;

namespace TTcms.Web
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
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var TTcmsSSOOption = new SSOAccountAuthenticationOptions
            {
                ClientId = "3",
                ClientSecret = "123",
                CallbackPath = new PathString("/login/signin-ttcmssso"),
                TokenEndpoint = "http://localhost:8550/token",
                AuthorizationEndpoint = "http://localhost:8550/GrantCode/Authorize",
                UserInformationEndpoint = "http://localhost:8550/TicketUser/GetTicketMessage"
            };

            TTcmsSSOOption.Scope.Add("user-base");
            app.UseSSOAccountAuthentication(TTcmsSSOOption);

            app.Use(async (context, next) => {
                context.Response.Headers.Set("Access-Control-Allow-Origin", "*");
                await next();
            });
            // 取消注释以下行可允许使用第三方登录提供程序登录
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}

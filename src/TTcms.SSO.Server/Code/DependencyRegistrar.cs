using Autofac;
using TTcms.Infrastructure.Core;
using TTcms.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Reflection = System.Reflection;
using System.Web;
using TTcms.Application.System;
using TTcms.Infrastructure.Core.DependencyManagement;
using Autofac.Integration.Mvc;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter.AutoMapper;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter;
using System.Data.Entity;
using TTcms.EFRepositories;
using TTcms.Infrastructure.Caching;
using TTcms.Infrastructure.Domain.Uow;
using TTcms.Infrastructure.Domain.Repositories;
using TTcms.Application.Core;
using Autofac.Integration.WebApi;
using TTcms.SSO.Server.Code.Authorize;
using TTcms.Infrastructure.Core.Authorize;
using TTcms.Infrastructure.OAuth;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using TTcms.SSO.Server.Code.OAuth;
using static TTcms.SSO.Server.Setting;


namespace TTcms.SSO.Server.Code
{
    public partial class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c =>
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                null)
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();
            //ICurrentContext
            builder.RegisterType<CurrentContext>().As<ICurrentContext>().InstancePerLifetimeScope();
            //IFreeBirdAuthorize
            builder.RegisterType<SSOAuthorize>().As<ITTcmsAuthorize>().SingleInstance();
            //ITicketStore
            builder.RegisterType<CacheTicketStore>().As<ITicketStore>().WithStaticCache().SingleInstance();
            builder.RegisterType<CacheTicketMagage>().As<ITicketManage>().WithStaticCache().SingleInstance();

            // modules
            builder.RegisterModule(new DbModule(typeFinder));
            builder.RegisterModule(new CachingModule());

            // 服务
            RegisterService(builder, typeFinder);

            builder.Register(c => new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(EndPointConfig.TokenEndpointPath),
                Provider = new SSOOAuthProvider(),
                AuthorizeEndpointPath = new PathString(EndPointConfig.AuthorizeEndpointPath),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                RefreshTokenProvider = new AuthenticationTokenProvider()
                {
                    TokenType = "RefreshToken",
                    TokenKeepingPredicate = data => data.GrantType == GrantTypes.RefreshToken,
                    ExpireTimeSpan = TimeSpan.FromDays(60)
                },
                AccessTokenProvider = new AuthenticationTokenProvider()
                {
                    TokenType = "AccessToken",
                    ExpireTimeSpan = TimeSpan.FromHours(2)
                },
                AuthorizationCodeProvider = new AuthenticationTokenProvider()
                {
                    TokenType = "AuthorizationCode",
                    ExpireTimeSpan = TimeSpan.FromMinutes(15),
                    RemoveWhenReceive = true
                },
                //在生产模式下设 AllowInsecureHttp = false
#if DEBUG
                AllowInsecureHttp = true
#endif
            }).As<OAuthAuthorizationServerOptions>().SingleInstance();

            //控制器
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            //WebApi 
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly).InstancePerRequest();

            //类型转换注册
            builder.Register(n => TypeAdapterFactory.CreateAdapter()).As<ITypeAdapter>().SingleInstance();
        }

        public int Order
        {
            get { return 1; }
        }
    }

    #region Modules

    public class DbModule : Module
    {
        private readonly ITypeFinder _typeFinder;

        public DbModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MyDbContext>().AsSelf().As<DbContext>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().AsSelf().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }

    public class CachingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StaticCache>().As<ICache>().Keyed<ICache>(typeof(StaticCache)).SingleInstance();
            builder.RegisterType<RequestCache>().As<ICache>().Keyed<ICache>(typeof(RequestCache)).InstancePerRequest();

            builder.RegisterType<CacheManager<StaticCache>>()
                .As<ICacheManager>()
                .Named<ICacheManager>("static")
                .SingleInstance();
            builder.RegisterType<CacheManager<RequestCache>>()
                .As<ICacheManager>()
                .Named<ICacheManager>("request")
                .InstancePerRequest();

            // Register resolving delegate
            builder.Register<Func<Type, ICache>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return keyed => cc.ResolveKeyed<ICache>(keyed);
            });

            builder.Register<Func<string, ICacheManager>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return named => cc.ResolveNamed<ICacheManager>(named);
            });

            builder.Register<Func<string, Lazy<ICacheManager>>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return named => cc.ResolveNamed<Lazy<ICacheManager>>(named);
            });
        }
    }

    #endregion
}
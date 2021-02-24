using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TTcms.Application.System;
using TTcms.Infrastructure.Core;
using TTcms.Infrastructure.Core.DependencyManagement;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter.AutoMapper;

namespace TTcms.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ContainerManager _containerManager;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //CorsConfig.RegisterCors(GlobalConfiguration.Configuration);

            ////类型转换（automap）
            //TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());


            //var builder = new ContainerBuilder();

            ////dependencies
            //var typeFinder = new AppDomainTypeFinder();
            //builder = new ContainerBuilder();
            //builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            //builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            ////register dependencies provided by other assemblies
            //builder = new ContainerBuilder();
            //var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            //var drInstances = new List<IDependencyRegistrar>();
            //foreach (var drType in drTypes)
            //    drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            ////sort
            //drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //foreach (var dependencyRegistrar in drInstances)
            //    dependencyRegistrar.Register(builder, typeFinder);

            //var container = builder.Build();
            //this._containerManager = new ContainerManager(container);
            //HttpConfiguration config = GlobalConfiguration.Configuration;
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            ////set dependency resolver
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //类型转换（automap）
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            //初始化IOC
            EngineContext.Initialize(false);
        }
    }
}

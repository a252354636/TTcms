using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TTcms.Infrastructure.Core;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter;
using TTcms.Infrastructure.TypeUtilities.TypeAdapter.AutoMapper;

namespace TTcms.SSO.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //类型转换（automap）
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            //初始化IOC
            EngineContext.Initialize(false);
        }
    }
}

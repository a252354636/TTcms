using Autofac;
using TTcms.Infrastructure.Core;
using TTcms.Application.System;
using System.Linq;

namespace TTcms.Web.Code
{
    public partial class DependencyRegistrar
    {
        public void RegisterService(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            // 服务
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
           .Where(t => t.Name.EndsWith("Service")).AsSelf().InstancePerRequest();
        }

    }
}
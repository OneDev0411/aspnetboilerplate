﻿using System.Reflection;
using Abp.Modules.Core.Data.Repositories.Interceptors;
using Abp.Modules.Core.Data.Repositories.NHibernate;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Abp.Modules.Core.Startup.Dependency
{
    public class AbpCoreDataModuleDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                Component.For(typeof(AuditInterceptor)),
                Component.For(typeof(MultiTenancyInterceptor<,>)),

                //All repoistories //TODO: Move to Abp.Modules.Core.Data?
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NhUserRepository))).InSameNamespaceAs<NhUserRepository>().WithService.DefaultInterfaces().WithServiceSelf().LifestyleTransient()
                );
        }
    }
}

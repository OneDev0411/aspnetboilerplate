﻿using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Abp.Data.Repositories;
using Abp.Data.Repositories.NHibernate;
using Abp.Localization;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Taskever.Services;
using Taskever.Services.Impl;
using Taskever.Web.Api;

namespace Taskever.Web.Dependency.Installers
{
    public class TaskeverInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                //All MVC controllers
                Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient(),

                //All Web Api Controllers
                Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleTransient(),

                //Generic repositories
                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(NhRepositoryBase<>)).LifestyleTransient(),
                Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(NhRepositoryBase<,>)).LifestyleTransient(),

                //All repoistories //TODO: Make a custom repository example
                //Classes.FromAssembly(Assembly.GetAssembly(typeof(NhTaskRepository))).InSameNamespaceAs<NhTaskRepository>().WithService.DefaultInterfaces().LifestyleTransient(),

                //All services
                Classes.FromAssembly(Assembly.GetAssembly(typeof(TaskService))).InSameNamespaceAs<TaskService>().WithService.DefaultInterfaces().LifestyleTransient(),

                //Component.For(typeof(AbpServiceApiController<ITaskService>)).Proxy.AdditionalInterfaces(typeof(ITaskService)).Interceptors<AbpServiceApiControllerInterceptor>(),

                //Localization manager
                Component.For<ILocalizationManager>().ImplementedBy<NullLocalizationManager>().LifestyleSingleton()

                );
        }
    }
}
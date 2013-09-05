﻿using System;
using System.Collections.Generic;
using Abp.Web.Utils.Extensions;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Abp.Web.Controllers.Dynamic
{
    /// <summary>
    /// Used to generate dynamic api controllers for arbitrary types.
    /// TODO: Is is true to make static? Use in DI for extensibility?
    /// </summary>
    public static class DynamicControllerGenerator
    {
        /// <summary>
        /// Reference to current Ioc container.
        /// </summary>
        internal static WindsorContainer IocContainer { get; set; }

        /// <summary>
        /// Generates a new dynamic api controller for given type.
        /// </summary>
        /// <typeparam name="T">Type of the object to create</typeparam>
        public static void GenerateFor<T>(string controllerName = null)
        {
            IocContainer.Register(
                
                Component.For<AbpDynamicApiControllerInterceptor<T>>().LifestyleTransient(),
                
                Component.For<AbpDynamicApiController<T>>().Proxy.AdditionalInterfaces(new[] {typeof (T)}).Interceptors<AbpDynamicApiControllerInterceptor<T>>().LifestyleTransient()
                
                );

            DynamicControllerManager.RegisterServiceController(
                new DynamicControllerInfo
                    {
                        Name = controllerName ?? GetControllerName<T>(),
                        Type = typeof (AbpDynamicApiController<T>)
                    });
        }

        /// <summary>
        /// Gets conventional controller name given type.
        /// </summary>
        /// <typeparam name="T">Type to get controller name</typeparam>
        /// <returns>Controller name</returns>
        private static string GetControllerName<T>()
        {
            var name = typeof(T).Name.ToFirstCharacterLower();
            if(name.EndsWith("Service") && name.Length > 7)
            {
                name = name.Substring(0, name.Length - 7);
            }

            return name;
        }
    }
}

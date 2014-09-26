﻿using System.Reflection;
using System.Web;
using Abp.Dependency;
using Abp.Localization.Sources.Xml;
using Abp.Modules;
using Abp.Startup;
using Abp.Startup.Configuration;

namespace Abp.Web.Startup
{
    /// <summary>
    /// This module is used to use ABP in ASP.NET web applications.
    /// </summary>
    public class AbpWebModule : AbpModule
    {
        public override void PreInitialize(IAbpInitializationContext context)
        {
            base.PreInitialize(context);

            if (HttpContext.Current != null)
            {
                XmlLocalizationSource.RootDirectoryOfApplication = HttpContext.Current.Server.MapPath("~");                
            }
        }

        public override void Initialize(IAbpInitializationContext context)
        { 
            base.Initialize(context);

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            context.Configuration.Localization.RegisterXmlSource("AbpWeb", "Localization\\AbpWeb");
        }
    }
}

﻿using System;
using System.Linq;
using Abp.AspNetCore.EmbeddedResources;
using Abp.AspNetCore.Localization;
using Abp.Dependency;
using Abp.Localization;
using Castle.LoggingFacility.MsLogging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Abp.AspNetCore
{
	public static class AbpApplicationBuilderExtensions
    {
        public static void UseAbp(this IApplicationBuilder app)
        {
            app.UseAbp(null);
        }

	    public static void UseAbp([NotNull] this IApplicationBuilder app, Action<AbpApplicationBuilderOptions> optionsAction)
	    {
		    Check.NotNull(app, nameof(app));

	        var options = new AbpApplicationBuilderOptions();
            optionsAction?.Invoke(options);

            if (options.UseCastleLoggerFactory)
		    {
			    app.UseCastleLoggerFactory();
			}

			InitializeAbp(app);

		    if (options.UseAbpRequestLocalization)
		    {
                //TODO: This should be added later than authorization middleware!
			    app.UseAbpRequestLocalization();
		    }
	    }

		public static void UseEmbeddedFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new EmbeddedResourceFileProvider(
                        app.ApplicationServices.GetRequiredService<IIocResolver>()
                    )
                }
            );
        }

        private static void InitializeAbp(IApplicationBuilder app)
        {
            var abpBootstrapper = app.ApplicationServices.GetRequiredService<AbpBootstrapper>();
            abpBootstrapper.Initialize();
        }

        public static void UseCastleLoggerFactory(this IApplicationBuilder app)
        {
            var castleLoggerFactory = app.ApplicationServices.GetService<Castle.Core.Logging.ILoggerFactory>();
            if (castleLoggerFactory == null)
            {
                return;
            }

            app.ApplicationServices
                .GetRequiredService<ILoggerFactory>()
                .AddCastleLogger(castleLoggerFactory);
        }

        public static void UseAbpRequestLocalization(this IApplicationBuilder app, Action<RequestLocalizationOptions> optionsAction = null)
        {
            var iocResolver = app.ApplicationServices.GetRequiredService<IIocResolver>();
            using (var languageManager = iocResolver.ResolveAsDisposable<ILanguageManager>())
            {
                var supportedCultures = languageManager.Object
                    .GetLanguages()
                    .Select(l => CultureInfoHelper.Get(l.Name))
                    .ToArray();

                var options = new RequestLocalizationOptions
                {
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                };

                var userProvider = new UserRequestCultureProvider();

                options.RequestCultureProviders.Insert(0, new AbpLocalizationHeaderRequestCultureProvider());
                options.RequestCultureProviders.Insert(2, userProvider);
                options.RequestCultureProviders.Insert(4, new DefaultRequestCultureProvider());

                optionsAction?.Invoke(options);

                //TODO: Find cookie provider, set to UserRequestCultureProvider to set user's setting!!!
                userProvider.CookieProvider = options.RequestCultureProviders.FirstOrDefault(p => p is CookieRequestCultureProvider);

                app.UseRequestLocalization(options);
            }
        }
    }
}

﻿using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using Abp.Dependency;
using Abp.Localization;
using Abp.Runtime.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Abp.Web.Localization
{
    internal class LocalizationScriptManager : ILocalizationScriptManager, ISingletonDependency
    {
        private readonly ILocalizationManager _localizationManager;

        private readonly ThreadSafeObjectCache<string> _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="Abp.Web.Localization.LocalizationScriptManager"/> class.
        /// </summary>
        /// <param name="localizationManager">Localization manager.</param>
        public LocalizationScriptManager(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
            _cache = new ThreadSafeObjectCache<string>(new MemoryCache("__LocalizationScriptManager"), TimeSpan.FromDays(1));
        }

        /// <inheritdoc/>
        public string GetScript()
        {
            return GetScript(Thread.CurrentThread.CurrentUICulture);
        }

        /// <inheritdoc/>
        public string GetScript(CultureInfo cultureInfo)
        {
            return _cache.Get(cultureInfo.Name, () => BuildAll(cultureInfo));
        }

        private string BuildAll(CultureInfo cultureInfo)
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();
            script.AppendLine("    abp.localization = abp.localization || {};");
            script.AppendLine();
            script.AppendLine("    abp.localization.currentCulture = {");
            script.AppendLine("        name: '" + cultureInfo.Name + "',");
            script.AppendLine("        displayName: '" + cultureInfo.DisplayName + "'");
            script.AppendLine("    };");
            script.AppendLine();
            script.Append("    abp.localization.languages = [");

            var languages = _localizationManager.GetAllLanguages();
            for (var i = 0; i < languages.Count; i++)
            {
                var language = languages[i];

                script.AppendLine("{");
                script.AppendLine("        name: '" + language.Name + "',");
                script.AppendLine("        displayName: '" + language.DisplayName + "',");
                script.AppendLine("        icon: '" + language.Icon + "',");
                script.AppendLine("        isDefault: " + language.IsDefault.ToString().ToLower());
                script.Append("    }");

                if (i < languages.Count - 1)
                {
                    script.Append(" , ");
                }
            }

            script.AppendLine("];");
            script.AppendLine();

            if (languages.Count > 0)
            {
                var currentLanguage = _localizationManager.CurrentLanguage;
                script.AppendLine("    abp.localization.currentLanguage = {");
                script.AppendLine("        name: '" + currentLanguage.Name + "',");
                script.AppendLine("        displayName: '" + currentLanguage.DisplayName + "',");
                script.AppendLine("        icon: '" + currentLanguage.Icon + "',");
                script.AppendLine("        isDefault: " + currentLanguage.IsDefault.ToString().ToLower());
                script.AppendLine("    };");
            }

            script.AppendLine();
            script.AppendLine("    abp.localization.values = abp.localization.values || {};");
            script.AppendLine();

            foreach (var source in _localizationManager.GetAllSources().OrderBy(s => s.Name))
            {
                script.Append("    abp.localization.values['" + source.Name + "'] = ");

                var stringValues = source.GetAllStrings().OrderBy(s => s.Name).ToList();
                var stringJson = JsonConvert.SerializeObject(stringValues.ToDictionary(_ => _.Name, _ => _.Value), MakeJsonSerializerSettings());
                script.Append(stringJson);

                script.AppendLine(";");
                script.AppendLine();
            }

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }

        private JsonSerializerSettings MakeJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            return settings;
        }
    }
}

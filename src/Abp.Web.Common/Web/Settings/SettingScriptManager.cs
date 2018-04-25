﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace Abp.Web.Settings
{
    /// <summary>
    /// This class is used to build setting script.
    /// </summary>
    public class SettingScriptManager : ISettingScriptManager, ISingletonDependency
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _abpSession;
        private readonly IIocResolver _iocResolver;

        public SettingScriptManager(
            ISettingDefinitionManager settingDefinitionManager, 
            ISettingManager settingManager, 
            IAbpSession abpSession, 
            IIocResolver iocResolver)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
            _abpSession = abpSession;
            _iocResolver = iocResolver;
        }

        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.setting = abp.setting || {};");
            script.AppendLine("    abp.setting.values = {");

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions()
                .Where(sd => sd.ClientVisibility.IsVisible);

            var added = 0;
            foreach (var settingDefinition in settingDefinitions)
            {
                if (settingDefinition.ClientVisibility.RequiresAuthentication && !_abpSession.UserId.HasValue)
                {
                    continue;
                }

                using (var scope = _iocResolver.CreateScope())
                {
                    var permissionDependencyContext = scope.Resolve<PermissionDependencyContext>();
                    permissionDependencyContext.User = _abpSession.ToUserIdentifier();

                    if (settingDefinition.ClientVisibility.PermissionDependency != null &&
                        (!_abpSession.UserId.HasValue || !(await settingDefinition.ClientVisibility.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext))))
                    {
                        continue;
                    }
                }

                if (added > 0)
                {
                    script.AppendLine(",");
                }
                else
                {
                    script.AppendLine();
                }

                var settingValue = await _settingManager.GetSettingValueAsync(settingDefinition.Name);

                script.Append("        '" +
                              settingDefinition.Name .Replace("'", @"\'") + "': " +
                              (settingValue == null ? "null" : "'" + settingValue.Replace(@"\", @"\\").Replace("'", @"\'") + "'"));

                ++added;
            }

            script.AppendLine();
            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}
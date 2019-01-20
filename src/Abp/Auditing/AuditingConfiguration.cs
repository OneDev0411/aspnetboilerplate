﻿using System;
using System.Collections.Generic;

namespace Abp.Auditing
{
    internal class AuditingConfiguration : IAuditingConfiguration
    {
        public bool IsEnabled { get; set; }

        public bool RunInBackground { get; set; }

        public bool IsEnabledForAnonymousUsers { get; set; }

        public IAuditingSelectorList Selectors { get; }

        public List<Type> IgnoredTypes { get; }
        public bool IsAuditReturnValues { get; set; }

        public AuditingConfiguration()
        {
            IsEnabled = true;
            Selectors = new AuditingSelectorList();
            IgnoredTypes = new List<Type>();
            RunInBackground = false;
            IsAuditReturnValues = false;
        }
    }
}
﻿using Abp.Domain.Entities;

namespace Abp.Security.Roles
{
    public abstract class PermissionSettingEntityBase : CreationAuditedEntity
    {
        /// <summary>
        /// Unique name of the permission.
        /// </summary>
        public virtual string PermissionName { get; set; }

        /// <summary>
        /// Is this role granted for this permission.
        /// Default value: true.
        /// </summary>
        public virtual bool IsGranted { get; set; }

        /// <summary>
        /// Creates a new <see cref="RolePermission"/> instance.
        /// </summary>
        public PermissionSettingEntityBase()
        {
            IsGranted = true;
        }
    }
}
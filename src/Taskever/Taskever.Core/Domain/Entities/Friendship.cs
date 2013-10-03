﻿using Abp.Domain.Entities;
using Abp.Modules.Core.Domain.Entities;
using Abp.Modules.Core.Domain.Entities.Utils;

namespace Taskever.Domain.Entities
{
    public class Friendship : Entity, IHasTenant
    {
        /// <summary>
        /// The tenant account which this entity is belong to.
        /// </summary>
        public virtual Tenant Tenant { get; set; }

        public virtual User User { get; set; }
        
        public virtual User Friend { get; set; }

        /// <summary>
        /// Can <see cref="Friend"/> assign tasks to the <see cref="User"/>
        /// </summary>
        public virtual bool CanAssignTask { get; set; }

        public virtual FriendshipStatus Status { get; set; }
    }
}

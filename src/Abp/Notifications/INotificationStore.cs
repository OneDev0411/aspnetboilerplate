﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to store (persist) notifications.
    /// </summary>
    public interface INotificationStore
    {
        /// <summary>
        /// Inserts a notification subscription.
        /// </summary>
        Task InsertSubscriptionAsync(NotificationSubscriptionInfo subscription);

        /// <summary>
        /// Deletes a notification subscription.
        /// </summary>
        Task DeleteSubscriptionAsync(long userId, string notificationName, string entityTypeName, string entityId);

        /// <summary>
        /// Inserts a notification.
        /// </summary>
        Task InsertNotificationAsync(NotificationInfo notification);

        /// <summary>
        /// Gets a notification by Id, or returns null if not found.
        /// </summary>
        Task<NotificationInfo> GetNotificationOrNullAsync(Guid notificationId);

        /// <summary>
        /// Inserts a user notification.
        /// </summary>
        Task InsertUserNotificationAsync(UserNotificationInfo userNotification);

        /// <summary>
        /// Gets subscriptions for a notification.
        /// </summary>
        Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(string notificationName, string entityTypeName, string entityId);

        /// <summary>
        /// Gets subscriptions for a user.
        /// </summary>
        Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(long userId);

        /// <summary>
        /// Checks if a user subscribed for a notification
        /// </summary>
        Task<bool> IsSubscribedAsync(long userId, string notificationName, string entityTypeName, string entityId);

        /// <summary>
        /// Updates a user notification state.
        /// </summary>
        Task UpdateUserNotificationStateAsync(Guid userNotificationId, UserNotificationState state);

        /// <summary>
        /// Updates all notification states for a user.
        /// </summary>
        Task UpdateAllUserNotificationStatesAsync(long userId, UserNotificationState state);

        /// <summary>
        /// Deletes a user notification.
        /// </summary>
        Task DeleteUserNotificationAsync(Guid userNotificationId);

        /// <summary>
        /// Deletes all notifications of a user.
        /// </summary>
        Task DeleteAllUserNotificationsAsync(long userId);
    }
}
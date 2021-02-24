using SiteMapGeneratorTool.Models;
using System;
using System.Collections.Generic;
using WebPush;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Notification helper
    /// </summary>
    public class NotificationHelper
    {
        // Variables
        private readonly FirebaseHelper FirebaseHelper;
        private readonly VapidDetails VapidDetails;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="keyPath">Firebase keypath</param>
        /// <param name="database">Firebase database</param>
        /// <param name="requestCollection">Firebase request collection</param>
        /// <param name="subject">VAPID subject</param>
        /// <param name="publicKey">VAPID public key</param>
        /// <param name="privateKey">VAPID private key</param>
        public NotificationHelper(string keyPath, string database, string requestCollection, string subject, string publicKey, string privateKey)
        {
            FirebaseHelper = new FirebaseHelper(keyPath, database, requestCollection);
            VapidDetails = new VapidDetails(subject, publicKey, privateKey);
        }

        /// <summary>
        /// Send a notification
        /// </summary>
        /// <param name="id">Guid of request</param>
        /// <param name="message">Message elements</param>
        public void SendNotification(string id, List<string> message)
        {
            SubscriptionModel subscription = FirebaseHelper.Get<SubscriptionModel>(id);
            WebPushClient webPushClient = new WebPushClient();
            webPushClient.SendNotification(subscription.PushSubscription, string.Join(',', message), VapidDetails);
        }
    }
}

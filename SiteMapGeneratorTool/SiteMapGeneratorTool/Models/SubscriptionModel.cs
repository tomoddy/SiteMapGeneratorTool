using Google.Cloud.Firestore;
using WebPush;

namespace SiteMapGeneratorTool.Models
{
    [FirestoreData]
    public class SubscriptionModel
    {
        public SubscriptionModel() { }

        public SubscriptionModel(string endPoint, string p256dh, string auth)
        {
            EndPoint = endPoint;
            P256DH = p256dh;
            Auth = auth;
        }

        [FirestoreProperty]
        public string EndPoint { get; set; }
        [FirestoreProperty]
        public string P256DH { get; set; }
        [FirestoreProperty]
        public string Auth { get; set; }

        public PushSubscription PushSubscription
        {
            get
            {
                return new PushSubscription(EndPoint, P256DH, Auth);
            }
        }
    }
}

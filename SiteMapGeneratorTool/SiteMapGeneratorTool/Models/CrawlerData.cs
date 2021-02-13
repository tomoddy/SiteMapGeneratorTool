using Google.Cloud.Firestore;

namespace SiteMapGeneratorTool.Models
{
    [FirestoreData]
    public class CrawlerData
    {
        [FirestoreProperty]
        public string Guid { get; set; }
        [FirestoreProperty]
        public string Domain { get; set; }
        [FirestoreProperty]
        public string Completion { get; set; }
        [FirestoreProperty]
        public int Pages { get; set; }
        [FirestoreProperty]
        public double Elapsed { get; set; }
        public string Link { get; set; }
    }
}

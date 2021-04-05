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
        public long Completion { get; set; }
        [FirestoreProperty]
        public int Pages { get; set; }
        [FirestoreProperty]
        public int MaxPages { get; set; }
        [FirestoreProperty]
        public int Depth { get; set; }
        [FirestoreProperty]
        public double Elapsed { get; set; }
        [FirestoreProperty]
        public string Message { get; set; }
        public string Link { get; set; }
    }
}
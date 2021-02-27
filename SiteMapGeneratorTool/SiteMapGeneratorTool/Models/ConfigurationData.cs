using Google.Cloud.Firestore;

namespace SiteMapGeneratorTool.Models
{
    [FirestoreData]
    public class ConfigurationData
    {
        [FirestoreProperty]
        public bool Power { get; set; }
    }
}
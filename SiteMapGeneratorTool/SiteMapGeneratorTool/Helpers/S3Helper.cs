using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using System.Text;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Communicates with s3 bucket
    /// </summary>
    public class S3Helper
    {
        // Properties
        private TransferUtility Client { get; set; }
        public string BucketName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="accessKey">S3 access key</param>
        /// <param name="privateKey">S3 private key</param>
        /// <param name="bucketName">S3 bucket name</param>
        public S3Helper(string accessKey, string privateKey, string bucketName)
        {
            Client = new TransferUtility(new AmazonS3Client(new BasicAWSCredentials(accessKey, privateKey), RegionEndpoint.EUWest2));
            BucketName = bucketName;
        }

        /// <summary>
        /// Uploads file to S3 bucket
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <param name="name">Name of file</param>
        /// <param name="data">Data to put in file</param>
        public void UploadFile(string guid, string name, string data)
        {
            using MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            Client.Upload(memoryStream, BucketName, $"{guid}/{name}");
        }

        /// <summary>
        /// Downloadsa file from S3 to local storage
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <param name="name">Name of file</param>
        public FileInfo DownloadFile(string guid, string name)
        {
            string retVal = $"LocalS3Files/{guid}/{name}";
            Client.Download(retVal, BucketName, $"{guid}/{name}");
            return new FileInfo(retVal);
        }
    }
}

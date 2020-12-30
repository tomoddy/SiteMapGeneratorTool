using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Communicates with s3 bucket
    /// </summary>
    public class S3Helper
    {
        // Properties
        // TODO Fix access
        public AmazonS3Client Client { get; set; }
        private TransferUtility TransferUtility { get; set; }
        public string BucketName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="accessKey">S3 access key</param>
        /// <param name="privateKey">S3 private key</param>
        /// <param name="bucketName">S3 bucket name</param>
        public S3Helper(string accessKey, string privateKey, string bucketName)
        {
            Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, privateKey), RegionEndpoint.EUWest2);
            TransferUtility = new TransferUtility(Client);
            BucketName = bucketName;
        }

        public bool FileExists(string guid)
        {
            
            var _ = Client.GetObjectAsync(BucketName, guid);
            var _2 = Client.GetObjectAsync(BucketName, "6d91fcb5-a0d2-4013-a7ca-d8b9ddaac58d");
            return false;
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
            TransferUtility.Upload(memoryStream, BucketName, $"{guid}/{name}");
        }

        public Task<GetObjectResponse> DownloadResponse(string name)
        {
            return Client.GetObjectAsync(BucketName, name);
        }
    }
}

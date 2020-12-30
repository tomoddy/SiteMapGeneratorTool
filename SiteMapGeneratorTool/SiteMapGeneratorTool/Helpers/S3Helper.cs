using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
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
        private AmazonS3Client Client { get; set; }
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

        /// <summary>
        /// Checks if file exists in s3 bucket
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <param name="name">Name of file</param>
        /// <returns>True if exists, false if not</returns>
        public bool FileExists(string guid, string name)
        {
            return !(DownloadResponse(guid, new FileInfo(name)) is null);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private MemoryStream DownloadResponse(string guid, FileInfo fileInfo)
        {
            // Create file information and get response from s3
            Task<GetObjectResponse> response = Client.GetObjectAsync(BucketName, $"{guid}/{fileInfo.Name}");

            // Write response to memory
            MemoryStream retVal = new MemoryStream();
            try
            {
                using Stream responseStream = response.Result.ResponseStream;
                responseStream.CopyTo(retVal);
                retVal.Position = 0;
                return retVal;
            }
            catch (AggregateException)
            {
                return null;
            }
        }
    }
}

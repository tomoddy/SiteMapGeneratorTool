using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using System.IO;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public ResponseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet("")]
        public FileStreamResult Information(string guid)
        {
            S3Helper s3Client = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
            FileInfo fileInfo = s3Client.DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Information"));
            return DownloadFileContents(fileInfo);
        }

        [HttpGet("sitemap")]
        public FileStreamResult Sitemap(string guid)
        {
            S3Helper s3Client = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
            FileInfo fileInfo = s3Client.DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Sitemap"));
            return DownloadFileContents(fileInfo);
        }

        [HttpGet("graph")]
        public FileStreamResult Graph(string guid)
        {
            S3Helper s3Helper = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
            FileInfo fileInfo = s3Helper.DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Graph"));
            return DownloadFileContents(fileInfo);
        }

        private FileStreamResult DownloadFileContents(FileInfo fileInfo)
        {
            MemoryStream memory = new MemoryStream();
            using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
                fileStream.CopyTo(memory);
            memory.Position = 0;
            return File(memory, FileHelper.GetMimeTypes()[fileInfo.Extension], fileInfo.Name);
        }
    }
}

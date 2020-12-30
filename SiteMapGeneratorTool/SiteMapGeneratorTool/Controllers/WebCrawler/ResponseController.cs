﻿using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SiteMapGeneratorTool.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private const string INVALID_RESPONSE = "Invalid GUID";

        private readonly IConfiguration Configuration;
        private readonly S3Helper S3Helper;

        public ResponseController(IConfiguration configuration)
        {
            Configuration = configuration;
            S3Helper = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
        }

        [HttpGet("")]
        public bool Check(string guid)
        {
            return S3Helper.FileExists(guid, Configuration.GetValue<string>("S3:Files:Information"));
        }

        [HttpGet("information")]
        public ActionResult Information(string guid)
        {
            return DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Information"));
        }

        [HttpGet("sitemap")]
        public ActionResult Sitemap(string guid)
        {
            return DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Sitemap"));
        }

        [HttpGet("graph")]
        public ActionResult Graph(string guid)
        {
            return DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Graph"));
        }

        private ActionResult DownloadFile(string guid, string name)
        {
            FileInfo fileInfo = new FileInfo(name);
            MemoryStream memoryStream = S3Helper.DownloadResponse(guid, fileInfo);
            if (memoryStream is null)
                return new JsonResult(INVALID_RESPONSE);
            else
                return File(memoryStream, FileHelper.GetMimeTypes()[fileInfo.Extension], fileInfo.Name);
        }
    }
}

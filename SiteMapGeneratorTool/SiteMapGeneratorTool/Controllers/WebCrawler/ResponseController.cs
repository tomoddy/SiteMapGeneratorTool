using Microsoft.AspNetCore.Mvc;
using System;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        [HttpGet("")]
        public string Index(string guid)
        {
            return new Guid(guid).ToString();
        }
    }
}

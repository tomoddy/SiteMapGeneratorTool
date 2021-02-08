using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.WebCrawler.Objects
{
	public class Webpage
	{
		// Properties
        public Uri Url { get; set; }
		public DateTime? LastModified { get; set; }
		[JsonProperty]
		public List<Uri> Links { get; set; }

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="url">Url of webpage</param>
		/// <param name="lastModified">Last modified date</param>
		public Webpage(Uri url, DateTime? lastModified)
		{
			Url = url;
			LastModified = lastModified;
			Links = new List<Uri>();
		}

		/// <summary>
		/// Constructor for testing
		/// </summary>
		/// <param name="url">Url of webpage</param>
		/// <param name="lastModified">Last modified date</param>
		/// <param name="links">List of links</param>
		public Webpage(string url, DateTime? lastModified, List<string> links)
        {
			Url = new Uri(url);
			LastModified = lastModified;
			Links = new List<Uri>();

			foreach (string link in links)
				Links.Add(new Uri(link));
        }

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>Json string of object</returns>
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}

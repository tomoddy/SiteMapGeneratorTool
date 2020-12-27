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
		public Webpage(Uri url)
		{
			Url = url;
			Links = new List<Uri>();
		}

		/// <summary>
		/// Adds list of links to Links
		/// </summary>
		/// <param name="newLinks">List of links</param>
		public void AddLinks(List<Uri> newLinks)
		{
			Links.AddRange(newLinks);
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

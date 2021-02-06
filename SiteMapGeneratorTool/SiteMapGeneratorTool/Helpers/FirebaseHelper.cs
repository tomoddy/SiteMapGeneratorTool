using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static SiteMapGeneratorTool.Models.HistoryModel;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Firebase helper
    /// </summary>
    public class FirebaseHelper
    {
        // Variables
        private readonly IFirebaseClient Client;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="basePath">Firebase base path</param>
        /// <param name="authSecret">Firebase authsecret key</param>
        public FirebaseHelper(string basePath, string authSecret)
        {
            Client = new FirebaseClient(new FirebaseConfig
            {
                BasePath = basePath,
                AuthSecret = authSecret
            });
        }

        /// <summary>
        /// Adds user to database as valid
        /// </summary>
        /// <param name="guid">GUID of user</param>
        public void Add(string guid, Crawler information)
        {
            if (Client.Set($"requests/{guid}", information).StatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not create Firebase entry for {guid}");
        }

        /// <summary>
        /// Get crawler information from database
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Crawler object</returns>
        public Crawler Get(string guid)
        {
            JObject information = Client.Get($"requests/{guid}").ResultAs<JObject>();
            if (information == null)
                return null;
            else
                return JsonConvert.DeserializeObject<Crawler>(information.ToString());
        }

        /// <summary>
        /// Get all entries from database
        /// </summary>
        /// <returns>List of entries</returns>
        public List<ResultsModel> GetAll()
        {
            // Create return value and get all requests
            List<ResultsModel> retVal = new List<ResultsModel>();
            JObject requests = Client.Get("requests").ResultAs<JObject>();

            // Check if requests exist
            if (requests != null)
            {
                // Add all requests to return value
                foreach (string guid in requests.Properties().Select(p => p.Name).ToList())
                    retVal.Add(new ResultsModel(guid, JsonConvert.DeserializeObject<Crawler>(requests.GetValue(guid).ToString())));
            }

            // Return list
            return retVal.OrderByDescending(x => x.Information.Completion).ToList();
        }
    }
}

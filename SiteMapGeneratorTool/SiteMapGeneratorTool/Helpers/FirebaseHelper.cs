using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Firebase helper
    /// </summary>
    public class FirebaseHelper
    {
        // Constants
        private const string VALID = "valid";

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
        public void AddUser(string guid)
        {
            if (Client.Set($"users/{guid}", VALID).ResultAs<string>() != VALID)
                throw new Exception($"Could not create Firebase entry for {guid}");
        }

        /// <summary>
        /// Checks if user exists
        /// </summary>
        /// <param name="guid">GUID of user</param>
        /// <returns>True if exists, otherwise false</returns>
        public bool UserExists(string guid)
        {
            string result = Client.Get($"users/{guid}").ResultAs<string>();
            if (result is null)
                return false;
            else if (result == VALID)
                return true;
            else
                throw new Exception($"Firebase entry for {guid} does not contain valid information");
        }
    }
}

using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;

namespace SiteMapGeneratorTool.Helpers
{
    public class FirebaseHelper
    {
        private const string VALID = "valid";

        public IFirebaseClient Client { get; set; }

        public FirebaseHelper(string basePath, string authSecret)
        {
            Client = new FirebaseClient(new FirebaseConfig
            {
                BasePath = basePath,
                AuthSecret = authSecret
            });
        }

        public void AddUser(string guid)
        {
            if (Client.Set($"users/{guid}", VALID).ResultAs<string>() != VALID)
                throw new Exception($"Could not create Firebase entry for {guid}");
        }

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

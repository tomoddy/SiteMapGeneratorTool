using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Firebase helper
    /// </summary>
    public class FirebaseHelper
    {
        // Variables
        private readonly FirestoreDb Database;

        // Properties
        private string Collection { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path">Path to key file</param>
        /// <param name="database">Database name</param>
        /// <param name="collection">Collection name</param>
        public FirebaseHelper(string path, string database, string collection)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            Database = FirestoreDb.Create(database);
            Collection = collection;
        }

        /// <summary>
        /// Adds data to firestore
        /// </summary>
        /// <typeparam name="T">Type to add (must be firestoredata)</typeparam>
        /// <param name="id">Id of added data</param>
        /// <param name="data">Data to add</param>
        public void Add<T>(string id, T data)
        {
            Database.Collection(Collection).Document(id).SetAsync(data).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets data from firestore
        /// </summary>
        /// <typeparam name="T">Type of data to return (must be firestoredata)</typeparam>
        /// <param name="id">Id of data to get</param>
        /// <returns>Data is exists, filenotfoundexception otherwise</returns>
        public T Get<T>(string id)
        {
            DocumentSnapshot retVal = Database.Collection(Collection).Document(id).GetSnapshotAsync().GetAwaiter().GetResult();
            return retVal.Exists ? retVal.ConvertTo<T>() : default;
        }

        /// <summary>
        /// Gets all data from firestore
        /// </summary>
        /// <typeparam name="T">Type of data to return (must be firestoredata)</typeparam>
        /// <returns>List of data</returns>
        public List<T> GetAll<T>()
        {
            List<T> retVal = new List<T>();
            QuerySnapshot snapshot = Database.Collection(Collection).GetSnapshotAsync().GetAwaiter().GetResult();

            foreach (DocumentSnapshot document in snapshot.Documents)
                retVal.Add(document.ConvertTo<T>());

            return retVal;
        }
    }
}

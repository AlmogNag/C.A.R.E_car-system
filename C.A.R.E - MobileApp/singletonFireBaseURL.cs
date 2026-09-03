using Firebase.Database;

namespace CARE_App_Mobile
{
    /// <summary>
    /// Thread-safe Singleton service providing a single global Firebase client instance.
    /// </summary>
    public static class FirebaseService
    {
        private const string FirebaseUrl = "https://care-c0bdb-default-rtdb.europe-west1.firebasedatabase.app/";
        private static FirebaseClient _client;

        public static FirebaseClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new FirebaseClient(FirebaseUrl);
                }
                return _client;
            }
        }
    }
}
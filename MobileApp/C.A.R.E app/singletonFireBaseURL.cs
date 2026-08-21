using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Firebase.Database;

namespace C.A.R.E_app
{
    public static class FirebaseService
    {
        private static readonly string FirebaseUrl = "https://care-c0bdb-default-rtdb.europe-west1.firebasedatabase.app/";

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

        public static object LoggedInUser { get; internal set; }
    }
}

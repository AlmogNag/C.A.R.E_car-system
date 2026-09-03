using System;
using CARE_App_Mobile.Views;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile
{
    public partial class App : Application
    {
        private IDisposable _alertSubscription;
        public static string ActiveCameraId = "CARE_CAMERA_01";
        public static bool IsAlertPageOpen = false;

        public App()
        {
            InitializeComponent();

            // Check if a real logged-in user exists
            if (Views.LoginPage.LoggedInUser != null)
            {
                MainPage = new AppShell();
            }
            else
            {
                // If no one is logged in – the login screen always opens!
                MainPage = new NavigationPage(new Views.LoginPage());
            }
        }

        private void StartBackgroundEmergencyListener()
        {
            try
            {
                _alertSubscription?.Dispose();

                _alertSubscription = FirebaseService.Client
                    .Child("AlertSystem")
                    .Child(ActiveCameraId)
                    .AsObservable<dynamic>()
                    .Subscribe(async ev =>
                    {
                        if (ev != null && ev.Object != null)
                        {
                            string key = ev.Key;
                            string valStr = ev.Object.ToString();

                            if (string.Equals(key, "isAlert", StringComparison.OrdinalIgnoreCase) &&
                                (valStr.Equals("True", StringComparison.OrdinalIgnoreCase) || valStr.Equals("1")))
                            {
                                if (!IsAlertPageOpen)
                                {
                                    IsAlertPageOpen = true;

                                    var fullAlert = await FirebaseService.Client
                                        .Child("AlertSystem")
                                        .Child(ActiveCameraId)
                                        .OnceSingleAsync<AlertPayload>();

                                    MainThread.BeginInvokeOnMainThread(async () =>
                                    {
                                        await MainPage.Navigation.PushModalAsync(new EmergencyPopupPage(fullAlert ?? new AlertPayload { isAlert = true }, ActiveCameraId));
                                        IsAlertPageOpen = false;
                                    });
                                }
                            }
                        }
                    });
            }
            catch { }
        }
    }
}
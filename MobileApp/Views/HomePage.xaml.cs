using System;
using System.Linq;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace CARE_App_Mobile.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Requesting notification permission on Android
            try
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Permission Error] {ex.Message}");
            }

            // Loading counters and listening for alerts
            LoadDashboardMetrics();
            StartListeningForSafetyAlerts();
        }

        private async void LoadDashboardMetrics()
        {
            try
            {
                string camId = !string.IsNullOrEmpty(App.ActiveCameraId) ? App.ActiveCameraId : "CARE_CAMERA_01";

                // 1. Retrieve the number of users associated with this camera
                var users = await FirebaseService.Client
                    .Child("Users")
                    .OnceAsync<Participant>();

                int userCount = 0;
                if (users != null)
                {
                    foreach (var u in users)
                    {
                        if (u.Object != null && string.Equals(u.Object.CameraId, camId, StringComparison.OrdinalIgnoreCase))
                        {
                            userCount++;
                        }
                    }
                }
                lblUsersCount.Text = $"{userCount} users";

                // 2. Retrieving the cohesion quantity from history
                var captures = await FirebaseService.Client
                    .Child("AlertsHistory")
                    .Child(camId)
                    .OnceAsync<AlertPayload>();

                int captureCount = captures != null ? captures.Count : 0;
                lblCapturesCount.Text = $"{captureCount} captures";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HomePage] Dashboard Error: {ex.Message}");
            }
        }

        private async void OnViewGuardiansClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Guardians");
        }

        private async void OnViewCapturesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Activity");
        }

        private void StartListeningForSafetyAlerts()
        {
            string camId = !string.IsNullOrEmpty(App.ActiveCameraId) ? App.ActiveCameraId : "CARE_CAMERA_01";

            try
            {
                FirebaseService.Client
                    .Child("AlertSystem")
                    .Child(camId)
                    .AsObservable<AlertPayload>()
                    .Subscribe(d =>
                    {
                        if (d?.Object != null && d.Object.isAlert)
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
#if ANDROID
                                // Triggering a pop-up alert banner on the device
                                Platforms.Android.AndroidNotificationManager.ShowEmergencyNotification(
                                    "🚨 C.A.R.E EMERGENCY ALERT",
                                    $"Occupancy detected in cabin [{d.Object.alertSource}]! Check vehicle now."
                                );
#endif
                                // Opening the red pop-up window with the model and camera ID.
                                if (Navigation.ModalStack.Count == 0 || !(Navigation.ModalStack.Last() is EmergencyPopupPage))
                                {
                                    await Navigation.PushModalAsync(new EmergencyPopupPage(d.Object, camId));
                                }
                            });
                        }
                    });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Firebase Listener Error] {ex.Message}");
            }
        }
    }
}
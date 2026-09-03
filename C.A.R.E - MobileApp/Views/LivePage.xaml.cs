using System;
using System.IO;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile.Views
{
    public partial class LivePage : ContentPage
    {
        private IDisposable _liveSubscription;

        public LivePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartLiveFeedSubscription();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _liveSubscription?.Dispose();
        }

        private void StartLiveFeedSubscription()
        {
            try
            {
                _liveSubscription?.Dispose();

                _liveSubscription = FirebaseService.Client
                    .Child("AlertSystem")
                    .Child(App.ActiveCameraId)
                    .AsObservable<AlertPayload>()
                    .Subscribe(ev =>
                    {
                        if (ev.Object != null)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                lblLastFrameTime.Text = $"Last update: {ev.Object.timestamp ?? DateTime.Now.ToString("HH:mm:ss")}";
                                lblStreamStatus.Text = ev.Object.isAlert ? "⚠️ PRESENCE DETECTED" : "● Cabin Clear";
                                lblStreamStatus.TextColor = ev.Object.isAlert ? Color.FromArgb("#EF4444") : Color.FromArgb("#10B981");

                                if (!string.IsNullOrEmpty(ev.Object.imageUrl))
                                {
                                    try
                                    {
                                        byte[] bytes = Convert.FromBase64String(ev.Object.imageUrl);
                                        imgLiveCamera.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                                    }
                                    catch { }
                                }
                            });
                        }
                    });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LivePage] Error: {ex.Message}");
            }
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            var alert = await FirebaseService.Client
                .Child("AlertSystem")
                .Child(App.ActiveCameraId)
                .OnceSingleAsync<AlertPayload>();

            if (alert != null && !string.IsNullOrEmpty(alert.imageUrl))
            {
                byte[] bytes = Convert.FromBase64String(alert.imageUrl);
                imgLiveCamera.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile.Views
{
    public partial class ActivityPage : ContentPage
    {
        public ObservableCollection<AlertRecord> HistoryList { get; set; } = new ObservableCollection<AlertRecord>();

        public ActivityPage()
        {
            InitializeComponent();
            cvAlerts.ItemsSource = HistoryList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadAlertsHistory();
        }

        private async void LoadAlertsHistory()
        {
            try
            {
                var historyData = await FirebaseService.Client
                    .Child("AlertsHistory")
                    .Child(App.ActiveCameraId)
                    .OnceAsync<AlertRecord>();

                HistoryList.Clear();
                foreach (var item in historyData.Reverse())
                {
                    if (item.Object != null)
                    {
                        HistoryList.Add(item.Object);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ActivityPage] Error: {ex.Message}");
            }
        }

        private void OnAlertSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is AlertRecord selected)
            {
                lblSelectedDetails.Text = $"Source: {selected.AlertSource} • Time: {selected.Timestamp}";

                if (!string.IsNullOrEmpty(selected.ImageUrl))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(selected.ImageUrl);
                        imgPreview.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    }
                    catch { }
                }
            }
        }
    }
}
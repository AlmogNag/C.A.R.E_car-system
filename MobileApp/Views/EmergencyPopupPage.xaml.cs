using System;
using System.IO;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile.Views
{
    public partial class EmergencyPopupPage : ContentPage
    {
        private string _activeCameraId = "CARE_CAMERA_01";

        public EmergencyPopupPage(AlertPayload payload, string cameraId)
        {
            InitializeComponent();
            _activeCameraId = cameraId;
            DisplayAlertData(payload);
        }

        private void DisplayAlertData(AlertPayload alert)
        {
            lblAlertDetails.Text = $"Source: {alert?.alertSource ?? "CABIN SENSORS"} • Time: {alert?.timestamp ?? DateTime.Now.ToString("HH:mm:ss")}";

            if (!string.IsNullOrEmpty(alert?.imageUrl))
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(alert.imageUrl);
                    imgAlertSnapshot.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
                catch { }
            }
        }

        private async void OnAcknowledgeClicked(object sender, EventArgs e)
        {
            btnAcknowledge.IsEnabled = false;
            try
            {
                // Reset isAlert to false in Firebase
                await FirebaseService.Client
                    .Child("AlertSystem")
                    .Child(_activeCameraId)
                    .Child("isAlert")
                    .PutAsync(false);
            }
            catch { }
            finally
            {
                btnAcknowledge.IsEnabled = true;
                // Close modal emergency page
                await Navigation.PopModalAsync();
            }
        }
    }
}
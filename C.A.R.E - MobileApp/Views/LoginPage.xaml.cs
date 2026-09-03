using System;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile.Views
{
    public partial class LoginPage : ContentPage
    {
        public static Participant LoggedInUser { get; set; }

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            string phone = txtPhone.Text?.Trim();
            string pass = txtPassword.Text?.Trim();

            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(pass))
            {
                lblError.Text = "Please enter phone and password.";
                return;
            }

            btnSignIn.IsEnabled = false;
            lblError.Text = "Verifying...";

            try
            {
                var userRecord = await FirebaseService.Client
                    .Child("Users")
                    .Child(phone)
                    .OnceSingleAsync<Participant>();

                if (userRecord != null && userRecord.Password == pass)
                {
                    if (string.IsNullOrEmpty(userRecord.Phone))
                    {
                        userRecord.Phone = phone;
                    }

                    LoggedInUser = userRecord;
                    App.ActiveCameraId = string.IsNullOrEmpty(LoggedInUser.CameraId) ? "CARE_CAMERA_01" : LoggedInUser.CameraId;

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    lblError.Text = "Invalid credentials.";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error: {ex.Message}";
            }
            finally
            {
                btnSignIn.IsEnabled = true;
            }
        }

        private async void OnGoToRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
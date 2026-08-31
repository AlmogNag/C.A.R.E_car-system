using System;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text?.Trim();
            string phone = txtRegPhone.Text?.Trim();
            string pass = txtRegPass.Text?.Trim();
            string cam = txtRegCam.Text?.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(cam))
            {
                lblRegStatus.Text = "Please fill in all fields.";
                return;
            }

            btnRegister.IsEnabled = false;
            lblRegStatus.TextColor = Color.FromArgb("#3B82F6");
            lblRegStatus.Text = "Creating account...";

            try
            {
                var existing = await FirebaseService.Client
                    .Child("Users")
                    .Child(phone)
                    .OnceSingleAsync<Participant>();

                if (existing != null)
                {
                    lblRegStatus.TextColor = Color.FromArgb("#EF4444");
                    lblRegStatus.Text = "Phone already registered.";
                    btnRegister.IsEnabled = true;
                    return;
                }

                var newGuardian = new Participant(fullName, phone, pass, cam);
                await FirebaseService.Client.Child("Users").Child(phone).PutAsync(newGuardian);

                LoginPage.LoggedInUser = newGuardian;
                App.ActiveCameraId = cam;

                await DisplayAlert("Success", "Account created successfully!", "OK");
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                lblRegStatus.TextColor = Color.FromArgb("#EF4444");
                lblRegStatus.Text = $"Failed: {ex.Message}";
                btnRegister.IsEnabled = true;
            }
        }
    }
}
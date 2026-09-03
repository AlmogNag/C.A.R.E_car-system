using System;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile.Views
{
    public partial class SettingsPage : ContentPage
    {
        private bool _isHebrew = false;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadUserData();
            ApplyLanguage(_isHebrew);
        }

        private async void LoadUserData()
        {
            var user = LoginPage.LoggedInUser;

            // If no user is logged in – redirect them back to the login screen.
            if (user == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            lblProfileName.Text = user.FullName;
            lblProfilePhone.Text = _isHebrew ? $"טלפון: {user.Phone}" : $"Phone: {user.Phone}";
        }

        private void OnEnglishClicked(object sender, EventArgs e)
        {
            _isHebrew = false;
            ApplyLanguage(false);
        }

        private void OnHebrewClicked(object sender, EventArgs e)
        {
            _isHebrew = true;
            ApplyLanguage(true);
        }

        private void ApplyLanguage(bool hebrew)
        {
            this.FlowDirection = hebrew ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            if (hebrew)
            {
                btnHe.BackgroundColor = Color.FromArgb("#2563EB");
                btnHe.TextColor = Colors.White;

                btnEn.BackgroundColor = Color.FromArgb("#161F3D");
                btnEn.TextColor = Color.FromArgb("#94A3B8");
            }
            else
            {
                btnEn.BackgroundColor = Color.FromArgb("#2563EB");
                btnEn.TextColor = Colors.White;

                btnHe.BackgroundColor = Color.FromArgb("#161F3D");
                btnHe.TextColor = Color.FromArgb("#94A3B8");
            }

            Title = hebrew ? "הגדרות" : "Settings";
            lblLoggedTitle.Text = hebrew ? "משתמש מחובר" : "LOGGED GUARDIAN";
            lblPrefTitle.Text = hebrew ? "העדפות" : "PREFERENCES";
            lblLangLabel.Text = hebrew ? "שפת ממשק" : "App Language / שפת ממשק";
            btnSignOut.Text = hebrew ? "התנתק" : "Sign Out";

            LoadUserData();
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            string title = _isHebrew ? "התנתקות" : "Sign Out";
            string msg = _isHebrew ? "האם את/ה בטוח/ה שברצונך להתנתק?" : "Are you sure you want to sign out?";
            string yes = _isHebrew ? "כן" : "Yes";
            string cancel = _isHebrew ? "ביטול" : "Cancel";

            bool confirm = await DisplayAlert(title, msg, yes, cancel);
            if (confirm)
            {
                LoginPage.LoggedInUser = null;
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
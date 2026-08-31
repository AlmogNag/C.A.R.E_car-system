using System;
using System.Collections.ObjectModel;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace CARE_App_Mobile.Views
{
    public partial class UsersPage : ContentPage
    {
        public ObservableCollection<Participant> GuardiansList { get; set; } = new ObservableCollection<Participant>();

        public UsersPage()
        {
            InitializeComponent();
            cvUsers.ItemsSource = GuardiansList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadGuardians();
        }

        private async void LoadGuardians()
        {
            try
            {
                var users = await FirebaseService.Client
                    .Child("Users")
                    .OnceAsync<Participant>();

                GuardiansList.Clear();
                foreach (var u in users)
                {
                    if (u.Object != null && string.Equals(u.Object.CameraId, App.ActiveCameraId, StringComparison.OrdinalIgnoreCase))
                    {
                        // If the internal Phone property is empty, extract the phone from Firebase key
                        if (string.IsNullOrEmpty(u.Object.Phone))
                        {
                            u.Object.Phone = u.Key;
                        }

                        GuardiansList.Add(u.Object);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UsersPage] Error: {ex.Message}");
            }
        }
    }
}
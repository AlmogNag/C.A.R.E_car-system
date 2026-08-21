using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Query;

namespace C.A.R.E_app
{
    public partial class RegisterForm : Form
    {
        public string RegisteredName { get; private set; } = string.Empty;
        public string RegisteredPhone { get; private set; } = string.Empty;
        public string RegisteredPassword { get; private set; } = string.Empty;
        public string RegisteredCamera { get; private set; } = string.Empty;

        // הכתובת האמיתית של ה-Firebase שלך עודכנה כאן
        //private readonly FirebaseClient client = new FirebaseClient("https://care-c0bdb-default-rtdb.europe-west1.firebasedatabase.app/");
        FirebaseClient client = FirebaseService.Client;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private async void btnSubmitRegister_Click(object sender, EventArgs e)
        {
            string name = txtNewName.Text.Trim();
            string phone = txtNewPhone.Text.Trim();
            string password = txtNewPassword.Text;
            string camId = txtNewCamera.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(camId))
            {
                MessageBox.Show("Please fill in all fields to register!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // יצירת האובייקט לפי המבנה שביקשת
                var newUser = new
                {
                    fullName = name,
                    password = password,
                    cameraId = camId
                };

                // שליחה לתיקיית Users תחת ה-Firebase שלך
                await client
                    .Child("Users")
                    .Child(phone)
                    .PutAsync(newUser);

                RegisteredName = name;
                RegisteredPhone = phone;
                RegisteredPassword = password;
                RegisteredCamera = camId;

                MessageBox.Show("Registration successfully completed and saved to Firebase!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Firebase Error: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
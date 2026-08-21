using Firebase.Database;
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
    public partial class LoginForm : Form
    {
        public static Participant? LoggedInUser { get; private set; }

        // הכתובת האמיתית של ה-Firebase שלך עודכנה גם כאן
        //private readonly FirebaseClient client = new FirebaseClient("https://care-c0bdb-default-rtdb.europe-west1.firebasedatabase.app/");
        FirebaseClient client = FirebaseService.Client;

        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string phoneInput = txtPhone.Text.Trim();
            string passwordInput = txtPassword.Text;

            if (string.IsNullOrEmpty(phoneInput) || string.IsNullOrEmpty(passwordInput))
            {
                MessageBox.Show("Please fill in all fields!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // שליפת המשתמש מתיקיית Users לפי הטלפון
                var firebaseUser = await client
                    .Child("Users")
                    .Child(phoneInput)
                    .OnceSingleAsync<Participant>();

                // בדיקה שהמשתמש קיים והסיסמה תואמת (בדיקה מול ה-password באות קטנה מהענן)
                if (firebaseUser != null && firebaseUser.password == passwordInput)
                {
                    LoggedInUser = firebaseUser;
                    LoggedInUser.Phone = phoneInput;

                    MessageBox.Show($"Login successful!\nWelcome back, {firebaseUser.fullName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Access Denied. Invalid phone number or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to Firebase: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();

            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                txtPhone.Text = registerForm.RegisteredPhone;
                txtPassword.Text = registerForm.RegisteredPassword;
            }
        }
    }
}

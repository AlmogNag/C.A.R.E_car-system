using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace C.A.R.E_app
{
    public partial class popUpView : Form
    {
        FirebaseClient client;

        // Defining the camera constant locally for this single-component phase
        private const string UserCamera = "CARE_CAMERA_01";

        public popUpView()
        {
            InitializeComponent();
            client = FirebaseService.Client;
        }

        // --- FIXED: Updated path to match the new nested camera database structure ---
        private async void btnReset_Click(object sender, EventArgs e)
        {
            picCamera.Image = null;
            try
            {
                // We must navigate into the camera node before resetting isAlert to false
                await client.Child("AlertSystem")
                            .Child(UserCamera)
                            .Child("isAlert")
                            .PutAsync(false);

                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating cloud: " + ex.Message);
            }
        }

        // --- UPDATED: Standard name mapping for popUpView form loading ---
        private void popUpView_Load(object sender, EventArgs e)
        {
            // Set up real-time observation on the specific camera node
            FirebaseService.Client
                .Child("AlertSystem")
                .Child(UserCamera)
                .AsObservable<dynamic>()
                .Subscribe(data => HandleAlertUpdate(data));
        }

        private void HandleAlertUpdate(dynamic data)
        {
            if (data != null && data.Object != null)
            {
                try
                {
                    string key = data.Key; // מזהה איזה שדה בדיוק השתנה בענן ("isAlert" או "imageUrl")

                    if (key == "isAlert")
                    {
                        bool isAlert = Convert.ToBoolean(data.Object);

                        if (isAlert == true)
                        {
                            // 1. קבלת התראה - הקפצת החלון לקדמה וצביעתו באדום
                            this.Invoke((MethodInvoker)delegate
                            {
                                this.WindowState = FormWindowState.Normal;
                                this.ShowInTaskbar = true;
                                this.TopMost = true;

                                this.BackColor = Color.DarkRed;
                                lblStatus.Text = "Danger! \nPresence detected!";
                                lblStatus.ForeColor = Color.White;
                                lblStatus.Left = (this.ClientSize.Width - lblStatus.Width) / 2;
                            });
                        }
                        else
                        {
                            // 2. איפוס התראה (המשתמש לחץ Reset) - החזרת המצב לקדמותו
                            this.Invoke((MethodInvoker)delegate
                            {
                                this.BackColor = SystemColors.Control;
                                lblStatus.Text = "System Secured \nNo presence detected.";
                                lblStatus.ForeColor = Color.Black;
                                this.TopMost = false;
                                picCamera.Image = null; // מנקה את התמונה הישנה
                            });
                        }
                    }
                    else if (key == "imageUrl")
                    {
                        // 3. הגיע עדכון לתמונה מהראסברי פאי - נפענח ונציג אותה
                        string base64Image = data.Object.ToString();

                        if (!string.IsNullOrEmpty(base64Image))
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                LoadBase64ToPictureBox(picCamera, base64Image);
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing Firebase dynamic payload: " + ex.Message);
                }
            }
        }

        private void LoadBase64ToPictureBox(PictureBox pBox, string base64String)
        {
            try
            {
                // Convert raw base64 data string back into a byte array
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Load bytes stream into memory to draw the image container
                using (var ms = new System.IO.MemoryStream(imageBytes))
                {
                    pBox.Image = Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to convert Base64 string to image: " + ex.Message);
            }
        }
    }
}
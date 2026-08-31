namespace CARE_App_Mobile
{
    public class Participant
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string CameraId { get; set; } = "CARE_CAMERA_01";

        public Participant() { }

        public Participant(string fullName, string phone, string password, string cameraId = "CARE_CAMERA_01")
        {
            FullName = fullName;
            Phone = phone;
            Password = password;
            CameraId = cameraId;
        }
    }
}
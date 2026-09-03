using System;

namespace CARE_App_Mobile
{
    /// <summary>
    /// Represents a historical alert record logged in AlertsHistory.
    /// </summary>
    public class AlertRecord
    {
        public string Timestamp { get; set; }
        public string AlertSource { get; set; }
        public bool MotionDetected { get; set; }
        public bool HumanDetected { get; set; }
        public string ImageUrl { get; set; } // Base64 or Image Link
        public bool IsAlert { get; set; }
    }

    /// <summary>
    /// Represents the real-time emergency alert status under AlertSystem/{CameraId}.
    /// </summary>
    public class AlertPayload
    {
        public bool isAlert { get; set; }
        public string imageUrl { get; set; }
        public string timestamp { get; set; }
        public string alertSource { get; set; }
    }
}
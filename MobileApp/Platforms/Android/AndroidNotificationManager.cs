using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using System;

namespace CARE_App_Mobile.Platforms.Android
{
    public static class AndroidNotificationManager
    {
        private const string CHANNEL_ID = "care_emergency_channel";
        private const string CHANNEL_NAME = "C.A.R.E Emergency Alerts";

        public static void ShowEmergencyNotification(string title, string message)
        {
            var context = global::Android.App.Application.Context;
            var manager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            // יצירת ערוץ בעדיפות מקסימלית עם צליל התראת חירום
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.High)
                {
                    Description = "High-priority emergency alerts for child occupancy",
                    LockscreenVisibility = NotificationVisibility.Public
                };
                channel.EnableVibration(true);
                channel.EnableLights(true);

                var soundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
                var audioAttributes = new AudioAttributes.Builder()
                    .SetContentType(AudioContentType.Sonification)
                    .SetUsage(AudioUsageKind.Alarm)
                    .Build();
                channel.SetSound(soundUri, audioAttributes);

                manager.CreateNotificationChannel(channel);
            }

            // Configuring click action - opening the app to the current screen
            var intent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
            if (intent != null)
            {
                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            }
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogAlert)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetPriority(NotificationCompat.PriorityMax)
                .SetCategory(NotificationCompat.CategoryAlarm)
                .SetVisibility(NotificationCompat.VisibilityPublic)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            manager.Notify(new Random().Next(1000, 9999), builder.Build());
        }
    }
}
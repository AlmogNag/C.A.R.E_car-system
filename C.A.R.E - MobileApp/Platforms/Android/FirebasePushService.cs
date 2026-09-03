using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using Firebase.Messaging;
using System;

namespace CARE_App_Mobile.Platforms.Android
{
    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebasePushService : FirebaseMessagingService
    {
        private const string CHANNEL_ID = "care_emergency_channel";
        private const string CHANNEL_NAME = "C.A.R.E Emergency Alerts";

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            string title = message.GetNotification()?.Title ?? "🚨 C.A.R.E EMERGENCY ALERT";
            string body = message.GetNotification()?.Body ?? "Occupancy detected in cabin! Check vehicle immediately.";

            ShowEmergencyNotification(title, body);
        }

        private void ShowEmergencyNotification(string title, string message)
        {
            var context = global::Android.App.Application.Context;
            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

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

                notificationManager?.CreateNotificationChannel(channel);
            }

            var intent = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName);
            if (intent != null)
            {
                intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            }
            var pendingIntent = PendingIntent.GetActivity(
                context,
                0,
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            );

            var builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogAlert)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetPriority(NotificationCompat.PriorityMax)
                .SetCategory(NotificationCompat.CategoryAlarm)
                .SetVisibility(NotificationCompat.VisibilityPublic)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            // Triggering the alert in the system
            notificationManager?.Notify(new Random().Next(1000, 9999), builder.Build());
        }
    }
}
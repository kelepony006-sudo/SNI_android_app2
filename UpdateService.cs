using Android.App;
using Android.Content;
using System;
using System.Net;

namespace SkullNetworkItalia.App.Services
{
    [Service]
    public class UpdateService : Service
    {
        private const string VERSION_URL = "https://tuoserver.com/app/version.txt";
        private const string CURRENT_VERSION = "1.0.0";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            CheckForUpdatesPeriodically();
            return StartCommandResult.Sticky;
        }

        private async void CheckForUpdatesPeriodically()
        {
            while (true)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        var latestVersion = await client.DownloadStringTaskAsync(VERSION_URL);

                        if (latestVersion.Trim() != CURRENT_VERSION)
                        {
                            ShowNotification();
                        }
                    }
                }
                catch (Exception)
                {
                    // Log error
                }

                // Controlla ogni 6 ore
                await System.Threading.Tasks.Task.Delay(6 * 60 * 60 * 1000);
            }
        }

        private void ShowNotification()
        {
            var intent = new Intent(this, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

            var notification = new Notification.Builder(this)
                .SetContentTitle("Skull Network Italia")
                .SetContentText("Aggiornamento disponibile!")
                .SetSmallIcon(Resource.Drawable.icon_community)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .Build();

            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(1, notification);
        }
    }
}
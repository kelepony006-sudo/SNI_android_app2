using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System;
using System.IO;
using System.Net;

namespace SkullNetworkItalia.App
{
    [Activity(
        Label = "Skull Network Italia",
        MainLauncher = true,
        Icon = "@drawable/icon_community",
        Theme = "@style/AppTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        private const string GITHUB_REPO = "https://github.com/SkullNetworkItalia/app";
        private const string APK_DOWNLOAD_URL = "https://tuoserver.com/app/update.apk";
        private const string VERSION_CHECK_URL = "https://tuoserver.com/app/version.txt";
        private const string APP_VERSION = "1.0.0";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Controlla aggiornamenti all'avvio
            CheckForUpdates();

            // Inizializza UI
            InitializeUI();
        }

        private void InitializeUI()
        {
            var btnCommunity = FindViewById<Button>(Resource.Id.btnCommunity);
            var btnForum = FindViewById<Button>(Resource.Id.btnForum);
            var btnNews = FindViewById<Button>(Resource.Id.btnNews);
            var imgLogo = FindViewById<ImageView>(Resource.Id.imgLogo);

            // Imposta l'icona della community
            imgLogo.SetImageResource(Resource.Drawable.icon_community);

            btnCommunity.Click += (sender, e) =>
            {
                OpenUrl("https://skullnetworkitalia.com");
            };

            btnForum.Click += (sender, e) =>
            {
                OpenUrl("https://forum.skullnetworkitalia.com");
            };

            btnNews.Click += (sender, e) =>
            {
                OpenUrl("https://news.skullnetworkitalia.com");
            };
        }

        private async void CheckForUpdates()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var latestVersion = await client.DownloadStringTaskAsync(VERSION_CHECK_URL);

                    if (latestVersion.Trim() != APP_VERSION)
                    {
                        ShowUpdateDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore nel controllo aggiornamenti: {ex.Message}");
            }
        }

        private void ShowUpdateDialog()
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Aggiornamento Disponibile");
            builder.SetMessage("È disponibile una nuova versione dell'app. Vuoi scaricarla ora?");
            builder.SetPositiveButton("Scarica", (sender, e) =>
            {
                DownloadAndInstallUpdate();
            });
            builder.SetNegativeButton("Più tardi", (sender, e) => { });
            builder.Show();
        }

        private async void DownloadAndInstallUpdate()
        {
            try
            {
                var progressDialog = new ProgressDialog(this);
                progressDialog.SetTitle("Download aggiornamento");
                progressDialog.SetMessage("Scaricamento in corso...");
                progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
                progressDialog.Show();

                var localPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "SkullNetworkUpdate.apk");

                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, e) =>
                    {
                        progressDialog.Progress = e.ProgressPercentage;
                    };

                    client.DownloadFileCompleted += (sender, e) =>
                    {
                        progressDialog.Dismiss();
                        InstallApk(localPath);
                    };

                    await client.DownloadFileTaskAsync(APK_DOWNLOAD_URL, localPath);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Errore download: {ex.Message}", ToastLength.Long).Show();
            }
        }

        private void InstallApk(string filePath)
        {
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(
                Android.Net.Uri.FromFile(new Java.IO.File(filePath)),
                "application/vnd.android.package-archive");
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }

        private void OpenUrl(string url)
        {
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
    }
}
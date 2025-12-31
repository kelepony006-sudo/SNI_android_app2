using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SkullNetworkItalia.Services
{
    public class UpdateInfo
    {
        public string Version { get; set; }
        public string DownloadUrl { get; set; }
        public string Changelog { get; set; }
        public bool Critical { get; set; }
    }

    public class AutoUpdateService
    {
        private const string UpdateUrl = "https://tuoserver.com/api/update/sni";
        private readonly HttpClient _httpClient;

        public AutoUpdateService()
        {
            _httpClient = new HttpClient();
        }

        public async Task CheckForUpdates()
        {
            try
            {
                var currentVersion = GetCurrentVersion();
                var response = await _httpClient.GetStringAsync(UpdateUrl);
                var updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(response);

                if (IsNewVersionAvailable(currentVersion, updateInfo.Version))
                {
                    await ShowUpdateDialog(updateInfo);
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Update check failed: {ex.Message}");
            }
        }

        private string GetCurrentVersion()
        {
            return AppInfo.Current.VersionString;
        }

        private bool IsNewVersionAvailable(string current, string latest)
        {
            var currentVersion = new Version(current);
            var latestVersion = new Version(latest);
            return latestVersion > currentVersion;
        }

        private async Task ShowUpdateDialog(UpdateInfo updateInfo)
        {
            bool update = await Application.Current.MainPage.DisplayAlert(
                "Aggiornamento Disponibile",
                $"È disponibile la versione {updateInfo.Version}\n\n{updateInfo.Changelog}",
                "Aggiorna",
                updateInfo.Critical ? null : "Più tardi");

            if (update)
            {
                await Launcher.OpenAsync(updateInfo.DownloadUrl);
            }
        }
    }
}
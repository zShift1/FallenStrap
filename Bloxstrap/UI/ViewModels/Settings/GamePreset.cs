using System.Windows.Media.Imaging;

namespace Bloxstrap.UI.ViewModels.Settings
{
    public class GamePreset : NotifyPropertyChangedViewModel
    {
        private BitmapImage? _icon;

        public string Name { get; init; } = "";

        public string Description { get; init; } = "";

        public long PlaceId { get; init; }

        public IReadOnlyDictionary<string, string> Flags { get; init; } = new Dictionary<string, string>();

        public BitmapImage? Icon
        {
            get => _icon;
            private set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public GamePreset()
        {
            LoadIconAsync();
        }

        private async void LoadIconAsync()
        {
            try
            {
                long universeId = await ResolveUniverseIdAsync(PlaceId);
                string? imageUrl = await FetchGameIconUrlAsync(universeId);

                if (!String.IsNullOrEmpty(imageUrl))
                {
                    byte[] bytes = await App.HttpClient.GetByteArrayAsync(imageUrl);

                    var stream = new MemoryStream(bytes);
                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    Icon = bitmap;
                }
            }
            catch (Exception ex)
            {
                App.Logger.WriteException("GamePreset::LoadIconAsync", ex);
            }

            // fallback icon so the card never looks broken
            if (Icon is null)
                Icon = new BitmapImage(new Uri("pack://application:,,,/FallenStrap.ico"));
        }

        private static async Task<long> ResolveUniverseIdAsync(long placeId)
        {
            string json = await App.HttpClient.GetStringAsync($"https://apis.roblox.com/universes/v1/places/{placeId}/universe");

            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("universeId", out var universeId))
                return universeId.GetInt64();

            throw new Exception($"Could not resolve universe for place {placeId}");
        }

        private static async Task<string?> FetchGameIconUrlAsync(long universeId)
        {
            string json = await App.HttpClient.GetStringAsync(
                $"https://thumbnails.roblox.com/v1/games/icons?universeIds={universeId}&returnPolicy=PlaceHolder&size=512x512&format=Png");

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("data", out var data) || data.GetArrayLength() == 0)
                return null;

            if (data[0].TryGetProperty("imageUrl", out var imageUrl) && imageUrl.ValueKind == JsonValueKind.String)
                return imageUrl.GetString();

            return null;
        }
    }
}
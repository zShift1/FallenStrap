using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Bloxstrap.Enums.FlagPresets;

namespace Bloxstrap.UI.ViewModels.Settings
{
    public class FastFlagsViewModel : NotifyPropertyChangedViewModel
    {
        private Dictionary<string, object>? _preResetFlags;

        public event EventHandler? RequestPageReloadEvent;
        
        public event EventHandler? OpenFlagEditorEvent;

        private void OpenFastFlagEditor() => OpenFlagEditorEvent?.Invoke(this, EventArgs.Empty);

        public ICommand OpenFastFlagEditorCommand => new RelayCommand(OpenFastFlagEditor);

        public Visibility CanShowFastFlagEditor => Visibility.Visible;

        public bool UseFastFlagManager
        {
            get => App.Settings.Prop.UseFastFlagManager;
            set => App.Settings.Prop.UseFastFlagManager = value;
        }

        public IReadOnlyDictionary<MSAAMode, string?> MSAALevels => FastFlagManager.MSAAModes;

        public MSAAMode SelectedMSAALevel
        {
            get => MSAALevels.FirstOrDefault(x => x.Value == App.FastFlags.GetPreset("Rendering.MSAA")).Key;
            set => App.FastFlags.SetPreset("Rendering.MSAA", MSAALevels[value]);
        }

        public bool FixDisplayScaling
        {
            get => App.FastFlags.GetPreset("Rendering.DisableScaling") == "True";
            set => App.FastFlags.SetPreset("Rendering.DisableScaling", value ? "True" : null);
        }

        public IReadOnlyDictionary<FastFlagManager.GraphicsQuality, string?> GraphicsQualities => FastFlagManager.GraphicsQualityLevels;

        public FastFlagManager.GraphicsQuality SelectedGraphicsQuality
        {
            get => GraphicsQualities.Where(x => x.Value == App.FastFlags.GetPreset("Rendering.FRMQuality")).FirstOrDefault().Key;
            set => App.FastFlags.SetPreset("Rendering.FRMQuality", GraphicsQualities[value]);
        }

        public IReadOnlyDictionary<FastFlagManager.GrassDistance, string?> GrassDistances => FastFlagManager.GrassDistances;

        public FastFlagManager.GrassDistance SelectedGrassDistance
        {
            get => GrassDistances.Where(x => x.Value == App.FastFlags.GetPreset("Rendering.GrassDistance")).FirstOrDefault().Key;
            set => App.FastFlags.SetPreset("Rendering.GrassDistance", GrassDistances[value]);
        }

        public bool DisableVoxelizer
        {
            get => App.FastFlags.GetPreset("Rendering.DisableVoxelizer") == "True";
            set => App.FastFlags.SetPreset("Rendering.DisableVoxelizer", value ? "True" : null);
        }

        public IReadOnlyDictionary<TextureQuality, string?> TextureQualities => FastFlagManager.TextureQualityLevels;

        public TextureQuality SelectedTextureQuality
        {
            get => TextureQualities.Where(x => x.Value == App.FastFlags.GetPreset("Rendering.TextureQuality.Level")).FirstOrDefault().Key;
            set
            {
                if (value == TextureQuality.Default)
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality", null);
                }
                else
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality.OverrideEnabled", "True");
                    App.FastFlags.SetPreset("Rendering.TextureQuality.Level", TextureQualities[value]);
                }
            }
        }
        public bool ResetConfiguration
        {
            get => _preResetFlags is not null;

            set
            {
                if (value)
                {
                    _preResetFlags = new(App.FastFlags.Prop);
                    App.FastFlags.Prop.Clear();
                }
                else
                {
                    App.FastFlags.Prop = _preResetFlags!;
                    _preResetFlags = null;
                }

                RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        // ---------------------------------------------------------------
        // Presets por juego: combinación de Fast Flags optimizada para
        // cada experiencia popular. Solo flags confirmadas en la allowlist
        // actual (rendering, geometry y networking/latencia).
        // ---------------------------------------------------------------

        public ObservableCollection<GamePreset> GamePresets { get; } = new()
        {
            new GamePreset
            {
                Name = "Blox Fruits",
                PlaceId = 2753915549,
                Description = "Máximo rendimiento para mundo abierto pesado: calidad baja, hierba fuera, LOD agresivo.",
                Flags = new Dictionary<string, string>
                {
                    { "DFIntDebugFRMQualityLevelOverride", "3" },
                    { "FIntFRMMaxGrassDistance", "0" },
                    { "FIntFRMMinGrassDistance", "0" },
                    { "DFFlagDebugPauseVoxelizer", "True" },
                    { "DFIntTaskSchedulerTargetFps", "240" },
                    { "DFIntCSGLevelOfDetailSwitchingDistance", "80" },
                    { "DFIntCSGLevelOfDetailSwitchingDistanceL12", "80" },
                    { "DFIntCSGLevelOfDetailSwitchingDistanceL23", "80" },
                    { "DFIntCSGLevelOfDetailSwitchingDistanceL34", "80" },
                    { "FFlagDebugGraphicsPreferD3D11", "True" },
                }
            },
            new GamePreset
            {
                Name = "Arsenal",
                PlaceId = 286090429,
                Description = "Baja latencia y FPS alto para shooter competitivo: red optimizada, MSAA mínimo, FPS desbloqueado.",
                Flags = new Dictionary<string, string>
                {
                    { "DFIntDebugFRMQualityLevelOverride", "8" },
                    { "DFIntTaskSchedulerTargetFps", "240" },
                    { "DFIntConnectionMTUSize", "1400" },
                    { "DFIntRakNetResendBufferArrayLength", "128" },
                    { "DFIntRakNetResendTimeoutMS", "200" },
                    { "DFIntNetworkPrediction", "1" },
                    { "DFIntCSGLevelOfDetailSwitchingDistance", "120" },
                    { "FIntDebugForceMSAASamples", "1" },
                }
            },
            new GamePreset
            {
                Name = "Brookhaven",
                PlaceId = 4924922222,
                Description = "Equilibrado con buena calidad visual: calidad media-alta, hierba moderada, FPS estable.",
                Flags = new Dictionary<string, string>
                {
                    { "DFIntDebugFRMQualityLevelOverride", "14" },
                    { "DFIntTaskSchedulerTargetFps", "144" },
                    { "FIntFRMMaxGrassDistance", "100" },
                    { "FIntFRMMinGrassDistance", "50" },
                    { "DFIntCSGLevelOfDetailSwitchingDistance", "250" },
                    { "FFlagDebugGraphicsPreferD3D11", "True" },
                }
            },
            new GamePreset
            {
                Name = "Adopt Me!",
                PlaceId = 920587237,
                Description = "Suave y estable para social: calidad media, hierba media, FPS tranquilo.",
                Flags = new Dictionary<string, string>
                {
                    { "DFIntDebugFRMQualityLevelOverride", "12" },
                    { "DFIntTaskSchedulerTargetFps", "120" },
                    { "FIntFRMMaxGrassDistance", "150" },
                    { "FIntFRMMinGrassDistance", "75" },
                    { "DFIntCSGLevelOfDetailSwitchingDistance", "300" },
                }
            },
            new GamePreset
            {
                Name = "Murder Mystery 2",
                PlaceId = 142823291,
                Description = "FPS alto con render limpio: calidad baja, voxelizer pausado, MSAA mínimo.",
                Flags = new Dictionary<string, string>
                {
                    { "DFIntDebugFRMQualityLevelOverride", "6" },
                    { "DFIntTaskSchedulerTargetFps", "240" },
                    { "DFFlagDebugPauseVoxelizer", "True" },
                    { "DFIntCSGLevelOfDetailSwitchingDistance", "100" },
                    { "FIntDebugForceMSAASamples", "1" },
                }
            },
            new GamePreset
            {
                Name = "Doors",
                PlaceId = 6516141723,
                Description = "Rendimiento + visual para horror: calidad media, hierba fuera, voxelizer pausado.",
                Flags = new Dictionary<string, string>
                {
                    { "DFIntDebugFRMQualityLevelOverride", "10" },
                    { "DFIntTaskSchedulerTargetFps", "165" },
                    { "FIntFRMMaxGrassDistance", "0" },
                    { "FIntFRMMinGrassDistance", "0" },
                    { "DFFlagDebugPauseVoxelizer", "True" },
                    { "FFlagDebugGraphicsPreferD3D11", "True" },
                    { "DFIntCSGLevelOfDetailSwitchingDistance", "200" },
                }
            },
        };

        public ICommand ApplyPresetCommand => new RelayCommand<GamePreset>(ApplyPreset);

        private void ApplyPreset(GamePreset? preset)
        {
            if (preset is null)
                return;

            foreach (var pair in preset.Flags)
                App.FastFlags.SetValue(pair.Key, pair.Value);

            App.FastFlags.Save();

            RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}

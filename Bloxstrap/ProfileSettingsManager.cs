using Bloxstrap.Models.Persistable;

namespace Bloxstrap
{
    /// <summary>
    /// <see cref="JsonManager{T}"/> for <see cref="Settings"/> that redirects its
    /// file location to the active profile folder when a non-default profile is set.
    /// </summary>
    public class ProfileSettingsManager : JsonManager<Settings>
    {
        public override string FileLocation => App.Profiles.Resolve(Path.Combine(Paths.Base, FileName), FileName);
    }
}
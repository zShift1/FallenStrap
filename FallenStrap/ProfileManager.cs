using FallenStrap.Models.Persistable;

namespace FallenStrap
{
    public class ProfileManager : JsonManager<ProfileConfig>
    {
        public override string FileName => "profiles.json";

        public override string FileLocation => Path.Combine(Paths.Base, FileName);

        public bool IsDefaultProfile => String.IsNullOrEmpty(Prop.Active) || Prop.Active == "default";

        public string? ActiveDirectory => IsDefaultProfile ? null : Path.Combine(Paths.Base, "Profiles", Prop.Active);

        public List<string> Profiles => Prop.Profiles;

        /// <summary>
        /// Returns the profile-aware replacement for a file path. When the
        /// default profile is active, the original path is returned untouched.
        /// </summary>
        public string Resolve(string defaultPath, string fileName)
        {
            if (ActiveDirectory is null)
                return defaultPath;

            return Path.Combine(ActiveDirectory, fileName);
        }

        public bool Exists(string name) => Prop.Profiles.Contains(name);

        public void Create(string name)
        {
            if (Exists(name))
                return;

            Prop.Profiles.Add(name);
            Save();
        }

        public void Delete(string name)
        {
            if (name == "default" || name == Prop.Active || !Exists(name))
                return;

            Prop.Profiles.Remove(name);
            Save();

            try
            {
                Directory.Delete(Path.Combine(Paths.Base, "Profiles", name), true);
            }
            catch (Exception ex)
            {
                App.Logger.WriteException("ProfileManager::Delete", ex);
            }
        }

        public void Switch(string name)
        {
            if (!Exists(name) || name == Prop.Active)
                return;

            Prop.Active = name;
            Save();
        }
    }
}
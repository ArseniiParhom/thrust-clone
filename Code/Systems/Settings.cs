using Godot;

namespace Thrust.Systems
{
    public class Settings
    {
        public const string SettingsFilePath = "user://settings.cfg";
        private SettingsData _data = null;

        public SettingsData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                ApplySettings(_data);
            }
        }

        public Settings()
        {
            Data = Load();
        }

        public Settings(SettingsData data)
        {
            Data = data;
        }
    
        public static async void ApplySettings(SettingsData data)
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), data.MasterVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), data.MusicVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), data.SfxVolume);
        }

        public void Save()
        {
            ConfigFile settingsFile = new ConfigFile();
            settingsFile.SetValue("Audio", "MasterVolume", Data.MasterVolume);
            settingsFile.SetValue("Audio", "MusicVolume", Data.MusicVolume);
            settingsFile.SetValue("Audio", "SfxVolume", Data.SfxVolume);

            settingsFile.Save(SettingsFilePath);
        }

        //TODO: Return loaded data.
        public SettingsData Load()
        {
            ConfigFile savedSettings = new ConfigFile();
            Error readError = savedSettings.Load(SettingsFilePath);
            SettingsData defaultData = new SettingsData();

            if (readError != Error.Ok)
            {
                return defaultData;
            }

            return new SettingsData()
            {
                MasterVolume = (float)savedSettings.GetValue("Audio", "MasterVolume", defaultData.MasterVolume),
                MusicVolume = (float)savedSettings.GetValue("Audio", "MusicVolume", defaultData.MusicVolume),
                SfxVolume = (float)savedSettings.GetValue("Audio", "SfxVolume", defaultData.SfxVolume)
            };
        }
    }
}
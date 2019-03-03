using System.Collections.Generic;
using System.Linq;
using eDream.GUI;
using EvilTools;
using Settings = EvilTools.Settings;

namespace eDream.libs
{
    public class DreamSettings : IDreamSettings
    {
        private const string SettingsFile = "settings.ini";
        private const string LoadLastDbSetting = "loadLastDatabase";
        private readonly LastOpened _recentlyOpened = new LastOpened();

        private readonly Settings _settings;

        public DreamSettings()
        {
            _settings = new Settings(SettingsFile, GetDefaultSettings());
            _recentlyOpened.LoadPaths();
        }

        public string LastDreamDatabase => RecentlyOpenedDiaries.FirstOrDefault();
        public bool ShouldLoadLastDreamDiary => _settings.GetValue(LoadLastDbSetting) == "yes";

        public IList<string> RecentlyOpenedDiaries => _recentlyOpened.GetPaths()
            .Where(path => !string.IsNullOrWhiteSpace(path)).ToList();

        public void AddPathToRecentlyOpenedPaths(string path)
        {
            _recentlyOpened.AddPath(path);
            _recentlyOpened.SavePaths();
        }

        private static List<Settings.SettingsPair> GetDefaultSettings()
        {
            return new List<Settings.SettingsPair>
            {
                new Settings.SettingsPair {Key = LoadLastDbSetting, Value = "yes"}
            };
        }
    }
}
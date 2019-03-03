using System.Collections.Generic;
using System.Linq;
using eDream.GUI;
using EvilTools;

namespace eDream.libs
{
    public class DreamDiaryPaths : IDreamDiaryPaths
    {
        private readonly LastOpened _recentlyOpened = new LastOpened();

        public DreamDiaryPaths()
        {
            _recentlyOpened.LoadPaths();
        }

        public string LastDreamDatabase => RecentlyOpenedDiaries.FirstOrDefault();

        public IList<string> RecentlyOpenedDiaries => _recentlyOpened.GetPaths()
            .Where(path => !string.IsNullOrWhiteSpace(path)).ToList();

        public void AddPathToRecentlyOpenedPaths(string path)
        {
            _recentlyOpened.AddPath(path);
            _recentlyOpened.SavePaths();
        }
    }
}
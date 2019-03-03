using System.Collections.Generic;

namespace eDream.GUI
{
    public interface IDreamSettings
    {
        string LastDreamDatabase { get; }
        bool ShouldLoadLastDreamDiary { get; }
        IList<string> RecentlyOpenedDiaries { get; }
        void AddPathToRecentlyOpenedPaths(string path);
    }
}
using System.Collections.Generic;

namespace eDream.GUI
{
    public interface IDreamDiaryPaths
    {
        string LastDreamDatabase { get; }
        IList<string> RecentlyOpenedDiaries { get; }
        void AddPathToRecentlyOpenedPaths(string path);
    }
}
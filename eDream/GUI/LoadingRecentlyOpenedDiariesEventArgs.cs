using System;
using System.Collections.Generic;
using eDream.Annotations;

namespace eDream.GUI
{
    public class LoadingRecentlyOpenedDiariesEventArgs : EventArgs
    {
        public IList<string> RecentlyOpenedDiaries { get; }

        public LoadingRecentlyOpenedDiariesEventArgs([NotNull] IList<string> recentlyOpenedDiaries)
        {
            RecentlyOpenedDiaries = recentlyOpenedDiaries ?? throw new ArgumentNullException(nameof(recentlyOpenedDiaries));
        }
    }
}
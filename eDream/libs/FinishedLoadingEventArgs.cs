using System;
using System.Collections.Generic;
using eDream.Annotations;
using eDream.program;

namespace eDream.libs
{
    public class FinishedLoadingEventArgs : EventArgs
    {
        public FinishedLoadingEventArgs(LoadingResult result, [NotNull] IEnumerable<DreamEntry> loadedDreamEntries)
        {
            Result = result;
            LoadedDreamEntries = loadedDreamEntries;
        }

        public LoadingResult Result { get; }
        public IEnumerable<DreamEntry> LoadedDreamEntries { get; }
    }
}
using System;
using System.Collections.Generic;
using eDream.program;

namespace eDream.libs
{
    public interface IDreamDiaryPersistenceService
    {
        event EventHandler<FinishedPersistingEventArgs> FinishedPersisting;
        event EventHandler<FinishedLoadingEventArgs> FinishedLoading;
        void PersistEntries(IEnumerable<DreamEntry> dreamEntries, string path);
        void LoadDiary(string path);
    }
}
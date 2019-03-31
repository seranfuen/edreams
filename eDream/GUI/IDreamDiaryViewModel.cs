using System;
using System.Collections.Generic;
using eDream.libs;
using eDream.program;

namespace eDream.GUI
{
    public interface IDreamDiaryViewModel : IDreamEntryProvider
    {
        string CurrentDatabasePath { get; set; }
        List<DreamDayEntry> DreamDays { get; }
        string FormText { get; }
        string StatusBarMessage { get; }
        event EventHandler LoadingFailed;
        event EventHandler LoadingSucceeded;
        event EventHandler PersistenceFailed;
        event EventHandler PersistenceSucceeded;
        void AddEntry(DreamEntry newEntry);
        void ClearFilteredEntries();
        List<DreamDayEntry> GetDayList();
        DreamTagStatisticsGenerator GetDreamTagStatistics();
        IList<string> GetRecentlyOpenedDiaryPaths();
        void LoadDiary();
        void LoadLastDiary();
        void Persist();
        void SetFilteredEntriesFromSearch(IEnumerable<DreamEntry> dreamEntries);
    }
}
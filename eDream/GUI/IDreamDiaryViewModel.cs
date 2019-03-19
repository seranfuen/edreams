using System;
using System.Collections.Generic;
using System.ComponentModel;
using eDream.libs;
using eDream.program;

namespace eDream.GUI
{
    public interface IDreamDiaryViewModel : IDreamEntryProvider
    {
        event EventHandler LoadingFailed;
        event EventHandler<LoadingRecentlyOpenedDiariesEventArgs> LoadingRecentlyOpenedDiaries;
        event EventHandler LoadingSucceeded;
        event EventHandler PersistenceFailed;
        event EventHandler PersistenceSucceeded;
        event PropertyChangedEventHandler PropertyChanged;
        string CurrentDatabasePath { get; set; }
        List<DreamDayEntry> DreamDays { get; }
        string FormText { get; }
        string StatusBarMessage { get; }
        IEnumerable<DreamEntry> DreamEntries { get; }
        void AddEntry(DreamEntry newEntry);
        void ClearFilteredEntries();
        void CloseCurrentDiary();
        List<DreamDayEntry> GetDayList();
        DreamTagStatisticsGenerator GetDreamTagStatistics();
        IList<string> GetRecentlyOpenedDiaryPaths();
        void LoadDiary();
        void LoadLastDiary();
        void Persist();
        void SetFilteredEntriesFromSearch(IEnumerable<DreamEntry> dreamEntries);
    }
}
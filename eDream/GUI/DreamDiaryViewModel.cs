﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using eDream.Annotations;
using eDream.libs;
using eDream.program;

namespace eDream.GUI
{
    public class DreamDiaryViewModel : INotifyPropertyChanged, IDreamDiaryViewModel
    {
        public event EventHandler<LoadingRecentlyOpenedDiariesEventArgs> LoadingRecentlyOpenedDiaries;
        private const string ApplicationName = "eDreams";
        private readonly IDreamDiaryPaths _dreamDiaryPaths;
        private readonly IDreamDiaryPersistenceService _dreamDiaryPersistenceService;
        private string _currentDatabasePath;
        private IList<DreamEntry> _dreamEntries = new List<DreamEntry>();
        private IEnumerable<DreamEntry> _filteredEntriesToShow;

        public DreamDiaryViewModel([NotNull] IDreamDiaryPersistenceService dreamDiaryPersistenceService,
            [NotNull] IDreamDiaryPaths dreamDiaryPaths)
        {
            _dreamDiaryPersistenceService = dreamDiaryPersistenceService ??
                                            throw new ArgumentNullException(nameof(dreamDiaryPersistenceService));
            _dreamDiaryPaths = dreamDiaryPaths ?? throw new ArgumentNullException(nameof(dreamDiaryPaths));
            _dreamDiaryPersistenceService.FinishedPersisting += DreamDiaryPersistenceServiceOnFinishedPersisting;
            _dreamDiaryPersistenceService.FinishedLoading += DreamDiaryPersistenceServiceOnFinishedLoading;
        }

        private int DayCount => DreamDays?.Count ?? 0;
        private int DreamCount => DreamDays?.SelectMany(x => x.DreamEntries).Count(x => !x.ToDelete) ?? 0;
        public event EventHandler LoadingFailed;

        public event EventHandler LoadingSucceeded;

        public event EventHandler PersistenceFailed;
        public event EventHandler PersistenceSucceeded;

        public string CurrentDatabasePath
        {
            get => _currentDatabasePath;
            set
            {
                _currentDatabasePath = value;
                if (string.IsNullOrWhiteSpace(value)) return;
                _dreamDiaryPaths.AddPathToRecentlyOpenedPaths(value);
            }
        }

        public List<DreamDayEntry> DreamDays => GetDayList();

        public string FormText => string.IsNullOrWhiteSpace(CurrentDatabasePath)
            ? $"{ApplicationName} ({GuiStrings.StatusBar_NoDreamDiaryLoaded})"
            : $"{ApplicationName} - {Path.GetFileName(CurrentDatabasePath)}";

        public string StatusBarMessage => DayCount <= 0
            ? GuiStrings.StatusBarMessage_NoDreams
            : string.Format(GuiStrings.StatusBarMessage_NumberDreamsAndDays, DreamCount, GetDreamWord(), DayCount,
                GetDayWord(), (decimal) DreamCount / DayCount);

        public IEnumerable<DreamEntry> DreamEntries => _dreamEntries;

        public void AddEntry(DreamEntry newEntry)
        {
            _dreamEntries.Add(newEntry);
        }

        public void ClearFilteredEntries()
        {
            _filteredEntriesToShow = null;
        }

        public List<DreamDayEntry> GetDayList()
        {
            return DreamCalendarCreator.GetDreamDayList(_filteredEntriesToShow ?? _dreamEntries);
        }

        public DreamTagStatisticsGenerator GetDreamTagStatistics()
        {
            return new DreamTagStatisticsGenerator(_dreamEntries);
        }

        public IList<string> GetRecentlyOpenedDiaryPaths()
        {
            return _dreamDiaryPaths.RecentlyOpenedDiaries;
        }

        public void LoadDiary()
        {
            _dreamDiaryPersistenceService.LoadDiary(CurrentDatabasePath);
        }

        public void LoadLastDiary()
        {
            OnLoadingRecentlyOpenedDiaries(_dreamDiaryPaths.RecentlyOpenedDiaries);
            if (string.IsNullOrWhiteSpace(_dreamDiaryPaths.LastDreamDatabase)) return;

            CurrentDatabasePath = _dreamDiaryPaths.LastDreamDatabase;
            LoadDiary();
        }

        public void Persist()
        {
            _dreamDiaryPersistenceService.PersistEntries(DreamEntries, CurrentDatabasePath);
        }

        public void SetFilteredEntriesFromSearch(IEnumerable<DreamEntry> dreamEntries)
        {
            _filteredEntriesToShow = dreamEntries;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CloseCurrentDiary()
        {
            CurrentDatabasePath = string.Empty;
            _dreamEntries = new List<DreamEntry>();
        }

        public DiaryImportResult Import([NotNull] IEnumerable<DreamEntry> dreamDiaryToImport)
        {
            if (dreamDiaryToImport == null) throw new ArgumentNullException(nameof(dreamDiaryToImport));

            var importedEntries = 0;
            var entriesToImport = dreamDiaryToImport.ToList();
            foreach (var entryToImport in entriesToImport)
                if (!DoesNotContain(entryToImport))
                {
                    AddEntry(entryToImport);
                    importedEntries++;
                }

            return new DiaryImportResult(entriesToImport.Count(), importedEntries);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool DoesNotContain(DreamEntry dreamEntry)
        {
            return _dreamEntries.Any(x => x.Date.Date == dreamEntry.Date.Date &&
                                          x.Text == dreamEntry.Text && x.GetTagString() == dreamEntry.GetTagString());
        }

        private void DreamDiaryPersistenceServiceOnFinishedLoading(object sender, FinishedLoadingEventArgs e)
        {
            switch (e.Result)
            {
                case LoadingResult.Error:
                    LoadingFailed?.Invoke(this, EventArgs.Empty);
                    break;
                case LoadingResult.Successful:
                    _dreamEntries = e.LoadedDreamEntries.ToList();
                    LoadingSucceeded?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private void DreamDiaryPersistenceServiceOnFinishedPersisting(object sender, FinishedPersistingEventArgs e)
        {
            switch (e.Result)
            {
                case PersistenceOperationResult.Error:
                    PersistenceFailed?.Invoke(this, EventArgs.Empty);
                    break;
                case PersistenceOperationResult.Successful:
                    PersistenceSucceeded?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private string GetDayWord()
        {
            return DayCount > 1 ? "days" : "day";
        }

        private string GetDreamWord()
        {
            return DreamCount > 1 ? "dreams" : "dream";
        }

        private void OnLoadingRecentlyOpenedDiaries(IList<string> recentlyOpenedDiaries)
        {
            LoadingRecentlyOpenedDiaries?.Invoke(this,
                new LoadingRecentlyOpenedDiariesEventArgs(recentlyOpenedDiaries));
        }
    }
}
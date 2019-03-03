using System;
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
    public class DreamDatabaseViewModel : INotifyPropertyChanged
    {
        public event EventHandler LoadingFailed;
        public event EventHandler<LoadingRecentlyOpenedDiariesEventArgs> LoadingRecentlyOpenedDiaries;

        public event EventHandler LoadingSucceeded;

        public event EventHandler PersistenceFailed;
        public event EventHandler PersistenceSucceeded;
        private const string ApplicationName = "eDreams";
        private readonly IDreamDiaryPaths _dreamDiaryPaths;
        private readonly IDreamDiaryPersistenceService _dreamDiaryPersistenceService;
        private string _currentDatabasePath;
        private IEnumerable<DreamEntry> _dreamEntries;
        private List<DreamDayEntry> _dreamList;

        public DreamDatabaseViewModel([NotNull] IDreamDiaryPersistenceService dreamDiaryPersistenceService,
            [NotNull] IDreamDiaryPaths dreamDiaryPaths)
        {
            _dreamDiaryPersistenceService = dreamDiaryPersistenceService ??
                                            throw new ArgumentNullException(nameof(dreamDiaryPersistenceService));
            _dreamDiaryPaths = dreamDiaryPaths ?? throw new ArgumentNullException(nameof(dreamDiaryPaths));
            _dreamDiaryPersistenceService.FinishedPersisting += DreamDiaryPersistenceServiceOnFinishedPersisting;
            _dreamDiaryPersistenceService.FinishedLoading += DreamDiaryPersistenceServiceOnFinishedLoading;
        }


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

        private IEnumerable<DreamEntry> DreamEntries => DreamList.SelectMany(x => x.DreamEntries);

        public List<DreamDayEntry> DreamList
        {
            get => _dreamList;
            set
            {
                _dreamList = value;
                OnPropertyChanged(nameof(DreamList));
            }
        }

        public string FormText => string.IsNullOrWhiteSpace(CurrentDatabasePath)
            ? $"{ApplicationName} ({GuiStrings.StatusBar_NoDreamDiaryLoaded})"
            : $"{ApplicationName} - {Path.GetFileName(CurrentDatabasePath)}";

        public string StatusBarMessage => DayCount <= 0
            ? GuiStrings.StatusBarMessage_NoDreams
            : string.Format(GuiStrings.StatusBarMessage_NumberDreamsAndDays, DreamCount, GetDreamWord(), DayCount,
                GetDayWord(), (decimal) DreamCount / DayCount);

        private int DayCount => DreamList?.Count ?? 0;
        private int DreamCount => DreamList?.SelectMany(x => x.DreamEntries).Count(x => !x.ToDelete) ?? 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DreamDayEntry> GetDayList()
        {
            return DreamCalendarCreator.GetDreamDayList(_dreamEntries);
        }

        public DreamTagStatistics GetDreamTagStatistics()
        {
            return new DreamTagStatistics(_dreamEntries);
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DreamDiaryPersistenceServiceOnFinishedLoading(object sender, FinishedLoadingEventArgs e)
        {
            switch (e.Result)
            {
                case LoadingResult.Error:
                    LoadingFailed?.Invoke(this, EventArgs.Empty);
                    break;
                case LoadingResult.Successful:
                    _dreamEntries = e.LoadedDreamEntries;
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
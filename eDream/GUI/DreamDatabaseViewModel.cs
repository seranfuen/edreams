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
        private const string ApplicationName = "eDreams";
        private readonly IDreamDiaryPersistenceService _dreamDiaryPersistenceService;
        private IEnumerable<DreamEntry> _dreamEntries;
        private List<DreamDayEntry> _dreamList;

        public DreamDatabaseViewModel([NotNull] IDreamDiaryPersistenceService dreamDiaryPersistenceService)
        {
            _dreamDiaryPersistenceService = dreamDiaryPersistenceService ??
                                            throw new ArgumentNullException(nameof(dreamDiaryPersistenceService));
            _dreamDiaryPersistenceService.FinishedPersisting += DreamDiaryPersistenceServiceOnFinishedPersisting;
            _dreamDiaryPersistenceService.FinishedLoading += DreamDiaryPersistenceServiceOnFinishedLoading;
        }


        public string CurrentDatabasePath { get; set; }

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

        public void LoadDiary()
        {
            _dreamDiaryPersistenceService.LoadDiary(CurrentDatabasePath);
        }

        public event EventHandler LoadingFailed;

        public event EventHandler LoadingSucceeded;

        public void Persist()
        {
            _dreamDiaryPersistenceService.PersistEntries(DreamEntries, CurrentDatabasePath);
        }

        public event EventHandler PersistenceFailed;
        public event EventHandler PersistenceSucceeded;

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

        public DreamTagStatistics GetDreamTagStatistics()
        {
            return new DreamTagStatistics(_dreamEntries);
        }
    }
}
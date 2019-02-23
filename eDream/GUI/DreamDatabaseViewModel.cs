using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using eDream.Annotations;
using eDream.program;

namespace eDream.GUI
{
    public class DreamDatabaseViewModel : INotifyPropertyChanged
    {
        private const string ApplicationName = "eDreams";
        private List<DreamDayEntry> _dreamList;

        public string CurrentDatabasePath { get; set; }

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

        private string GetDayWord()
        {
            return DayCount > 1 ? "days" : "day";
        }

        private string GetDreamWord()
        {
            return DreamCount > 1 ? "dreams" : "dream";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
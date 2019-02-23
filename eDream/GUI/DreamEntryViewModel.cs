using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using eDream.Annotations;
using eDream.program;

namespace eDream.GUI
{
    public class DreamEntryViewModel : INotifyPropertyChanged
    {
        private string _dreamText;

        public string DreamText
        {
            get => _dreamText;
            set
            {
                _dreamText = value;
                OnPropertyChanged(nameof(DreamText));
                OnPropertyChanged(nameof(WordCount));
                OnPropertyChanged(nameof(WordCountDisplay));
            }
        }

        public string Tags { get; set; }
        public DateTime DreamDate { get; set; }
        public int WordCount => Regex.Split(DreamText?.Trim() ?? "", @"\s").Count(x => !string.IsNullOrWhiteSpace(x));
        public string WordCountDisplay => $"Number of words: {WordCount}";
        public bool HasTextOrTags => !string.IsNullOrWhiteSpace(DreamText) || !string.IsNullOrWhiteSpace(Tags);

        public event PropertyChangedEventHandler PropertyChanged;

        public static DreamEntryViewModel ForNewDream()
        {
            return new DreamEntryViewModel {DreamDate = DateTime.Now.Date};
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static DreamEntryViewModel FromExistingEntry(DreamEntry editEntry)
        {
            return new DreamEntryViewModel
            {
                DreamText = editEntry.Text,
                DreamDate = editEntry.Date,
                Tags = editEntry.GetTagString()
            };
        }

        public DreamEntry ToDreamEntry()
        {
            return new DreamEntry(DreamDate, Tags, DreamText);
        }
    }
}
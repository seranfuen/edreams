using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using eDream.Annotations;
using eDream.program;

namespace eDream.GUI
{
    public class DreamDatabaseViewModel : INotifyPropertyChanged
    {
        private List<DreamDayEntry> _dreamList;

        public List<DreamDayEntry> DreamList
        {
            get => _dreamList;
            set
            {
                _dreamList = value;
                OnPropertyChanged(nameof(DreamList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
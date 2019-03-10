using System;
using eDream.Annotations;
using eDream.GUI;

namespace eDream.program
{
    public class DreamDiaryBus : IDreamDiaryBus
    {
        private readonly DreamDiaryViewModel _dreamDiary;
        private FrmSearchForm _frmSearchWindow;

        public DreamDiaryBus([NotNull] DreamDiaryViewModel dreamDiary)
        {
            _dreamDiary = dreamDiary ?? throw new ArgumentNullException(nameof(dreamDiary));
        }

        public event EventHandler<SearchPerformedEventArgs> SearchPerformed;
        public event EventHandler DiaryPersisted;

        public void PersistDiary()
        {
            _dreamDiary.Persist();
            DiaryPersisted?.Invoke(this, EventArgs.Empty);
        }

        public void EditEntry([NotNull] DreamEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            var editEntry = new NewEntryForm(entry, _dreamDiary.GetDreamTagStatistics().TagStatistics);
            editEntry.ShowDialog();
            PersistDiary();
        }

        public void AddNewEntry()
        {
            var addEntryBox = new NewEntryForm(_dreamDiary.GetDreamTagStatistics().TagStatistics);
            addEntryBox.ShowDialog();
            if (!addEntryBox.CreatedEntry) return;
            _dreamDiary.AddEntry(addEntryBox.NewEntry);
            PersistDiary();
        }

        public void OpenSearchDialog()
        {
            if (_frmSearchWindow != null && _frmSearchWindow.Visible)
            {
                _frmSearchWindow.Focus();
            }
            else if (_frmSearchWindow == null)
            {
                _frmSearchWindow = new FrmSearchForm(_dreamDiary);
                _frmSearchWindow.SearchCompleted += (s, e) =>
                {
                    if (e.SearchResult == null) _dreamDiary.ClearFilteredEntries();
                    else _dreamDiary.SetFilteredEntriesFromSearch(e.SearchResult);
                    SearchPerformed?.Invoke(this, e);
                };
                _frmSearchWindow.Show();
            }
            else
            {
                _frmSearchWindow.Visible = true;
            }
        }
    }
}
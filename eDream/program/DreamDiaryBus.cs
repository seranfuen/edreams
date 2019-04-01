using System;
using System.Windows.Forms;
using eDream.Annotations;
using eDream.GUI;
using eDream.libs;

namespace eDream.program
{
    public class DreamDiaryBus : IDreamDiaryBus
    {
        private readonly IDreamDiaryViewModel _dreamDiary;
        private readonly IDreamReaderWriterFactory _dreamReaderWriterFactory;
        private FrmSearchForm _frmSearchWindow;

        public DreamDiaryBus([NotNull] IDreamDiaryViewModel dreamDiary,
            IDreamReaderWriterFactory dreamReaderWriterFactory)
        {
            _dreamDiary = dreamDiary ?? throw new ArgumentNullException(nameof(dreamDiary));
            _dreamReaderWriterFactory = dreamReaderWriterFactory;
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
            var editEntry = new FrmDreamEntry(entry, _dreamDiary.GetDreamTagStatistics().GetStatistics());
            editEntry.ShowDialog();
            PersistDiary();
        }

        public void AddNewEntry()
        {
            var addEntryBox = new FrmDreamEntry(_dreamDiary.GetDreamTagStatistics().GetStatistics());
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

        public void ImportDiary(string fileName)
        {
            using (var reader = _dreamReaderWriterFactory.CreateReader())
            {
                if (reader.IsFileValid(fileName))
                {
                    var entries = reader.LoadFile(fileName);
                    var result = _dreamDiary.Import(entries);
                    _dreamDiary.Persist();
                    MessageBox.Show(
                        string.Format(GuiStrings.ImportResult, result.EntriesImported, result.EntriesInImportedDiary),
                        GuiStrings.ImportResultSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(GuiStrings.ImportError, fileName),
                        GuiStrings.InvalidDiaryImportTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
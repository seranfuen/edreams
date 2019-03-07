using System;
using eDream.Annotations;
using eDream.GUI;
using eDream.libs;

namespace eDream.program
{
    public class DreamDiaryBus : IDreamDiaryBus
    {
        private readonly DreamDiaryViewModel _dreamDiary;

        public DreamDiaryBus([NotNull] DreamDiaryViewModel dreamDiary)
        {
            _dreamDiary = dreamDiary ?? throw new ArgumentNullException(nameof(dreamDiary));
        }

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
    }
}
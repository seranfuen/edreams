using System;
using eDream.GUI;

namespace eDream.program
{
    public interface IDreamDiaryBus
    {
        event EventHandler<SearchPerformedEventArgs> SearchPerformed;
        event EventHandler DiaryPersisted;
        void PersistDiary();
        void EditEntry(DreamEntry entry);
        void AddNewEntry();
        void OpenSearchDialog();
        void ImportDiary(string fileName);
    }
}
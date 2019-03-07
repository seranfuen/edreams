using System;

namespace eDream.program
{
    public interface IDreamDiaryBus
    {
        event EventHandler DiaryPersisted;
        void PersistDiary();
        void EditEntry(DreamEntry entry);
        void AddNewEntry();
    }
}
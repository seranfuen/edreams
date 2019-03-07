using System;
using eDream.Annotations;
using eDream.program;

namespace eDream.GUI
{
    public class EntryViewerModel
    {
        private readonly IDreamDiaryBus _dreamDiaryBus;
        private readonly DreamEntry _entry;
        private readonly int _entryNumber;

        private EntryViewerModel([NotNull] DreamEntry entry, int entryNumber, [NotNull] IDreamDiaryBus bus)
        {
            _entry = entry ?? throw new ArgumentNullException(nameof(entry));
            _entryNumber = entryNumber;
            _dreamDiaryBus = bus ?? throw new ArgumentNullException(nameof(bus));
            DreamDate = entry.Date.ToShortDateString();
            DreamText = entry.Text;
            DreamTags = entry.GetTagString();
        }

        public string DreamDate { get; }
        public string EntryNumber => $"Entry #{_entryNumber}";
        public string DreamText { get; }
        public string DreamTags { get; }

        // Why make this a static method? Because I'm initializing EntryViewerModel from another entity (i.e. DreamEntry)
        // even if the constructor, for convenience, is going to take the same parameters, I prefer to hide this fact from callers.
        // The method makes it clear we're doing some work behind the scenes to set the properties of EntityViewerModel
        // from another object. This would not be clear from the constructor
        // If at some point I have the need to provide the properties myself, then I will make a public constructor for that
        public static EntryViewerModel FromEntry([NotNull] DreamEntry entry, int entryNumber,
            [NotNull] IDreamDiaryBus dreamDiaryBus)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            if (dreamDiaryBus == null) throw new ArgumentNullException(nameof(dreamDiaryBus));
            return new EntryViewerModel(entry, entryNumber, dreamDiaryBus);
        }

        public void DeleteEntry()
        {
            _entry.ToDelete = true;
            _dreamDiaryBus.PersistDiary();
        }

        public void EditEntry()
        {
            _dreamDiaryBus.EditEntry(_entry);
        }
    }
}
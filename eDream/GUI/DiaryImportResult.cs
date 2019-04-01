namespace eDream.GUI
{
    public class DiaryImportResult
    {
        public DiaryImportResult(int entriesInImportedDiary, int entriesImported)
        {
            EntriesInImportedDiary = entriesInImportedDiary;
            EntriesImported = entriesImported;
        }

        public int EntriesInImportedDiary { get; }
        public int EntriesImported { get; }

        public override string ToString()
        {
            return
                $"{nameof(EntriesInImportedDiary)}: {EntriesInImportedDiary}, {nameof(EntriesImported)}: {EntriesImported}";
        }
    }
}
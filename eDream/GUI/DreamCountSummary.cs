namespace eDream.GUI
{
    public class DreamCountSummary
    {
        public DreamCountSummary(int dreamCount, int daysWithDreams)
        {
            DreamCount = dreamCount;
            DaysWithDreams = daysWithDreams;
        }

        public int DreamCount { get; }
        public int DaysWithDreams { get; }

        public override string ToString()
        {
            return $"{nameof(DreamCount)}: {DreamCount}, {nameof(DaysWithDreams)}: {DaysWithDreams}";
        }
    }
}
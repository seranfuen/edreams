using System.Collections.Generic;

namespace eDream.program
{
    public interface IDreamEntryProvider
    {
        IEnumerable<DreamEntry> DreamEntries { get; }
    }
}
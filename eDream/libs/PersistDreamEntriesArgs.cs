using System;
using System.Collections.Generic;
using eDream.Annotations;
using eDream.program;

namespace eDream.libs
{
    public class PersistDreamEntriesArgs
    {
        public PersistDreamEntriesArgs([NotNull] string path, [NotNull] IEnumerable<DreamEntry> entries)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Entries = entries ?? throw new ArgumentNullException(nameof(entries));
        }

        public string Path { get; }
        public IEnumerable<DreamEntry> Entries { get; }
    }
}
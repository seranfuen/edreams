using System;
using System.Collections.Generic;
using eDream.program;

namespace eDream.libs
{
    public interface IDiaryReader : IDisposable
    {
        bool IsFileValid(string filename);
        IEnumerable<DreamEntry> LoadFile(string filename);
    }
}
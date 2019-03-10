using System;
using System.Collections.Generic;
using eDream.program;

namespace eDream.GUI
{
    public class SearchPerformedEventArgs : EventArgs
    {
        public IEnumerable<DreamEntry> SearchResult { get; }

        public SearchPerformedEventArgs(IEnumerable<DreamEntry> searchResult)
        {
            SearchResult = searchResult;
        }
    }
}
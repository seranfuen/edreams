/****************************************************************************
 * FrmMain: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Ángel Verbo
 ****************************************************************************/
/****************************************************************************
    This file is part of FrmMain.

    FrmMain is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FrmMain is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FrmMain.  If not, see <http://www.gnu.org/licenses/gpl-3.0.html>.]
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using eDream.Annotations;
using eDream.program;

namespace eDream.libs
{
    public class DreamTagStatistics
    {
        private readonly IEnumerable<DreamEntry> _dreamEntries;
        private readonly List<DreamMainStatTag> _tagStatistics = new List<DreamMainStatTag>();

        public DreamTagStatistics([NotNull] IEnumerable<DreamEntry> dreamEntries)
        {
            _dreamEntries = dreamEntries ?? throw new ArgumentNullException(nameof(dreamEntries));
            var dreamEntryList = dreamEntries.ToList();
            TotalDays = dreamEntryList.GroupBy(dream => dream.Date.Date).Count();
            TotalEntries = dreamEntryList.Count;
            GenerateStatistics();
        }

        public int TotalDays { get; }

        public int TotalEntries { get; }

        public List<DreamMainStatTag> TagStatistics => _tagStatistics.ToList();

        private void GenerateStatistics()
        {
            foreach (var dreamEntry in _dreamEntries)
            {
                if (!dreamEntry.IsValid) continue;

                var tags = dreamEntry.GetTagsAsList();

                foreach (var tag in tags)
                {
                    var mainStatTag = GetExistingOrCreateMainTag(tag.Tag);
                    mainStatTag.IncreaseCount();
                    foreach (var childTag in tag.ChildTags) mainStatTag.IncreaseChildCount(childTag.Tag);
                }
            }

            TagStatistics.Sort();
            TagStatistics.Reverse();
        }

        private DreamMainStatTag GetExistingOrCreateMainTag(string tagName)
        {
            var existingTag = TagStatistics.SingleOrDefault(x => x.IsTag(tagName));
            if (existingTag != null) return existingTag;

            var newStatTag = new DreamMainStatTag(tagName);
            _tagStatistics.Add(newStatTag);
            return newStatTag;
        }
    }
}
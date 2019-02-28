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
using eDream.program;

namespace eDream.libs
{
    public class DreamTagStatistics
    {
        private List<DreamDayEntry> _dayList;

        private List<DreamEntry> _dreamEntries;

        public int TotalDays { get; private set; }

        public int TotalEntries { get; private set; }

        public List<DreamMainStatTag> TagStatistics { get; private set; }

        public void GenerateStatistics(List<DreamEntry> newEntries,
            List<DreamDayEntry> dayList)
        {
            _dreamEntries = newEntries;
            _dayList = dayList;
            TagStatistics = new List<DreamMainStatTag>();
            // Process all the dreamEntries list
            foreach (var t in _dreamEntries)
            {
                // Skip if invalid entry (e.g. set to delete)
                if (!t.GetIfValid()) continue;
                var iTags = t.GetTagsAsList();
                /**
                 * Loops through all the tags obtained from the entry and
                 * searches the current tagStatistics list. If they are found,
                 * their count will be increased. If not found, a new StatTag
                 * will be created at the end of the list
                 */
                foreach (var tag in iTags)
                {
                    // Tag will be at this position if it is added to the list
                    var mainTagPos = TagStatistics.Count;
                    // Search for a tentative position of an already existing list
                    var pos = IsInList(TagStatistics, tag);
                    if (pos == -1)
                        TagStatistics.Add(new DreamMainStatTag(tag.Tag));
                    else
                        mainTagPos = pos;
                    TagStatistics[mainTagPos].IncreaseCount();

                    var iChildTags = tag.ChildTags;
                    foreach (var childTag in iChildTags) TagStatistics[mainTagPos].IncreaseChildCount(childTag.Tag);
                }
            }

            TagStatistics.Sort();
            TagStatistics.Reverse();
            CountEntriesAndDays();
        }

        private void CountEntriesAndDays()
        {
            TotalDays = 0;
            // Days are considered valid if they contain at least one entry
            foreach (var day in _dayList)
                if (day.Count > 0)
                    TotalDays++;
            TotalEntries = 0;
            foreach (var entry in _dreamEntries)
                if (entry.GetIfValid())
                    TotalEntries++;
        }

        private static int IsInList(IReadOnlyList<DreamMainStatTag> list, DreamTag element)
        {
            if (element == null || list == null) return -1;
            for (var i = 0; i < list.Count; i++)
                if (string.Equals(list[i].Tag, element.Tag, StringComparison.CurrentCultureIgnoreCase))
                    return i;
            return -1;
        }
    }
}
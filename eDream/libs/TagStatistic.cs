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

using eDream.Annotations;

namespace eDream.libs
{
    public class TagStatistic
    {
        private TagStatistic(string tagName, string parentTagName, int tagCount,
            decimal? tagCountOverTotalEntriesRatio,
            decimal daysWithTagOverTotalDaysRatio, decimal? tagCountOverParentTagCountRatio)
        {
            Tag = tagName;
            Count = tagCount;
            TagCountToTotalEntriesRatio = tagCountOverTotalEntriesRatio;
            DaysWithTagOfTimesTagAppearsOverTotalDaysRatio = daysWithTagOverTotalDaysRatio;
            ParentTag = parentTagName;
            TagCountOverParentTagCountRatio = tagCountOverParentTagCountRatio;
        }

        public string Tag { get; }

        public string TagDisplay => ParentTag != null ? $"    {Tag}" : Tag;

        public int Count { get; }

        public decimal? TagCountToTotalEntriesRatio { get; }

        [UsedImplicitly]
        public string PercentOfTotalEntriesDisplay =>
            $"{(TagCountToTotalEntriesRatio ?? TagCountOverParentTagCountRatio) * 100:n1} %";

        [UsedImplicitly]
        public string PercentOfTotalDaysDisplay => $"{DaysWithTagOfTimesTagAppearsOverTotalDaysRatio * 100:n1} %";

        public decimal? TagCountOverParentTagCountRatio { get; }
        public decimal DaysWithTagOfTimesTagAppearsOverTotalDaysRatio { get; }
        public string ParentTag { get; }

        public static TagStatistic ForChildTag(string tagName, string parentTagName, int tagCount,
            decimal tagCountOverParentTagRatio,
            decimal daysWithTagOverTotalDaysRatio)
        {
            return new TagStatistic(tagName, parentTagName, tagCount, null,
                daysWithTagOverTotalDaysRatio, tagCountOverParentTagRatio);
        }

        public static TagStatistic ForMainTag(string tagName, int tagCount,
            decimal tagCountOverTotalEntriesRatio,
            decimal daysWithTagOverTotalDaysRatio)
        {
            return new TagStatistic(tagName, null, tagCount, tagCountOverTotalEntriesRatio,
                daysWithTagOverTotalDaysRatio, null);
            ;
        }
    }
}
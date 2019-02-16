/****************************************************************************
 * eDreams: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Angel Verbo
 ****************************************************************************/
/****************************************************************************
    This file is part of eDreams.

    eDreams is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    eDreams is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with eDreams.  If not, see <http://www.gnu.org/licenses/gpl-3.0.html>.]
****************************************************************************/

using System;
using System.Collections.Generic;

namespace eDream.program
{
    /// <summary>
    ///     This class represents a particular calendar day that contains one or
    ///     more dream entries. It therefore has a date and a list of dream entries
    ///     for that date
    /// </summary>
    public class DreamDayList : IComparable<DreamDayList>
    {
        private readonly List<DreamEntry> _dreamEntries = new List<DreamEntry>();

        private DateTime _date;

        public DreamDayList(DateTime date, DreamEntry entry)
        {
            Date = date;
            AddDreamEntry(entry);
        }

        public DateTime Date
        {
            get => _date;
            private set => _date = new DateTime(value.Year, value.Month, value.Day);
        }

        public List<DreamEntry> DreamEntries => _dreamEntries ?? new List<DreamEntry>();

        /// <inheritdoc />
        /// <summary>
        ///     Compares the dates of two DreamDayList objects
        /// </summary>
        /// <param name="dreamDay"></param>
        /// <returns></returns>
        public int CompareTo(DreamDayList dreamDay)
        {
            return _date.CompareTo(dreamDay.Date);
        }

        public bool IsSameDate(DateTime date)
        {
            return Date.CompareTo(date) == 0;
        }

        /// <summary>
        ///     Returns the number of valid dream entries contained by the object
        /// </summary>
        /// <returns></returns>
        public int GetNumberEntries()
        {
            var count = 0;
            for (var i = 0; i < _dreamEntries.Count; i++)
                if (!_dreamEntries[i].ToDelete)
                    count++;
            return count;
        }

        public void AddDreamEntry(DreamEntry entry)
        {
            _dreamEntries.Add(entry);
        }

        public override string ToString()
        {
            return _date.ToString("dd-MM-yyyy") + " (" + GetNumberEntries() +
                   ")";
        }
    }
}
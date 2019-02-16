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
using System.Linq;

namespace eDream.program
{
    /// <inheritdoc />
    /// <summary>
    ///     This class represents a particular calendar day that contains one or
    ///     more dream entries. It therefore has a date and a list of dream entries
    ///     for that date
    /// </summary>
    public class DreamDayList : IComparable<DreamDayList>
    {
        public DreamDayList(DateTime date, List<DreamEntry> dreamEntries)
        {
            Date = date.Date;
            DreamEntries = dreamEntries ?? throw new ArgumentNullException(nameof(dreamEntries));
        }

        public DateTime Date { get; }

        public List<DreamEntry> DreamEntries { get; }

        public int Count => DreamEntries.Count(entry => !entry.ToDelete);

        public int CompareTo(DreamDayList dreamDay)
        {
            return Date.CompareTo(dreamDay.Date);
        }

        public override string ToString()
        {
            return Date.ToString("dd-MM-yyyy") + " (" + Count +
                   ")";
        }
    }
}
﻿/****************************************************************************
 * eDreams: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Ángel Verbo
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

using System.Collections.Generic;
using System.Linq;
using eDream.program;

namespace eDream.libs
{
    /// <summary>
    ///     Create a "calendar", a list of DreamDayList objects that represent
    ///     an individual date
    /// </summary>
    public class DreamCalendarCreator
    {
        public static List<DreamDayList> GetDreamDayList(List<DreamEntry> entries)
        {
            var query =
                from entry in entries
                where entry.GetIfValid()
                group entry by entry.Date
                into g
                select g;

            return query.Select(g => new DreamDayList(g.Key, g.ToList())).ToList();
         }
    }
}
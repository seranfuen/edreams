/****************************************************************************
 * eDreams: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012, Sergio Ángel Verbo
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
using System.Text;
using eDream.program;

namespace eDream.libs {
    /// <summary>
    /// Create a "calendar", a list of DreamDayList objects that represent
    /// an individual date
    /// </summary>
    class DreamCalendarCreator {
        /// <summary>
        /// This function parses a list of dream entries, creating a list of
        /// DreamDayList that represents a particular date. Each object
        /// representing a date will contain the dream entries that correspond
        /// to that date
        /// </summary>
        public static List<DreamDayList> GetDreamDayList(List<DreamEntry> entries) {
            List<DreamDayList> dayList = new List<DreamDayList>();
            bool found;
            for (int i = 0; i < entries.Count; i++) {
                DreamEntry theEntry = entries[i];
                found = false;
                if (theEntry.GetIfValid()) {
                    // Find an already existing date
                    for (int j = 0; j < dayList.Count; j++) {
                        if (dayList[j].IsSameDate(theEntry.GetDate())) {
                            found = true;
                            dayList[j].AddDreamEntry(theEntry);
                            break;
                        }
                    }
                    if (!found) {
                        dayList.Add(new DreamDayList(theEntry.GetDate(), 
                            theEntry));
                    }
                }
            }
            return dayList;
        }
    }
}

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

namespace eDream.program {
    /// <summary>
    /// This class represents a particular calendar day that contains one or
    /// more dream entries. It therefore has a date and a list of dream entries
    /// for that date
    /// </summary>
    class DreamDayList : IComparable<DreamDayList> {

        private List<DreamEntry> dreamEntries = new List<DreamEntry>();
        
        private DateTime date;

        /// <summary>
        /// Sets or gets the date the object represents
        /// </summary>
        public DateTime Date {
            get {
                return this.date;
            }
            set {
                this.date = new DateTime(value.Year, value.Month, value.Day);
            }
        }

        /// <summary>
        /// The list of dream entries that the object contains and that 
        /// correspond to the date the object represents
        /// </summary>
        public List<DreamEntry> DreamEntries {
            get {
                return this.dreamEntries != null ? this.dreamEntries : 
                    new List<DreamEntry>();
            }
        }

        public DreamDayList(DateTime date) {
            Date = date;
        }

        /// <summary>
        /// Shorthand constructor since most of the time an object will be
        /// created once we need to contain an entry for the day
        /// </summary>
        /// <param name="date"></param>
        /// <param name="entry">A dream entry that will be added to the list
        /// when the object is created</param>
        public DreamDayList(DateTime date, DreamEntry entry) {
            Date = date;
            AddDreamEntry(entry);
        }


        /// <summary>
        /// Compares the DateTime object provided with the internal DateTime
        /// object contained that represents the date of the object and returns
        /// true if they are the same date -time is ignored-, false if not
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsSameDate(DateTime date) {
            return (Date.CompareTo(date) == 0);
        }

        /// <summary>
        /// Returns the number of valid dream entries contained by the object
        /// </summary>
        /// <returns></returns>
        public int GetNumberEntries() {
            int count = 0;
            for (int i = 0; i < dreamEntries.Count; i++) {
                if (!dreamEntries[i].ToDelete) {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Adds a new dream entry to the object
        /// </summary>
        /// <param name="entry"></param>
        public void AddDreamEntry(DreamEntry entry) {
            dreamEntries.Add(entry);
        }

        /// <summary>
        /// Compares the dates of two DreamDayList objects
        /// </summary>
        /// <param name="dreamDay"></param>
        /// <returns></returns>
        public int CompareTo(DreamDayList dreamDay) {
            return date.CompareTo(dreamDay.Date);
        }

        /// <summary>
        /// Returns a string representation of the object, that shows
        /// the date and the number of entries. In principle it is used to
        /// pass a listbox a list of DreamDayList objects and display that text
        /// </summary>
        /// <returns></returns>
        override public String ToString() {
            return date.ToString("dd-MM-yyyy") + " (" + GetNumberEntries() +
                ")";
        }
    }
}

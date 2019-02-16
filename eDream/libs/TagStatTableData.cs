/****************************************************************************
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
using System;
using System.Collections.Generic;
using System.Text;
using EvilTools;

namespace eDream.libs {
    /// <summary>
    /// This object is passed to the table used to display tag statistics.
    /// Each object contains data about one tag
    /// </summary>
    class TagStatTableData {
        private string tagName;
        private int tagCount;
        private int totalEntries;
        private bool isChild;
        private const string childTab = "     ";

        /// <summary>
        /// The objects that represent the information shown in the table
        /// containing the tag statistics
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="tagCount"></param>
        /// <param name="totalEntries"></param>
        /// <param name="isChild"></param>
        public TagStatTableData(string tagName, int tagCount, int totalEntries, bool
            isChild) {
            this.tagName = tagName;
            this.tagCount = tagCount;
            this.totalEntries = totalEntries;
            this.isChild = isChild;
        }

        /// <summary>
        /// Gets or sets the tag name, formatted to be displayed
        /// </summary>
        public string Tag {
            get {
                return !isChild ? tagName : childTab + tagName;
            }
            set {
                tagName = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of times the tag has appeared
        /// </summary>
        public int Count {
            get {
                return tagCount;
            }
            set {
                tagCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the percentage of number of times the tag has appeared
        /// compared to the total number of entries
        /// </summary>
        public string Percentage {
            get {
                return ! isChild && Count > 0 ? 
                    StringUtils.GeneratePercentageAsStr(tagCount, totalEntries) 
                    + "%" :
                    "";
            }
        }
    }
}

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

namespace eDream.libs {
    /// <summary>
    /// This object is passed to the table used to display the tags that have been
    /// used at least once.
    /// </summary>
    public class TagTableData {
        private string tagName;
        private int tagCount;

        /// <summary>
        /// Gets or sets the tag name
        /// </summary>
        public string Tag {
            get {
                return tagName;
            }
            set {
                tagName = value;
            }
        }

        /// <summary>
        /// Gets or sets the tag count
        /// </summary>
        public int Count {
            get {
                return tagCount;
            }
            set {
                tagCount = value;
            }
        }
    }
}

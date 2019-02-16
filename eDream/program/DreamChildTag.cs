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
using eDream.libs;
using EvilTools;

namespace eDream.program {
    /// <summary>
    /// DreamChildTag objects represent child/subtags belonging to a main tag,
    /// usually clarifying or specifying what type of dream the main tag
    /// represents.
    /// </summary>
    public class DreamChildTag : DreamTag {

        /// <summary>
        /// The parent tag name
        /// </summary>
        protected string parentTagName;

        /// <summary>
        /// </summary>
        /// <param name="tagName">The name given to the tag</param>
        /// <param name="parentTagName">The name of its parent tag</param>
        public DreamChildTag(string tagName, string parentTagName) {
            TagName = tagName;
            ParentTagName = parentTagName;
        }

        /// <summary>
        /// The name of the parent tag where the child tag is contained
        /// </summary>
        public string ParentTagName {
            set {
                parentTagName = StringUtils.CapitalizeString(value);
            }
            get {
                return parentTagName;
            }
        }
    }
}

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
    /// Dream tags are tags -labels- used to sort dreams by type, theme, etc.,
    /// and to generate statistics based on this
    /// DreamMainTag represents main tags, which can contain any number of
    /// child tags that clarify or specify the meaning or scope of the main
    /// tag
    /// </summary>
    class DreamMainTag : DreamTag {
        /// <summary>
        /// A list of the child tags belonging to the parent -main- tag
        /// </summary>
        protected List<DreamChildTag> childTags = new List<DreamChildTag>();

        /// <summary>
        /// Constructor for a main tag
        /// </summary>
        /// <param name="tagName">Name given to the tag</param>
        public DreamMainTag(string tagName) {
            this.TagName = tagName;
        }

        /// <summary>
        /// The child tags contained by the main tag
        /// </summary>
        public List<DreamChildTag> ChildTags {  
            get {
                return childTags;
            }
        }

        /// <summary>
        /// Creates a new child tag object with the name provided and adds it
        /// to the list of child tags
        /// </summary>
        /// <param name="childTagName">Name of the child tag</param>
        /// <returns>True if tag successfully added, false if tag was already
        /// present, so it wasn't added</returns>
        public bool AddChildTag(string childTagName) {
            if (HasChildTag(childTagName)) {
                return false;
            }
            childTags.Add(new DreamChildTag(childTagName, this.TagName));
            return true;
        }

        /// <summary>
        /// Compares the child tag name given with the list of child tags
        /// and returns true if found, false if not found
        /// </summary>
        /// <param name="childTagName">Name of child tag we are searching</param>
        /// <returns></returns>
        public bool HasChildTag(string childTagName) {
            for (int i = 0; i < childTags.Count; i++) {
                if (String.Compare(childTagName, childTags[i].TagName, true) 
                    == 0) {
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Counts the number of valid child tags contained by the main tag
        /// </summary>
        /// <returns>Number of child tags</returns>
        public int CountChildTags() {
            return childTags.Count;
        }
    }
}

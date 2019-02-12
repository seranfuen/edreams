﻿/****************************************************************************
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
    /// It represents a main tag (a tag that can have children) that is used
    /// for statistics purposes, so a counter is added to keep track of
    /// how many times the tag has been found (for example in the whole database)
    /// </summary>
    class DreamMainStatTag : DreamMainTag, IComparable<DreamMainStatTag> {
        
        // Number of times the tag has been found in all the entries
        private int tagCount = 0;

        /* List of DreamChildStatTags, redefined from main tag because we are
         * using DreamChildStatTag children to hold statistics, rather than 
         * ChildStatTag objects */
        new private List<DreamChildStatTag> childTags = 
            new List<DreamChildStatTag>();

        /// <summary>
        /// The list of DreamChildStatTag objects the main tag contains. It is
        /// returned already sorted by count, descending order.
        /// </summary>
        new public List<DreamChildStatTag> ChildTags {
            get {
                SortChildTags();
                return childTags;
            }
        }

        /// <summary>
        /// The number of times the tag has been found
        /// </summary>
        public int TagCount {
            get {
                return tagCount >= 0 ? tagCount : 0;
            }
        }

        public DreamMainStatTag(string tagName) : base(tagName) { }

        /// <summary>
        /// Increments the tag count by one
        /// </summary>
        public void IncreaseCount() {
            if (tagCount < 0) {
                tagCount = 0;
            }
            tagCount++;
        }

        /// <summary>
        /// This method will increase the count of a child tag by one.
        /// If the child tag is found, its counter is increased by one. If it
        /// is not found, a new child stat tag will be added and its counter
        /// set to 1
        /// </summary>
        /// <param name="childName">Name of the child tag whose counter
        /// we want to increase</param>
        public void IncreaseChildCount(string childName) {
                DreamChildStatTag childTag = GetChildStatTag(childName);
                if (childTag == null) {
                    DreamChildStatTag newTag = new DreamChildStatTag(childName,
                        TagName);
                    // Increase count since adding one means it was already found
                    newTag.IncreaseCount();
                    childTags.Add(newTag);
                }
                else { // If not necessary to add new child tag, just increase it
                    childTag.IncreaseCount();
                }
        }

        /// <summary>
        /// Returns the number of times a child tag as appeared or -1 if the
        /// child tag was not found
        /// </summary>
        /// <param name="childName">Name of DreamChildStatTag</param>
        /// <returns>Number of times it has appeared or -1 if not found</returns>
        public int GetChildTagCount(string childName) {
            DreamChildStatTag theTag = GetChildStatTag(childName);
            return theTag == null ? -1 : theTag.TagCount;
        }

        /// <summary>
        /// Counts the number of valid child tags contained by the main tag
        /// </summary>
        /// <returns>Number of child tags</returns>
        public new int CountChildTags() {
            return childTags.Count;
        }

        /// <summary>
        /// A method implemented by the IComparer interface, it compares the
        /// counter of two DreamMainStatTag objects and returns which is 
        /// larger
        /// </summary>
        /// <param name="o">A DreamMainStatTag to compare to</param>
        /// <returns></returns>
        public int CompareTo(DreamMainStatTag o) {
            if (o == null) {
                return 0;
            }
            int comp = tagCount.CompareTo(o.TagCount);
            // If tag cont is the same, sort them by name
            if (comp == 0) {
                return -tagName.CompareTo(o.tagName);
            }
            return comp;
        }

        /// <summary>
        /// Returns a child stat tag matching the name given or null if child
        /// tag was not found. Care should be taken because there can be 
        /// null returns
        /// </summary>
        /// <param name="childTag">Name of child stat tag. Non case sensitive</param>
        /// <returns>Null if not found, or the DreamChildStatTag object</returns>
        private DreamChildStatTag GetChildStatTag(string childTag) {
             for (int i = 0; i < childTags.Count; i++) {
                if (String.Compare(childTags[i].TagName, childTag, true) 
                    == 0) {
                    return childTags[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Sorts by count in reverse order the list of ChildTags
        /// </summary>
        private void SortChildTags() {
            childTags.Sort();
            childTags.Reverse();
        }
    }
}
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

namespace eDream.program
{
    /// <summary>
    ///     Represents a child tag (a tag that is contained in a main tag but which
    ///     cannot contain itself any child tags) used for statistics purposes, so
    ///     a counter of how many times it has appeared is kept
    /// </summary>
    internal class DreamChildStatTag : DreamChildTag, IComparable<DreamChildStatTag>
    {
        /// <summary>
        ///     Times it has appeared
        /// </summary>
        private int tagCount;

        public DreamChildStatTag(string tag, string parentName) :
            base(tag)
        {
        }

        /// <summary>
        ///     The number of times the tag has been found
        /// </summary>
        public int TagCount => tagCount >= 0 ? tagCount : 0;


        /// <summary>
        ///     A method implemented by the IComparer interface, it compares the
        ///     counter of two DreamChildStatTag objects and returns which is
        ///     larger
        /// </summary>
        /// <param name="o">A DreamChildStatTag to compare to</param>
        /// <returns></returns>
        public int CompareTo(DreamChildStatTag o)
        {
            if (o == null) return 0;
            var comp = tagCount.CompareTo(o.TagCount);
            // If tag cont is the same, sort them by name
            if (comp == 0)
                return -Tag.CompareTo(o.Tag);
            return comp;
        }

        /// <summary>
        ///     Increments the tag count by one
        /// </summary>
        public void IncreaseCount()
        {
            if (tagCount < 0) tagCount = 0;
            tagCount++;
        }
    }
}
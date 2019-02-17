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

namespace eDream.program
{
    /// <inheritdoc />
    /// <summary>
    ///     Dream tags are tags -labels- used to sort dreams by type, theme, etc.,
    ///     and to generate statistics based on this
    ///     DreamMainTag represents main tags, which can contain any number of
    ///     child tags that clarify or specify the meaning or scope of the main
    ///     tag
    /// </summary>
    public class DreamMainTag : DreamTag
    {
        public DreamMainTag(string tag) : base(tag)
        {
        }

        public List<DreamChildTag> ChildTags { get; } = new List<DreamChildTag>();

        public void AddChildTag(DreamChildTag childTag)
        {
            if (HasChildTag(childTag)) return;
            ChildTags.Add(childTag);
        }

        private bool HasChildTag(DreamChildTag childTagName)
        {
            return ChildTags.Contains(childTagName);
        }
    }
}
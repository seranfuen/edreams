/****************************************************************************
 * FrmMain: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Ángel Verbo
 ****************************************************************************/
/****************************************************************************
    This file is part of FrmMain.

    FrmMain is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FrmMain is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FrmMain.  If not, see <http://www.gnu.org/licenses/gpl-3.0.html>.]
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using eDream.Annotations;
using EvilTools;

namespace eDream.program
{
    public class DreamMainStatTag : IComparable<DreamMainStatTag>
    {
        private readonly List<DreamChildStatTag> _childTags =
            new List<DreamChildStatTag>();

        public DreamMainStatTag([NotNull] string tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            Tag = StringUtils.CapitalizeString(tag);
        }

        public List<DreamChildStatTag> ChildStatTags
        {
            get
            {
                SortChildTags();
                return _childTags.ToList();
            }
        }

        public string Tag { get; }

        public int TagCount { get; private set; }

        public int CompareTo(DreamMainStatTag o)
        {
            if (o == null) return 0;
            var comp = TagCount.CompareTo(o.TagCount);
            if (comp == 0) return -string.Compare(Tag, o.Tag, StringComparison.Ordinal);
            return comp;
        }


        public void IncreaseChildCount(string childName)
        {
            var childTag = GetChildStatTag(childName);
            if (childTag == null)
            {
                var newTag = new DreamChildStatTag(childName);
                newTag.IncreaseCount();
                _childTags.Add(newTag);
            }
            else
            {
                childTag.IncreaseCount();
            }
        }

        public void IncreaseCount()
        {
            TagCount++;
        }

        private DreamChildStatTag GetChildStatTag(string childTagName)
        {
            return _childTags.FirstOrDefault(tag =>
                string.Equals(tag.Tag, childTagName, StringComparison.OrdinalIgnoreCase));
        }

        private void SortChildTags()
        {
            _childTags.Sort();
            _childTags.Reverse();
        }

        public bool IsTag(string tagName)
        {
            return string.Equals(Tag, tagName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
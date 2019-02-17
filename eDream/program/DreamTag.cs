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
using EvilTools;

namespace eDream.program
{
    /// <summary>
    ///     This class represents a tag without any children or stats methods
    ///     associated with it
    /// </summary>
    public abstract class DreamTag
    {
        protected DreamTag(string tagName)
        {
            Tag = string.IsNullOrWhiteSpace(tagName) ? string.Empty : StringUtils.CapitalizeString(tagName);
        }

        public string Tag { get; }

        protected bool Equals(DreamTag other)
        {
            return string.Equals(Tag, other.Tag, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DreamTag) obj);
        }

        public override int GetHashCode()
        {
            return Tag != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Tag) : 0;
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}
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

namespace eDream.program
{
    /// <inheritdoc />
    /// <summary>
    ///     DreamChildTag objects represent child/subtags belonging to a main tag,
    ///     usually clarifying or specifying what type of dream the main tag
    ///     represents.
    /// </summary>
    public class DreamChildTag : DreamTag
    {
        public DreamChildTag(string tag) : base(tag)
        {
        }
    }
}
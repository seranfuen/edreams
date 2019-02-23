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
using System.Text;

namespace eDream.libs {
    /// <summary>
    /// The class contains the constants that are used for XML operations, such
    /// as the names of the nodes or other formatting options
    /// </summary>
    class XMLConstants {
        public static string root_node  = "FrmMain";
        public static string entry_node = "Entry";
        public static string date_node  = "Date";
        public static string text_node  = "Text";
        public static string tags_node  = "Tags";
        public static int first_level_tab = 2;
        public static int second_level_tab = 4;
    }
}

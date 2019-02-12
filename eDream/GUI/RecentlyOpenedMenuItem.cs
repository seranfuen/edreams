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
using System.Windows.Forms;

namespace eDream.GUI {
    /// <summary>
    /// Menu item that displays a list of recently opened databases
    /// </summary>
    class RecentlyOpenedMenuItem : ToolStripMenuItem {

        // The file path
        private string filePath;
        // The representation of the path, how it is shown in the menu
        private string menuPath;

        /// <summary>
        /// Gets the file path the menu entry represents
        /// </summary>
        public string FilePath {
            get {
                return filePath;
            }
        }

        /// <summary>
        /// Creates a menu item for a given file path to a database
        /// </summary>
        /// <param name="filePath">The actual file path</param>
        /// <param name="fileText">How the path is displayed in the menu</param>
        public RecentlyOpenedMenuItem(string filePath, string fileText) {
            this.filePath = filePath;
            menuPath = fileText;
            this.Text = menuPath;
        }
    }
}

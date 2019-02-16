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
using EvilTools;

namespace eDream.program {
    /// <summary>
    /// This class represents a tag without any children or stats methods 
    /// associated with it
    /// </summary>
    public class DreamTag {
        protected string tagName;
        /// <summary>
        /// The tag name. It is always capitalized (first character set to
        /// upper case)
        /// </summary>
        public string TagName {
            set {
                if (string.IsNullOrWhiteSpace(value)) {
                    value = string.Empty;
                }
                else {
                   value = StringUtils.CapitalizeString(value);
                }
                tagName = value;
            }
            get {
                return tagName;
            }
        }
    }
}

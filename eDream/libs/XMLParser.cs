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
using System.Xml;
using eDream.program;

namespace eDream.libs {
    /// <summary>
    /// This class will try to read a valid eDream XML file and extract the
    /// dream entries
    /// </summary>
    class XMLParser : IDisposable {
        // The XML Reader an XmlTextReader
        XmlTextReader XMLReader;
        /* This flag should be set to flase if something fails parsing the doc
         * If the doc is successfully parsed, it will be true. Another flag
         * should tell whether there were any entries or not
         */
        private bool isValid;

        /// <summary>
        /// Returns false if the parser threw an exception at some point, true
        /// otherwise. This doesn't mean that the list will return any objects
        /// at all
        /// </summary>
        public bool IsValid {
            get {
                return isValid;
            }
        }

        /// <summary>
        /// Check is the XML file contains a) valid XML data  b) 
        /// XMLConstants.root_node open and end tags. Retruns true if it's
        /// considered valid, false if it threw an exception or if couldn't
        /// find the root tags (it could be an XML file, but not an
        /// eDream file. That way prevent data loss by telling the caller
        /// that it can't operate on it)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool IsFileValid(string filename) {
            bool open = false;
            bool end = false;
            try {
                XMLReader = new XmlTextReader(filename);
                while (XMLReader.Read()) {
                    if (XMLReader.NodeType == XmlNodeType.Element &&
                        XMLReader.Name == XMLConstants.root_node && !end) {
                        open = true;
                    }
                    else if (XMLReader.NodeType == XmlNodeType.EndElement &&
                      XMLReader.Name == XMLConstants.root_node && open) {
                        end = open; // only consider it closed if it was opened
                    }
                }
            }
            catch (Exception) {
                open = false;
                end = false;
            }
            finally {
                XMLReader.Close();
            }
            return (open && end);
        }

        /// <summary>
        /// Loads the XML file whose path is contained in filename and
        /// attempts to parse it as a dream database file, so it 
        /// looks for Date, Text and Tags fields for each entry. 
        /// It returns a list of DreamEntry objects. If 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IEnumerable<DreamEntry> LoadFile(string filename) {
            List<DreamEntry> dreamEntries = new List<DreamEntry>();
            try {
                XMLReader = new XmlTextReader(filename);
                string text = string.Empty;
                string date = string.Empty;
                string tags = string.Empty;
                while (XMLReader.Read()) {
                    // If entry node opening, clear text, date and tags
                    if (XMLReader.NodeType == XmlNodeType.Element &&
                    XMLReader.Name == XMLConstants.entry_node) {
                        text = string.Empty;
                        date = string.Empty;
                        tags = string.Empty;
                    }
                    /* Check if text, tag or date nodes are opening and read
                     * their value to the variables
                     */ 
                    else if (XMLReader.Name == XMLConstants.text_node &&
                        XMLReader.NodeType == XmlNodeType.Element) {
                        XMLReader.Read();
                        text = XMLReader.Value;
                    }
                    else if (XMLReader.Name == XMLConstants.tags_node &&
                        XMLReader.NodeType == XmlNodeType.Element) {
                        XMLReader.Read();
                        tags = XMLReader.Value;
                    } else if (XMLReader.Name == XMLConstants.date_node &&
                        XMLReader.NodeType == XmlNodeType.Element) {
                        XMLReader.Read();
                        date = XMLReader.Value;
                    }
                    // When entry node closes, create new dream entry
                    else if (XMLReader.NodeType == XmlNodeType.EndElement &&
                        XMLReader.Name == XMLConstants.entry_node) {
                        dreamEntries.Add(new DreamEntry(DateTime.Parse(date), tags, text));
                    }
                }
                isValid = true;
            }
            catch (Exception e) {
                Console.WriteLine("<<Debug>>\n\n" + e.Message);
                isValid = false;
                dreamEntries = new List<DreamEntry>(); // if exception return empty list
            }
            finally {
                XMLReader.Close();
            }
            return dreamEntries;
        }

        /// <summary>
        /// Delete object
        /// </summary>
        public void Dispose() {
            GC.SuppressFinalize(this);
        }
    }



}
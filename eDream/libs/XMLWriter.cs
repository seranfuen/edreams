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
using System.Xml;
using eDream.program;
using EvilTools;

namespace eDream.libs {
    /// <summary>
    /// This class will write dream entry objects to an XML file
    /// </summary>
    class XMLWriter : IDisposable {

        XmlTextWriter XMLSaver;
        // The encoding will always be UTF-8 (it should always be the standard one!!)
        private Encoding XMLEncoding = Encoding.UTF8;

        /// <summary>
        /// Creates an XML file with only the root nodes
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>True if no exception, false if there was an exception</returns>
        public bool CreateFile(string fileName)     {
            bool valid = true;
            try {
                XMLSaver = new XmlTextWriter(fileName, XMLEncoding);
                CreateHeader();
                CreateEnd();
                XMLSaver.Flush();
            }
            catch (Exception e) {
                System.Console.WriteLine("<<Debug>>\n ** " + e.Message + "\n");
                valid = false;
            }
            finally {
                XMLSaver.Close();
            }
            return valid;
        }

        /// <summary>
        /// Parses the list of dream entries into the XML
        /// </summary>
        /// <param name="fileName">The path to the XML file to create</param>
        /// <param name="dreamList">The dream entries that will be parsed</param>
        /// <returns>True if no exception was thrown, false otherwise</returns>
        public bool ParseEntries(string fileName, List<DreamEntry> dreamList) {
            bool valid = true;
            try {
                XMLSaver = new XmlTextWriter(fileName, XMLEncoding);
                CreateHeader();
                WriteEntryToParser(dreamList);
                CreateEnd();
                XMLSaver.Flush();
            }
            catch (Exception e) {
                System.Console.WriteLine("<<Debug>>\n ** " + e.Message + "\n");
                valid = false;
            }
            finally {
                XMLSaver.Close();
            }
            return valid;
        }

        /// <summary>
        /// Write the dream list to the XMLSave object
        /// </summary>
        /// <param name="dreamList"></param>
        private void WriteEntryToParser(List<DreamEntry> dreamList) {
            if (XMLSaver == null || dreamList == null) {
                throw new Exception("Null XMLSaver object or null tagList");
            }
            for (int i = 0; i < dreamList.Count; i++) {
                if (dreamList[i].ToDelete) { // If set to delete, do not process
                    continue;
                }
                XMLSaver.WriteWhitespace(StringUtils.GenerateSpaces(XMLConstants.first_level_tab));
                XMLSaver.WriteStartElement(XMLConstants.entry_node);
                XMLSaver.WriteWhitespace("\n" +
                    StringUtils.GenerateSpaces(XMLConstants.second_level_tab));
                XMLSaver.WriteElementString(XMLConstants.date_node,
                    dreamList[i].GetDateAsStr());
                XMLSaver.WriteWhitespace("\n" +
                    StringUtils.GenerateSpaces(XMLConstants.second_level_tab));
                XMLSaver.WriteElementString(XMLConstants.text_node,
                    dreamList[i].Text);
                XMLSaver.WriteWhitespace("\n" +
                    StringUtils.GenerateSpaces(XMLConstants.second_level_tab));
                XMLSaver.WriteElementString(XMLConstants.tags_node,
                    dreamList[i].GetTagsAsString());
                XMLSaver.WriteWhitespace("\n" +
                    StringUtils.GenerateSpaces(XMLConstants.first_level_tab));
                XMLSaver.WriteEndElement();
                XMLSaver.WriteWhitespace("\n");
            }
        }

        /// <summary>
        /// Write the header and the initial node to the file
        /// </summary>
        private void CreateHeader() {
            if (XMLSaver == null) {
                throw new Exception("Null XMLSaver object");
            }
            XMLSaver.WriteStartDocument();
            XMLSaver.WriteWhitespace("\n");
            XMLSaver.WriteStartElement(XMLConstants.root_node);
            XMLSaver.WriteWhitespace("\n");
        }

        /// <summary>
        /// Close the root tag
        /// </summary>
        private void CreateEnd() {
            if (XMLSaver == null) {
                throw new Exception("Null XMLSaver object");
            }
            XMLSaver.WriteEndElement();
            XMLSaver.WriteEndDocument();
        }

        /// <summary>
        /// Delete object
        /// </summary>
        public void Dispose() {
            GC.SuppressFinalize(this);
        }
    }
}
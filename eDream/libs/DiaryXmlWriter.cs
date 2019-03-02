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
using eDream.Annotations;
using eDream.program;
using EvilTools;

namespace eDream.libs
{
    public class DiaryXmlWriter : IDisposable
    {
        // The encoding will always be UTF-8 (it should always be the standard one!!)
        private readonly Encoding _xmlEncoding = Encoding.UTF8;

        private XmlTextWriter _xmlTextWriter;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool CreateFile(string fileName)
        {
            var valid = true;
            try
            {
                _xmlTextWriter = new XmlTextWriter(fileName, _xmlEncoding);
                CreateHeader();
                CreateEnd();
                _xmlTextWriter.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("<<Debug>>\n ** " + e.Message + "\n");
                valid = false;
            }
            finally
            {
                _xmlTextWriter.Close();
            }

            return valid;
        }

        public bool WriteEntriesToFile([NotNull] string fileName, [NotNull] IEnumerable<DreamEntry> dreamList)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (dreamList == null) throw new ArgumentNullException(nameof(dreamList));
            var valid = true;
            try
            {
                _xmlTextWriter = new XmlTextWriter(fileName, _xmlEncoding);
                CreateHeader();
                WriteEntryToParser(dreamList);
                CreateEnd();
                _xmlTextWriter.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("<<Debug>>\n ** " + e.Message + "\n");
                valid = false;
            }
            finally
            {
                _xmlTextWriter.Close();
            }

            return valid;
        }

        private void CreateEnd()
        {
            _xmlTextWriter.WriteEndElement();
            _xmlTextWriter.WriteEndDocument();
        }

        private void CreateHeader()
        {
            _xmlTextWriter.WriteStartDocument();
            _xmlTextWriter.WriteWhitespace("\n");
            _xmlTextWriter.WriteStartElement(XMLConstants.root_node);
            _xmlTextWriter.WriteWhitespace("\n");
        }

        private void WriteEntryToParser(IEnumerable<DreamEntry> dreamList)
        {
            foreach (var entry in dreamList)
            {
                _xmlTextWriter.WriteWhitespace(StringUtils.GenerateSpaces(XMLConstants.first_level_tab));
                _xmlTextWriter.WriteStartElement(XMLConstants.entry_node);
                _xmlTextWriter.WriteWhitespace("\n" +
                                               StringUtils.GenerateSpaces(XMLConstants.second_level_tab));
                _xmlTextWriter.WriteElementString(XMLConstants.date_node,
                    entry.GetDateAsStr());
                _xmlTextWriter.WriteWhitespace("\n" +
                                               StringUtils.GenerateSpaces(XMLConstants.second_level_tab));
                _xmlTextWriter.WriteElementString(XMLConstants.text_node,
                    entry.Text);
                _xmlTextWriter.WriteWhitespace("\n" +
                                               StringUtils.GenerateSpaces(XMLConstants.second_level_tab));
                _xmlTextWriter.WriteElementString(XMLConstants.tags_node,
                    entry.GetTagString());
                _xmlTextWriter.WriteWhitespace("\n" +
                                               StringUtils.GenerateSpaces(XMLConstants.first_level_tab));
                _xmlTextWriter.WriteEndElement();
                _xmlTextWriter.WriteWhitespace("\n");
            }
        }
    }
}
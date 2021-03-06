﻿/****************************************************************************
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
using System.Xml;
using eDream.program;

namespace eDream.libs
{
    internal class DiaryReader : IDiaryReader
    {
        private XmlTextReader _xmlTextReader;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool IsFileValid(string filename)
        {
            var open = false;
            var end = false;
            try
            {
                _xmlTextReader = new XmlTextReader(filename);
                while (_xmlTextReader.Read())
                    if (_xmlTextReader.NodeType == XmlNodeType.Element &&
                        _xmlTextReader.Name == XmlConstants.RootNode && !end)
                        open = true;
                    else if (_xmlTextReader.NodeType == XmlNodeType.EndElement &&
                             _xmlTextReader.Name == XmlConstants.RootNode && open)
                        end = true; // only consider it closed if it was opened
            }
            catch (Exception)
            {
                open = false;
                end = false;
            }
            finally
            {
                _xmlTextReader.Close();
            }

            return open && end;
        }

        public IEnumerable<DreamEntry> LoadFile(string filename)
        {
            var dreamEntries = new List<DreamEntry>();
            try
            {
                _xmlTextReader = new XmlTextReader(filename);
                var text = string.Empty;
                var date = string.Empty;
                var tags = string.Empty;
                while (_xmlTextReader.Read())
                    if (_xmlTextReader.NodeType == XmlNodeType.Element &&
                        _xmlTextReader.Name == XmlConstants.EntryNode)
                    {
                        text = string.Empty;
                        date = string.Empty;
                        tags = string.Empty;
                    }

                    else if (_xmlTextReader.Name == XmlConstants.TextNode &&
                             _xmlTextReader.NodeType == XmlNodeType.Element)
                    {
                        _xmlTextReader.Read();
                        text = _xmlTextReader.Value;
                    }
                    else if (_xmlTextReader.Name == XmlConstants.TagsNode &&
                             _xmlTextReader.NodeType == XmlNodeType.Element)
                    {
                        _xmlTextReader.Read();
                        tags = _xmlTextReader.Value;
                    }
                    else if (_xmlTextReader.Name == XmlConstants.DateNode &&
                             _xmlTextReader.NodeType == XmlNodeType.Element)
                    {
                        _xmlTextReader.Read();
                        date = _xmlTextReader.Value;
                    }

                    else if (_xmlTextReader.NodeType == XmlNodeType.EndElement &&
                             _xmlTextReader.Name == XmlConstants.EntryNode)
                    {
                        dreamEntries.Add(new DreamEntry(DateTime.Parse(date), tags, text));
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine("<<Debug>>\n\n" + e.Message);
                dreamEntries = new List<DreamEntry>();
            }
            finally
            {
                _xmlTextReader.Close();
            }

            return dreamEntries;
        }
    }
}
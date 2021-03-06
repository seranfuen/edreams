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
using System.Linq;
using System.Text.RegularExpressions;
using eDream.libs;

namespace eDream.program
{
    /// <summary>
    ///     DreamEntry is a class that represents an individual dream entry,
    ///     that is, it contains its date, a summary or text about it,
    ///     and any tags used to sort dreams by common topics and generate
    ///     statistics. It corresponds with each group of data enclosed by
    ///     <entry></entry> in the XML storage
    /// </summary>
    public class DreamEntry
    {
        private bool _isTextValid;
        private IList<DreamMainTag> _tags;

        private string _text;

        private bool _toDelete;

        public DreamEntry(DateTime date, string tags, string text)
        {
            if (string.IsNullOrWhiteSpace(tags) && string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException();
            Text = text ?? "";
            Date = date.Date;
            SetTags(tags ?? "");
        }

        public DateTime Date { get; set; }

        public string Text
        {
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _text = value.Trim();
                _isTextValid = !string.IsNullOrWhiteSpace(_text);
                CountTextWords();
            }
            get => _text;
        }

        public int NumberOfWords { get; private set; }

        public bool ToDelete
        {
            set => _toDelete = value;
            get => _toDelete || !IsValid;
        }

        public bool IsValid => _isTextValid && !_toDelete;

        public bool ContainsTagOrChildTag(string tagName)
        {
            return GetTagsAsList().Any(tag => string.Equals(tagName, tag.Tag, StringComparison.OrdinalIgnoreCase)
                                              || tag.ChildTags.Any(childTag => string.Equals(childTag.Tag, tagName,
                                                  StringComparison.OrdinalIgnoreCase)));
        }

        public bool DreamTextContains(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && Regex.IsMatch(_text, value.Trim(),
                       RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        public string GetDateAsDayStr()
        {
            return Date.ToString("dd-MM-yyyy");
        }


        public string GetDateAsStr()
        {
            return Date.ToString("yyyy-MM-dd");
        }

        public IList<DreamMainTag> GetTagsAsList()
        {
            return _tags ?? new List<DreamMainTag>();
        }

        public string GetTagString()
        {
            return DreamTagParser.TagsToString(_tags);
        }

        public void SetTags(string tags)
        {
            _tags = DreamTagParser.ParseStringToDreamTags(tags);
        }

        private void CountTextWords()
        {
            if (!_isTextValid)
            {
                NumberOfWords = 0;
                return;
            }

            NumberOfWords = Regex.Split(_text, @"\s+").Length;
        }
    }
}
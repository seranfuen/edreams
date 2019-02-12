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
using System.Text.RegularExpressions;
using eDream.libs;
using EvilTools;

namespace eDream.program
{
    /// <summary>
    /// DreamEntry is a class that represents an individual dream entry, 
    /// that is, it contains its date, a summary or text about it, 
    /// and any tags used to sort dreams by common topics and generate 
    /// statistics. It corresponds with each group of data enclosed by 
    /// <entry></entry> in the XML storage
    /// </summary>
    class DreamEntry
    {
        // The date, text and tags for the entry
        private string text;

        private List<DreamMainTag> tags;

        private DateTime date;

        // Tag parser to convert tag objects to string and vice versa
        private DreamTagParser tagParser = new DreamTagParser();

        /* Flag that signals if the date could be parsed from its string
         * representation */
        private bool isDateValid = false;

        /* This flag signals whether the text is considered valid.
         * To be precise, we will not accept null/whitespace only text */
        private bool isTextValid = false;

        /* An entry that is flagged to be deleted will not be written to the
         * XML file by the module XMLWriter, 
         * and GetIfValid() will return false, so it becames a disabled entry */ 
        private bool toDelete = false;

        /* The number of words the entry text has */
        private int wordNumber;

        /// <summary>
        /// Sets or gets the entry text
        /// </summary>
        public string Text {
            set {
                this.text = value.Trim();
                isTextValid = !(String.IsNullOrWhiteSpace(this.text));
                CountTextWords();
            }
            get {
                return text;
            }
        }

        /// <summary>
        /// Returns the number of words the entry text has
        /// </summary>
        public int WordNumber {
            get {
                return wordNumber;
            }
        }

        /// <summary>
        /// Get or set the flag that signals the entry as a candidate 
        /// to be deleted, or not to be processed by the XML parser. Notice
        /// that it will return true, even if the toDelete flag was set to
        /// false, if the entry is not valid (could not parse the date or the
        /// text is null or only whitespace)
        /// </summary>
        public bool ToDelete {
            set {
                toDelete = value;
            }
            get {
                return (toDelete || !GetIfValid());
            }
        }

        public DreamEntry(DateTime date, List<DreamMainTag> tags, string text)
        {
            SetDate(date);
            Text = text;
            SetTags(tags);
        }

        public DreamEntry(string date, List<DreamMainTag> tags, string text)
        {
            SetDate(date);
            Text = text;
            SetTags(tags);
        }

        public DreamEntry(DateTime date, string tags, string text)
        {
            SetDate(date);
            Text = text;
            SetTags(tags);
        }

        public DreamEntry(string date, string tags, string text)
        {
            SetDate(date);
            Text = text;
            SetTags(tags);
        }

        /// <summary>
        /// Set the entry date to the DateTime object provided
        /// </summary>
        /// <param name="date">The date the dream entry will be set to</param>
        public void SetDate(DateTime date) {
            this.date = date;
            isDateValid = true;
        }

        /// <summary>
        /// Set the entry date to the string representation of a date provided.
        /// If the string could not be parsed, the isDateValid flag will be set
        /// to false and the entry will become disabled.
        /// DateTime.Parse() is used so any date that can be parsed by this 
        /// method will be valid, so see documentation:
        /// (http://msdn.microsoft.com/en-us/library/1k1skd40.aspx)
        /// </summary>
        /// <param name="date">A string representation of a date</param>
        public void SetDate(string date) {
            try {
                this.date = DateTime.Parse(date);
            } catch (FormatException) {
                this.date = DateTime.MinValue;
                isDateValid = false;
                return;
            }
            isDateValid = true;
        }

        /// <summary>
        /// Returns the current DateTime object representing the entry date.
        /// If there was an error parsing the date, it will return 
        /// DateTime.MinValue
        /// </summary>
        /// <returns></returns>
        public DateTime GetDate() {
            return date;
        }

        
        /// <summary>
        /// Returns the string representation of the date with a format of
        /// YYYY-MM-DD (this is how the date is stored in the XML file)
        /// </summary>
        /// <returns></returns>
        public string GetDateAsStr() {
            /* The way the Japanese or the Chinese normally represent dates. 
               Far superior to and more logical than European, 
               not to mention US style!! */
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Returns the string representation of the date with a format of
        /// dd-MM-yyyy (this is how the date is shown in the application,
        /// following European convention)
        /// </summary>
        /// <returns></returns>
        public string GetDateAsDayStr() {
            return date.ToString("dd-MM-yyyy");
        }

        /// <summary>
        /// Sets the entry tags with the list of DreamMainTag objects provided
        /// </summary>
        /// <param name="tags"></param>
        public void SetTags(List<DreamMainTag> tags) {
            this.tags = tags;
        }

        /// <summary>
        /// Set the entry tags from the string representation given, which will
        /// be parsed
        /// </summary>
        /// <param name="tags"></param>
        public void SetTags(string tags) {
            this.tags = tagParser.StringToTags(tags);
        }

        /// <summary>
        /// Returns the entry tags as a list of DreamMainTag objects
        /// </summary>
        /// <returns></returns>
        public List<DreamMainTag> GetTagsAsList() {
            return this.tags != null ? this.tags : new List<DreamMainTag>();
        }

        /// <summary>
        /// Returns the entry tags as a string representation
        /// </summary>
        /// <returns></returns>
        public string GetTagsAsString() {
            return tagParser.TagsToString(tags);
        }

        /// <summary>
        /// Returns the validity value of the entry. It is considered invalid
        /// if the text or date were invalid data or if the entry has been set
        /// to delete so it is considered inactive
        /// </summary>
        /// <returns></returns>
        public bool GetIfValid() {
            return (isDateValid && isTextValid && !toDelete);
        }

        /// <summary>
        /// Searches the entry text and returns true if the value given was
        /// found, false otherwise
        /// </summary>
        /// <param name="value">The string that is beign searched for</param>
        /// <param name="caseSensitive">True if case sensitive, false
        /// otherwise</param>
        /// <returns>True if the text was found in the entry, false otherwise
        /// </returns>
        public bool SearchTextFor(string value, bool caseSensitive) {
            if (String.IsNullOrWhiteSpace(value) || !isTextValid) {
                return false;
            }
            value = value.Trim();
            string pattern = @"\s+" + value + @"\s+";
            if (!caseSensitive) {
                return Regex.IsMatch(text, pattern);
            }
            return Regex.IsMatch(text, pattern, 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
          }

        /// <summary>
        /// Counts the number of words in the text
        /// </summary>
        private void CountTextWords() {
            if (!isTextValid) {
                wordNumber = 0;
                return;
            }
            wordNumber = (Regex.Split(text, @"\s+")).Length;
        }
    }
}
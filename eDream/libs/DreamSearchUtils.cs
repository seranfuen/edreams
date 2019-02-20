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
using EvilTools;
using System.Text;
using eDream.program;

namespace eDream.libs {
    /// <summary>
    /// This class contains methods to search a list of dream entries and return
    /// a list with the entries that matched what we were searching for
    /// </summary>
    class DreamSearchUtils {

        public enum TagSearchType {
            ANDSearch,
            ORSearch
        }

        /// <summary>
        /// Returns a list of dream entry object whose text contains the
        /// string searchFor
        /// </summary>
        /// <param name="entries">The entries to search</param>
        /// <param name="searchFor">The string to search in the text of the
        /// entries</param>
        /// <returns></returns>
        public List<DreamEntry> SearchEntriesText(List<DreamEntry> entries,
            string searchFor) {
                var results = new List<DreamEntry>();
                for (var i = 0; i < entries.Count; i++) {
                    if (entries[i].DreamTextContains(searchFor)) {
                        results.Add(entries[i]);
                    }
                }
                return results;
        }

        /// <summary>
        /// Performs a tag search and returns a list with the dream entries
        /// that match the results
        /// </summary>
        /// <param name="entries">The entries that are being searched</param>
        /// <param name="searchFor">The targs to search for</param>
        /// <param name="childTags">True if child tags are also searched</param>
        /// <param name="type">One of the class constants for the search type,
        /// either ANDsearch or ORsearch</param>
        /// <returns></returns>
        public List<DreamEntry> SearchEntriesTags(List<DreamEntry> entries,
            string[] searchFor, bool childTags, TagSearchType type) {
                CleanString(searchFor);
                if (type == TagSearchType.ANDSearch) {
                    return DoAndSearch(entries, searchFor, childTags);
                }
                else {
                    return DoOrSearch(entries, searchFor, childTags);
                }
        }

        /// <summary>
        /// Returns a list of all entries that are found within the range
        /// of dates from-to, both inclusive
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<DreamEntry>SearchEntriesDateRange(List<DreamEntry> entries,
            DateTime from, DateTime to) {
            var results = new List<DreamEntry>();
            var sorter = new DateRangeSort(from, to);
            for (var i = 0; i < entries.Count; i++) {
                if (sorter.EvaluateDate(entries[i].Date)) {
                    results.Add(entries[i]);
                }
            }
            return results;
        }

        /// <summary>
        /// Returns a list of all entries that match the date given
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<DreamEntry>SearchEntriesOnDate(List<DreamEntry> entries,
            DateTime day) {
            var results = new List<DreamEntry>();
            var sorter = new DateRangeSort(day);
            for (var i = 0; i < entries.Count; i++) {
                if (sorter.EvaluateDate(entries[i].Date)) {
                    results.Add(entries[i]);
                }
            }
            return results;
        }

        /// <summary>
        /// Returns a deep cloned string array so that we can modify it
        /// when performing AND search (we delete tags already found) without
        /// affecting other searches
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string[] CloneString(string[] str) {
            var result = new string[str.Length];
            for (var i = 0; i < str.Length; i++) {
                result[i] = (string)str[i].Clone();
            }
            return result;
        }


        /// <summary>
        /// Trims each element in the string array
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private void CleanString(string[] str) {
            for (var i = 0; i < str.Length; i++) {
                str[i] = str[i].Trim();
            }
        }

        /// <summary>
        /// Performs an AND search to the tags of the dream entries in a list:
        /// when all the tags are found in a dream entry, it's added to the
        /// results
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="tags"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private List<DreamEntry> DoAndSearch(List<DreamEntry> entries,
            string[] tags, bool includeChildTags) {
                var results = new List<DreamEntry>();
                for (var i = 0; i < entries.Count; i++) {
                    if (MatchAndSearch(entries[i], tags, includeChildTags)) {
                        results.Add(entries[i]);
                    }
                }
                return results;
        }

        /// <summary>
        /// Performs an OR search to the tags of the dream entires in a list:
        /// as soon as one of the tags being searched for is found, the entry
        /// is added to the results
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="tags"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private List<DreamEntry> DoOrSearch(List<DreamEntry> entries,
            string[] tags, bool includeChildTags) {
                var results = new List<DreamEntry>();
                for (var i = 0; i < entries.Count; i++) {
                    if (MatchOrSearch(entries[i], tags, includeChildTags)) {
                        results.Add(entries[i]);
                    }
                }
                return results;
        }

        /// <summary>
        /// Searches the entry's tags and returns true if all the tags that 
        /// were being searched
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="tag"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private bool MatchAndSearch(DreamEntry entry, string[] tags,
            bool includeChildTags) {
                var tag = CloneString(tags);
                var mainTags = entry.GetTagsAsList();
                for (var i = 0; i < mainTags.Count; i++) {
                    // Find if among each main tag a tag in the list is found
                    for (var j = 0; j < tag.Length; j++) {
                        if (string.IsNullOrWhiteSpace(tag[j])) {
                            continue;
                        }
                        if (string.Compare(tag[j], mainTags[i].Tag, true) 
                            == 0) {
                                tag[j] = string.Empty;
                        }
                    }
                    // If child tags also included, search them
                    if (includeChildTags) {
                        var childTags = mainTags[i].ChildTags;
                        for (var j = 0; j < childTags.Count; j++) {
                            for (var k = 0; k < tag.Length; k++) {
                                if (string.IsNullOrWhiteSpace(tag[k])) {
                                    continue;
                                }
                                if (string.Compare(tag[k], childTags[j].Tag,
                                    true) == 0) {
                                        tag[k] = string.Empty;
                                }
                            }
                        }
                    }
                    /* Check if there are still unfound tags. If so, continue
                     * search, otherwise return true
                     */
                    if (ToFind(tag) == 0) {
                        return true;
                    }
                }
                return false;                
        }

        /// <summary>
        /// Searches the entry's tags and returns true as soon as one match is
        /// found, false otherwise
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="tags"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private bool MatchOrSearch(DreamEntry entry, string[] tags,
            bool includeChildTags) {
                var mainTags = entry.GetTagsAsList();
                for (var i = 0; i < mainTags.Count; i++) {
                    for (var j = 0; j < tags.Length; j++) {
                        if (string.Compare(tags[j], mainTags[i].Tag,
                            true) == 0) {
                                return true;
                        }
                    }
                    // Search child tags if searching for them
                    if (includeChildTags) {
                        var childTags = mainTags[i].ChildTags;
                        for (var j = 0; j < childTags.Count; j++) {
                            for (var k = 0; k < tags.Length; k++) {
                                if (string.Compare(tags[k], childTags[j].Tag,
                                    true) == 0) {
                                        return true;
                                }
                            }
                        }
                    }
                }
                return false;
        }
   

        /// <summary>
        /// Returns the number of tags in the string that haven't been found
        /// yet. A tag that is found is replaced in the string by String.Empty,
        /// so this method returns the number of strings that are not empty
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private int ToFind(string[] tags) {
            var counter = 0;
            for (var i = 0; i < tags.Length; i++) {
                if (!string.IsNullOrWhiteSpace(tags[i])) {
                    counter++;
                }
            }
            return counter;
        }

        
    }
}

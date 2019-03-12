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
using System.Linq;
using System.Text.RegularExpressions;
using eDream.Annotations;
using eDream.program;

namespace eDream.libs
{
    public class DreamDiarySearch
    {
        private const string QuotedSearchExpressionRegexPattern = "\"[^\\\"]*\"";
        public const string SeparateWordsInSearchExpressionRegexPattern = "\\w+";

        private readonly IEnumerable<DreamEntry> _dreamEntries;

        public DreamDiarySearch([NotNull] IEnumerable<DreamEntry> dreamEntries)
        {
            _dreamEntries = dreamEntries ?? throw new ArgumentNullException(nameof(dreamEntries));
        }

        public IEnumerable<DreamEntry> SearchEntriesBetweenDates(DateTime from, DateTime to)
        {
            return _dreamEntries.Where(entry => entry.Date.Date >= from.Date && entry.Date.Date <= to.Date);
        }

        public List<DreamEntry> SearchEntriesByText(string searchFor)
        {
            return _dreamEntries.Where(entry => ContainsSearchValue(entry, searchFor)).ToList();
        }

        public IEnumerable<DreamEntry> SearchEntriesOnDate(DateTime day)
        {
            return _dreamEntries.Where(entry => entry.Date.Date.Equals(day.Date));
        }

        /// <summary>
        ///     Performs a tag search and returns a list with the dream entries
        ///     that match the results
        /// </summary>
        /// <param name="entries">The entries that are being searched</param>
        /// <param name="searchFor">The targs to search for</param>
        /// <param name="childTags">True if child tags are also searched</param>
        /// <param name="type">
        ///     One of the class constants for the search type,
        ///     either ANDsearch or ORsearch
        /// </param>
        /// <returns></returns>
        public IEnumerable<DreamEntry> SearchEntriesTags(IEnumerable<DreamEntry> entries,
            string[] searchFor, bool childTags, TagSearchType type)
        {
            CleanString(searchFor);
            return type == TagSearchType.AndSearch
                ? DoAndSearch(entries, searchFor, childTags)
                : DoOrSearch(entries, searchFor, childTags);
        }


        /// <summary>
        ///     Trims each element in the string array
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private void CleanString(string[] str)
        {
            for (var i = 0; i < str.Length; i++) str[i] = str[i].Trim();
        }

        /// <summary>
        ///     Returns a deep cloned string array so that we can modify it
        ///     when performing AND search (we delete tags already found) without
        ///     affecting other searches
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string[] CloneString(string[] str)
        {
            var result = new string[str.Length];
            for (var i = 0; i < str.Length; i++) result[i] = (string) str[i].Clone();
            return result;
        }

        private static bool ContainsSearchValue(DreamEntry entry, string searchFor)
        {
            var literalWords = GetQuotedExpressionsForLiteralSearch(searchFor);
            var searchForWithoutQuotedExpressions = RemoveQuotedExpressions(searchFor, literalWords);
            var unquotedWords = GetUnquotedWords(searchForWithoutQuotedExpressions);

            return literalWords.Any(entry.DreamTextContains) || unquotedWords.Any(entry.DreamTextContains);
        }

        /// <summary>
        ///     Performs an AND search to the tags of the dream entries in a list:
        ///     when all the tags are found in a dream entry, it's added to the
        ///     results
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="tags"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private List<DreamEntry> DoAndSearch(IEnumerable<DreamEntry> entries,
            string[] tags, bool includeChildTags)
        {
            var results = new List<DreamEntry>();
            foreach (var entry in entries)
                if (MatchAndSearch(entry, tags, includeChildTags))
                    results.Add(entry);
            return results;
        }

        /// <summary>
        ///     Performs an OR search to the tags of the dream entires in a list:
        ///     as soon as one of the tags being searched for is found, the entry
        ///     is added to the results
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="tags"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private List<DreamEntry> DoOrSearch(IEnumerable<DreamEntry> entries,
            string[] tags, bool includeChildTags)
        {
            var results = new List<DreamEntry>();
            foreach (var entry in entries)
                if (MatchOrSearch(entry, tags, includeChildTags))
                    results.Add(entry);
            return results;
        }

        private static List<string> GetQuotedExpressionsForLiteralSearch(string searchFor)
        {
            var quotedWordsRegex = new Regex(QuotedSearchExpressionRegexPattern,
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var literalWords = new List<string>();
            foreach (var match in quotedWordsRegex.Matches(searchFor))
                literalWords.Add(match.ToString().Replace("\"", "").Trim());
            return literalWords;
        }

        private static IEnumerable<string> GetUnquotedWords(string searchForWithoutQuotedExpressions)
        {
            var unquotedWords = new List<string>();
            var separateWordsRegex =
                new Regex(SeparateWordsInSearchExpressionRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var match in separateWordsRegex.Matches(searchForWithoutQuotedExpressions))
                unquotedWords.Add(match.ToString().Trim());
            return unquotedWords;
        }

        /// <summary>
        ///     Searches the entry's tags and returns true if all the tags that
        ///     were being searched
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="tag"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private bool MatchAndSearch(DreamEntry entry, string[] tags,
            bool includeChildTags)
        {
            var tag = CloneString(tags);
            var mainTags = entry.GetTagsAsList();
            for (var i = 0; i < mainTags.Count; i++)
            {
                // Find if among each main tag a tag in the list is found
                for (var j = 0; j < tag.Length; j++)
                {
                    if (string.IsNullOrWhiteSpace(tag[j])) continue;
                    if (string.Compare(tag[j], mainTags[i].Tag, true)
                        == 0)
                        tag[j] = string.Empty;
                }

                // If child tags also included, search them
                if (includeChildTags)
                {
                    var childTags = mainTags[i].ChildTags;
                    for (var j = 0; j < childTags.Count; j++)
                    for (var k = 0; k < tag.Length; k++)
                    {
                        if (string.IsNullOrWhiteSpace(tag[k])) continue;
                        if (string.Compare(tag[k], childTags[j].Tag,
                                true) == 0)
                            tag[k] = string.Empty;
                    }
                }

                /* Check if there are still unfound tags. If so, continue
                 * search, otherwise return true
                 */
                if (ToFind(tag) == 0) return true;
            }

            return false;
        }

        /// <summary>
        ///     Searches the entry's tags and returns true as soon as one match is
        ///     found, false otherwise
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="tags"></param>
        /// <param name="includeChildTags"></param>
        /// <returns></returns>
        private bool MatchOrSearch(DreamEntry entry, string[] tags,
            bool includeChildTags)
        {
            var mainTags = entry.GetTagsAsList();
            for (var i = 0; i < mainTags.Count; i++)
            {
                for (var j = 0; j < tags.Length; j++)
                    if (string.Compare(tags[j], mainTags[i].Tag,
                            true) == 0)
                        return true;
                // Search child tags if searching for them
                if (includeChildTags)
                {
                    var childTags = mainTags[i].ChildTags;
                    for (var j = 0; j < childTags.Count; j++)
                    for (var k = 0; k < tags.Length; k++)
                        if (string.Compare(tags[k], childTags[j].Tag,
                                true) == 0)
                            return true;
                }
            }

            return false;
        }

        private static string RemoveQuotedExpressions(string searchFor, IEnumerable<string> literalWords)
        {
            foreach (var literalWord in literalWords) searchFor = searchFor.Replace(literalWord, "");
            return searchFor;
        }


        /// <summary>
        ///     Returns the number of tags in the string that haven't been found
        ///     yet. A tag that is found is replaced in the string by String.Empty,
        ///     so this method returns the number of strings that are not empty
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private int ToFind(string[] tags)
        {
            var counter = 0;
            for (var i = 0; i < tags.Length; i++)
                if (!string.IsNullOrWhiteSpace(tags[i]))
                    counter++;
            return counter;
        }
    }
}
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

        public List<DreamEntry> SearchEntriesForAllTags([NotNull] IEnumerable<string> tags)
        {
            if (tags == null) throw new ArgumentNullException(nameof(tags));
            var tagList = tags.ToList();
            if (!tagList.Any() || tagList.All(string.IsNullOrWhiteSpace)) return new List<DreamEntry>();
            return _dreamEntries.Where(entry =>
                DreamContainsAllTags(entry, tagList.Where(tag => !string.IsNullOrWhiteSpace(tag)))).ToList();
        }

        public IList<DreamEntry> SearchEntriesForAnyTag(IEnumerable<string> tags)
        {
            if (tags == null) throw new ArgumentNullException(nameof(tags));
            var tagList = tags.ToList();
            if (!tagList.Any() || tagList.All(string.IsNullOrWhiteSpace)) return new List<DreamEntry>();
            return _dreamEntries.Where(entry =>
                DreamContainsAnyTag(entry, tagList.Where(tag => !string.IsNullOrWhiteSpace(tag)))).ToList();
        }

        public IEnumerable<DreamEntry> SearchEntriesOnDate(DateTime day)
        {
            return _dreamEntries.Where(entry => entry.Date.Date.Equals(day.Date));
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

        private static bool DreamContainsAllTags(DreamEntry entry, IEnumerable<string> tags)
        {
            return tags.All(tag => entry.ContainsTagOrChildTag(tag.Trim()));
        }

        private static bool DreamContainsAnyTag(DreamEntry entry, IEnumerable<string> tags)
        {
            return tags.Any(tag => entry.ContainsTagOrChildTag(tag.Trim()));
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

        private static string RemoveQuotedExpressions(string searchFor, IEnumerable<string> literalWords)
        {
            return literalWords.Aggregate(searchFor, (current, literalWord) => current.Replace(literalWord, ""));
        }
    }
}
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
using eDream.program;

namespace eDream.libs
{
    /// <summary>
    ///     Collection of static methods to parse tags from a string to tag objects
    ///     and viceversa
    /// </summary>
    public static class DreamTagParser
    {
        /// <summary>
        ///     Converts a list of DreamMainTag objects to a string showing them
        ///     as a list in the format tag1,tag2(childTag1;childTag2)
        /// </summary>
        /// <param name="tagList">a list of DreamMainTag objects</param>
        /// <returns>A string showing the tags as a list</returns>
        public static string TagsToString(List<DreamMainTag> tagList)
        {
            if (tagList == null || tagList.Count == 0) return string.Empty;
            var mainTagsStr = new string[tagList.Count];
            string[] childTagsStr;
            for (var i = 0; i < tagList.Count; i++)
            {
                mainTagsStr[i] = tagList[i].Tag;
                /* If there are child tags, append them to the main tag string */
                if (tagList[i].ChildTags.Count <= 0) continue;

                childTagsStr = new string[tagList[i].ChildTags.Count];
                for (var j = 0; j < tagList[i].ChildTags.Count; j++)
                    if (!string.IsNullOrEmpty(tagList[i].ChildTags[j].Tag))
                        childTagsStr[j] = tagList[i].ChildTags[j].Tag;
                mainTagsStr[i] = mainTagsStr[i] + " " + DreamTagTokens.OpenChildTags +
                                 string.Join(char.ToString(DreamTagTokens.MainTagSeparator) + " ",
                                     childTagsStr) + DreamTagTokens.CloseChildTags;
            }

            return string.Join(char.ToString(DreamTagTokens.MainTagSeparator) + " ",
                mainTagsStr);
        }

        /// <summary>
        ///     Converts a string representation of tags and child tags to a list
        ///     of dream main tag objects
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<DreamMainTag> ParseStringToDreamTags(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var tagList = new List<DreamMainTag>();
            var mainTags = ExtractMainTags(str.Trim());

            foreach (var tag in mainTags)
            {
                var childStart = tag.IndexOf(DreamTagTokens.OpenChildTags);
                if (childStart == -1) childStart = tag.Length;
                var mainTag = tag.Substring(0, childStart);
                if (string.IsNullOrWhiteSpace(mainTag)) continue;

                var newTag =
                    new DreamMainTag(mainTag);
                if (childStart < tag.Length)
                {
                    var childTags =
                        ExtractChildTags(tag.Substring(childStart,
                            tag.Length - childStart));
                    foreach (var childTag in childTags)
                        newTag.AddChildTag(new DreamChildTag(childTag));
                }

                tagList.Add(newTag);
            }

            return tagList;
        }

        private static IEnumerable<string> ExtractMainTags(string stringTagToExtract)
        {
            return new DreamTagStringExtractor(stringTagToExtract).Tags;
        }

        /// <summary>
        ///     Returns a list with the child tags of a main tag
        /// </summary>
        /// <param name="str"></param>
        /// <returns>
        ///     A string list with the child tags or an empty one
        ///     if there was an error
        /// </returns>
        private static IEnumerable<string> ExtractChildTags(string str)
        {
            var startPosition = str.IndexOf(DreamTagTokens.OpenChildTags);
            var endPosition = str.IndexOf(DreamTagTokens.CloseChildTags);
            if (endPosition == -1) endPosition = str.Length;
            if (startPosition == -1 || !(startPosition < str.Length)) return new string[0];
            var strings =
                str.Substring(startPosition + 1, endPosition -
                                                 startPosition - 1).Split(DreamTagTokens.MainTagSeparator);
            for (var i = 0; i < strings.Length; i++) strings[i] = strings[i].Trim();
            return strings;
        }
    }
}
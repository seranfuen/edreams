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
using System.Text;
using eDream.program;

namespace eDream.libs {
    
    /// <summary>
    /// Collection of static methods to parse tags from a string to tag objects
    /// and viceversa
    /// </summary>
    class DreamTagParser {

        /// <summary>
        /// Characters used to separate tags and child tags
        /// </summary>
        public static char mainTagSeparator  = ',';
        public static char openChildTags  = '(';
        public static char closeChildTags = ')';

        /// <summary>
        /// Converts a list of DreamMainTag objects to a string showing them
        /// as a list in the format tag1,tag2(childTag1;childTag2)
        /// </summary>
        /// <param name="tagList">a list of DreamMainTag objects</param>
        /// <returns>A string showing the tags as a list</returns>
        public string TagsToString(List<DreamMainTag> tagList) {
            if (tagList == null || tagList.Count == 0) {
                return string.Empty;
            }
            string[] mainTagsStr = new string[tagList.Count];
            string[] childTagsStr;
            for (int i = 0; i < tagList.Count; i++) {
                mainTagsStr[i] = tagList[i].TagName;
                /* If there are child tags, append them to the main tag string */   
                if (tagList[i].CountChildTags() > 0) {
                    childTagsStr = new string[tagList[i].CountChildTags()];
                    for (int j = 0; j < tagList[i].ChildTags.Count; j++) {
                        if (! string.IsNullOrEmpty(tagList[i].ChildTags[j].TagName)) {
                            childTagsStr[j] = tagList[i].ChildTags[j].TagName;
                        }
                    }
                    mainTagsStr[i] = mainTagsStr[i] + " " + openChildTags +
                        string.Join(char.ToString(mainTagSeparator) + " ",
                        childTagsStr) + closeChildTags;
                }
            }
            return string.Join(char.ToString(mainTagSeparator) + " ",
                mainTagsStr);
        }

        /// <summary>
        /// Converts a string representation of tags and child tags to a list
        /// of dream main tag objects
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<DreamMainTag> StringToTags(string str) {
            List<DreamMainTag> tagList = new List<DreamMainTag>();
            if (string.IsNullOrWhiteSpace(str)) {
                return tagList;
            }
            string[] mainTags = ExtractMainTags(str.Trim());
            for (int i = 0; i < mainTags.Length; i++) {
                int childStart = mainTags[i].IndexOf(openChildTags);
                if (childStart == -1) {
                    childStart = mainTags[i].Length;
                }
                DreamMainTag newTag = 
                    new DreamMainTag(mainTags[i].Substring(0,childStart));
                if (childStart < mainTags[i].Length) {
                    string[] childTags = 
                        ExtractChildTags(mainTags[i].Substring(childStart, 
                        mainTags[i].Length - childStart));
                    for (int j = 0; j < childTags.Length; j++) {
                        newTag.AddChildTag(childTags[j]);
                    }
                }
                tagList.Add(newTag);
            }
            return tagList;
        }

        /// <summary>
        /// Separates the main tags in a string and returns a string array
        /// with each tag
        /// </summary>
        /// <param name="str">the string to parse</param>
        /// <returns></returns>
        private string[] ExtractMainTags(string str) {
            List<string> mainTagList = new List<string>();
            /**
             * The cursor is the current position, end is where a main tag
             * ends
             */ 
            int cursor = 0;
            int start = 0; // Start of main tag
            /**
             * If true, we are inside brackets and all commas will be ignored
             * and not considered to end the main tag
             */
            bool childTagOpen = false;
            /**
             * If a child tag closed and the next non-whitespace character is
             * not the separator, it will end the last tag at cursor-1
             */ 
            bool childTagClosed = false;
            while (cursor < str.Length) {
                // Skip all white space
                if (str[cursor] == ' ') {
                    cursor++;
                    continue;
                }
                // child tags open
                else if (str[cursor] == openChildTags) {
                    childTagOpen = true;
                }
                // child tags close
                else if (str[cursor] == closeChildTags) {
                    childTagOpen = false;
                    childTagClosed = true;
                } 
                // if character after child tags closed is not the separator,
                // we consider there was one
                else if (str[cursor] != mainTagSeparator && 
                    childTagClosed == true) {
                        mainTagList.Add((str.Substring(start, cursor - start)).Trim());
                    childTagClosed = false;
                    childTagOpen = false;
                    start = cursor;
                }
                else if (str[cursor] == mainTagSeparator && !childTagOpen) {
                    mainTagList.Add((str.Substring(start, cursor - start)).Trim());
                    childTagOpen = false;
                    childTagClosed = false;
                    start = cursor + 1;
                }
                cursor++;
            }
            mainTagList.Add((str.Substring(start, cursor - start)).Trim());
            return mainTagList.ToArray();
        }

        /// <summary>
        /// Returns a list with the child tags of a main tag
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A string list with the child tags or an empty one
        /// if there was an error</returns>
        private string[] ExtractChildTags(string str) {
            int startPosition = str.IndexOf(openChildTags);
            int endPosition = str.IndexOf(closeChildTags);
            if (endPosition == -1) {
                endPosition = str.Length - 1;
            }
            if (startPosition == -1 || !(startPosition < str.Length)) {
                return new string[0];
            }
            string[] strings = 
                str.Substring(startPosition + 1, endPosition - 
                startPosition-1).Split(mainTagSeparator);
            for (int i = 0; i < strings.Length; i++) {
                strings[i] = strings[i].Trim();
            }
            return strings;
        }
    }
}
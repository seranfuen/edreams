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
using eDream.program;
using EvilTools;

namespace eDream.libs {

    /// <summary>
    /// This class parses the tags of a list of dream entries and generates
    /// statistics of the number of times each tag has appeared, or a list
    /// of unique tags found
    /// </summary>
    class DreamTagStatistics {
    
        /// <summary>
        /// Number of spaces on the left for making child tags distinct from
        /// their parents in the formatted list
        /// </summary>
        private const int leftTab = 5;

        /// <summary>
        /// RTF commands for the formatted list
        /// </summary>
        private const string RTFHeader = 
                            @"{\rtf1\ansi{\fonttbl\f0\fswiss Helvetica;}\fs20";
        private const string boldStart = @"\b ";
        private const string boldEnd = @"\b0 ";
        private const string lineBreak = @"\line";

        /// <summary>
        /// Separator for a list of tags
        /// </summary>
        private const string tagListSeparator = ", ";
        
        /// <summary>
        /// List objects for parsing the tags
        /// </summary>
        private List<DreamEntry> dreamEntries;
        private List<DreamDayEntry> dayList;
        /// <summary>
        /// The list of DreamMainStatTag objects once the entries are parsed
        /// </summary>
        private List<DreamMainStatTag> tagStatistics;
        /// <summary>
        /// Number of (valid) total days and (valid) total entries contained
        /// </summary>
        private int totalDays = 0;
        private int totalEntries = 0;

        /// <summary>
        /// Gets the number of different days that have entries
        /// </summary>
        public int TotalDays {
            get {
                return totalDays;
            }
        }

        /// <summary>
        /// Total number of entries parsed
        /// </summary>
        public int TotalEntries {
            get {
                return totalEntries;
            }
        }

        /// <summary>
        /// Returns the list of DreamMainStatTag objects
        /// </summary>
        public List<DreamMainStatTag> TagStatistics {
            get {
                return tagStatistics;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newEntries">The Entries to be Parsed</param>
        /// <param name="dayList">List of days containing entries</param>
        public void GenerateStatistics(List<DreamEntry> newEntries,
                                       List<DreamDayEntry> dayList) {
            dreamEntries = newEntries;
            this.dayList = dayList;
            tagStatistics = new List<DreamMainStatTag>();
            // Process all the dreamEntries list
            for (int i = 0; i < dreamEntries.Count; i++) {
                // Skip if invalid entry (e.g. set to delete)
                if (!dreamEntries[i].GetIfValid()) {
                    continue;
                }
                var iTags = dreamEntries[i].GetTagsAsList();
                /**
                 * Loops through all the tags obtained from the entry and
                 * searches the current tagStatistics list. If they are found,
                 * their count will be increased. If not found, a new StatTag
                 * will be created at the end of the list
                 */ 
                for (int j = 0; j < iTags.Count; j++) {
                    // Tag will be at this position if it is added to the list
                    int mainTagPos = tagStatistics.Count; 
                    // Search for a tentative position of an already existing list
                    int pos = IsInList(tagStatistics, iTags[j]);
                    if (pos == -1) { // Not found, add new stat tag
                        tagStatistics.Add(new DreamMainStatTag(iTags[j].Tag));
                    }
                    else { // Found, set mainTagPos to the pos found
                        mainTagPos = pos;
                    }
                    tagStatistics[mainTagPos].IncreaseCount();
                    // Parse all its child tags
                    List<DreamChildTag> iChildTags = iTags[j].ChildTags;
                    for (int k = 0; k < iChildTags.Count; k++) {
                        tagStatistics[mainTagPos].IncreaseChildCount(iChildTags[k].Tag);
                    }
                }
            }
            // Sort by tag count in reverse order the main tags
            tagStatistics.Sort();
            tagStatistics.Reverse();
            CountEntriesAndDays();
        }
        
        /// <summary>
        /// Generates an RTF format string with the statistics from higher count
        /// to lower, including child tags
        /// </summary>
        /// <returns></returns>
        public string GenerateStatisticsStr() {
            string str = RTFHeader;
            for (int i = 0; i < tagStatistics.Count; i++) {
                str += boldStart + tagStatistics[i].Tag +
                    boldEnd + "  —  " +
                    tagStatistics[i].TagCount + "  (" + 
                    StringUtils.GeneratePercentageAsStr(tagStatistics[i].TagCount, TotalEntries) + 
                    "%)" + lineBreak;
                List<DreamChildStatTag> childTags = tagStatistics[i].ChildTags;
                for (int j = 0; j < childTags.Count; j++) {
                    str += StringUtils.GenerateSpaces(leftTab) +
                        childTags[j].Tag + "  —  " +
                        childTags[j].TagCount + "  (" +
                        StringUtils.GeneratePercentageAsStr(childTags[j].TagCount,
                        childTags.Count) + "%)" + lineBreak;
                }
                str += lineBreak;
            }
            return str;
        }

        /// <summary>
        /// Returns a string array containing the names of all the tags parsed
        /// </summary>
        /// <returns></returns>
        public string[] GetTagList() {
            string[] tagList = new string[tagStatistics.Count];
            for (int i = 0; i < tagStatistics.Count; i++) {
                if (!string.IsNullOrWhiteSpace(tagStatistics[i].Tag)) {
                    tagList[i] = tagStatistics[i].Tag;
                }
            }
            return tagList;
        }

        /// <summary>
        /// Searches the tagStatistics list for the given tag. If it is found,
        /// will return its index position within the list. If not found, will
        /// return -1
        /// </summary>
        /// <param name="list">The list where it will be searched</param>
        /// <param name="element">A DreamTag object to search in list</param>
        /// <returns>-1 if not found, index number within the provided list
        /// if found</returns>
        private int IsInList(List<DreamMainStatTag> list, DreamTag element) {
            if (element == null || list == null) {
                return -1;
            }
            for (int i = 0; i < list.Count; i++) {
                if (list[i].Tag.ToLower() ==
                    element.Tag.ToLower()) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Counts the number of valid days and entries
        /// </summary>
        private void CountEntriesAndDays() {
            totalDays = 0;
            // Days are considered valid if they contain at least one entry
            for (int i = 0; i < dayList.Count; i++) {
                if (dayList[i].Count > 0) {
                    totalDays++;
                }
            }
            totalEntries = 0;
            for (int i = 0; i < dreamEntries.Count; i++) {
                if (dreamEntries[i].GetIfValid()) {
                    totalEntries++;
                }
            }
        }
    }
}

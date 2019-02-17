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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using eDream.libs;
using eDream.program;

namespace eDream.GUI {
    /// <summary>
    /// This form displays a list of all dream tags passed to the constructor
    /// sorted by count (number of times they have appeared). It has the option
    /// to display or to hide child tags, as well as displaying other miscellanea
    /// statistics
    /// </summary>
    internal partial class DreamsStatisticsShow : Form {
        /// <summary>
        /// The raw dream tag statistics
        /// </summary>
        private DreamTagStatistics stats;

        // If flag to true, will show child tags
        private bool showChildTags = true;

        /// <summary>
        /// Create a new form
        /// </summary>
        /// <param name="stats">A DreamTagStatistics object with the tags
        /// parsed</param>
        public DreamsStatisticsShow(DreamTagStatistics stats) {
            InitializeComponent();
            this.stats = stats;
            StartPosition = FormStartPosition.CenterScreen;
            SetTableData(GenerateData(stats.TagStatistics));
            DisplayStatisticsLabel(stats.TotalEntries, stats.TotalDays);
        }

        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            Dispose();
        }

        /// <summary>
        /// Displays a label with the number of entries, of total days and the
        /// percentage of times each entry appears over the total days
        /// </summary>
        /// <param name="totalEntries">The number of total entries</param>
        /// <param name="totalDays">The number of total days</param>
        private void DisplayStatisticsLabel(int totalEntries, int totalDays) {
            string str = string.Format("There are {0} dreams in {1} days",
                totalEntries, totalDays);
            if (totalDays > 0)
                str += string.Format(" ({0} dreams/day)", ((float)totalEntries/
                    (float)totalDays).ToString("0.00"));    
            totalDreamsLabel.Text = str;
        }

        /// <summary>
        /// Generates the TagStatTableData objects to display in the table
        /// </summary>
        /// <param name="statTags">A list of main tags to extract the rows from</param>
        /// <returns>The rows for the table</returns>
        private List<TagStatTableData> GenerateData(List<DreamMainStatTag> statTags) {
            List<TagStatTableData> tableData = new List<TagStatTableData>();
            if (statTags == null) {
                return tableData;
            }
            for (int i = 0; i < statTags.Count; i++) {
                tableData.Add(new TagStatTableData(statTags[i].Tag, 
                    statTags[i].TagCount, stats.TotalEntries, false));
                // Generate child tags too
                if (showChildTags) {
                    List<DreamChildStatTag> childTags = statTags[i].ChildTags;
                    for (int j = 0; j < childTags.Count; j++) {
                        tableData.Add(new TagStatTableData(childTags[j].Tag,
                            childTags[j].TagCount, stats.TotalEntries, true));
                    }
                }
            }
            return tableData;
        }

        /// <summary>
        /// When no dream statistics available, show a message telling the user
        /// there are no entries
        /// </summary>
        private void SetTableDefaultData() {
            List<TagStatTableData> data = new List<TagStatTableData>();
            data.Add(new TagStatTableData("No dream entries", 0, 0, false));
            statsTable.DataSource = data;
            statsTable.Enabled = false;
            SetColumns();
        }

        /// <summary>
        /// Loads the parsed main tag statistics to the table
        /// </summary>
        /// <param name="data">The list rows</param>
        private void SetTableData(List<TagStatTableData> data) {
            if (data.Count == 0) {
                SetTableDefaultData();
                return;
            }
            statsTable.DataSource = data;
            statsTable.Enabled = true;
            SetColumns();
        }


        /// <summary>
        /// Gives size to the two columns and set them to non-editable.
        /// It should be called when TagTableData data has been set for the table.
        /// Otherwise, it will catch th exception and do nothing
        /// </summary>
        private void SetColumns() {
            try {
                statsTable.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
                statsTable.Columns[1].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.ColumnHeader;
                statsTable.Columns[2].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.ColumnHeader;
                statsTable.Columns[2].HeaderText = "% of dreams";
                statsTable.Columns[0].ReadOnly = true;
                statsTable.Columns[0].SortMode =
                    DataGridViewColumnSortMode.Automatic;
                statsTable.Columns[1].ReadOnly = true;
                statsTable.Columns[1].SortMode =
                    DataGridViewColumnSortMode.Automatic;
                statsTable.Columns[2].ReadOnly = true;
                statsTable.Columns[2].SortMode =
                    DataGridViewColumnSortMode.Automatic;
            }
            catch (Exception e) {
                Console.WriteLine("<<DEBUG>>\n Error in SetColumns():\n   " +
                    e.Message);
            }
        }

        /// <summary>
        /// When clicking on the check box to show or not child tags, refresh
        /// the table with the new settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void childTagsCheck_CheckedChanged(object sender, EventArgs e) {
            if (childTagsCheck.Checked != showChildTags) {
                showChildTags = childTagsCheck.Checked;
                SetTableData(GenerateData(stats.TagStatistics));
            }
        }
    }
}

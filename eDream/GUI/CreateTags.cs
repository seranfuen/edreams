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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms; 
using eDream.libs;
using eDream.program;

namespace eDream.GUI {
        internal partial class CreateTags : Form {

        // Stat tagts to display already existing tags
        List<DreamMainStatTag> statTags;

        /// <summary>
        /// Padding (tabs) added to child tags to make them distinctive 
        /// from main tags
        /// </summary>
        private const int childTab = 4;

        /// <summary>
        /// True if showing child tags, false otherwse
        /// </summary>
        private bool showingChildTags = false;

        /// <summary>
        /// The parent window
        /// </summary>
        private Form parent;

        /// <summary>
        /// This signal is emitted whenever the tag text is modified
        /// </summary>
        public EventHandler OnTagTextChange;

        /// <summary>
        /// Initial tags as sent from the caller, so they can be reset
        /// </summary>
        private string initialTags = "";

        /// <summary>
        /// Gets or sets the text currently displayed in the tag box
        /// </summary>
        public string TagText {
            set {
                tagBox.Text = value;
            }

            get {
                return tagBox.Text;
            }
        }

        /// <summary>
        /// Create a Create Tags window that will allow to easily add tags
        /// based on all the tags already existing
        /// </summary>
        /// <param name="parent">The parent Form</param>
        /// <param name="statTags">A list of DreamMainStatTag objects</param>
        /// <param name="initialTags">A string with the tags that will be shown
        /// by default to represent already existing tags being edited</param>
        public CreateTags(object parent, List<DreamMainStatTag> statTags, 
            string initialTags) {
            InitializeComponent();
            this.parent = (Form)parent;
            this.parent.FormClosed += new FormClosedEventHandler(ParentClosing);
            this.statTags = statTags;
            this.initialTags = initialTags;
            TagText = initialTags;
            StartPosition = FormStartPosition.CenterScreen;
            tagSearch.KeyDown += new KeyEventHandler(DetectKey);
            tagBox.TextChanged += new EventHandler(emitTextChanged);
            SetTableData(GenerateData(statTags));
            tagsTable.DoubleClick += new EventHandler(TableDoubleClick);
        }

        /// <summary>
        /// Loads the main tag statistics to the table
        /// </summary>
        /// <param name="data">List of TagTableData objects representing each
        /// table row</param>
        private void SetTableData(List<TagTableData> data) {
            if (data.Count == 0) {
                SetDefaultData();
            }
            else {
                tagsTable.DataSource = data;
                tagsTable.Enabled = true;
                SetColumns();
            }
        }

        /// <summary>
        /// Sets default data informing the user there is no data available
        /// </summary>
        private void SetDefaultData() {
            List<TagTableData> data = new List<TagTableData>();
            data.Add(new TagTableData() {
                Tag = "No tags available",
                Count = 0
            });
            tagsTable.DataSource = data;
            tagsTable.Enabled = false;
            SetColumns();
        }

        /// <summary>
        /// Sets the table back to the original tags (statTags list)
        /// </summary>
        private void ResetInitialData() {
            SetTableData(GenerateData(statTags));
        }

        /// <summary>
        /// Gives size to the two columns and set them to non-editable.
        /// It should be called when TagTableData data has been set
        /// Otherwise, it will throw an exception and do nothing
        /// </summary>
        private void SetColumns() {
            try {
                tagsTable.AutoSizeColumnsMode = 
                    DataGridViewAutoSizeColumnsMode.Fill;
                tagsTable.Columns[1].AutoSizeMode = 
                    DataGridViewAutoSizeColumnMode.ColumnHeader;
                tagsTable.Columns[0].ReadOnly = true;
                tagsTable.Columns[0].SortMode = 
                    DataGridViewColumnSortMode.Automatic;
                tagsTable.Columns[1].ReadOnly = true;
                tagsTable.Columns[1].SortMode =
                    DataGridViewColumnSortMode.Automatic;
            } catch (Exception e) {
                Console.WriteLine("<<DEBUG>>\n Error in SetColumns():\n   " +
                    e.Message);
            }
        }



        /// <summary>
        /// Generates a list of TagTableData object from the statTags given,
        /// so it can be displayed in the list
        /// </summary>
        /// <param name="statTags"></param>
        /// <returns></returns>
        private List<TagTableData> GenerateData(List<DreamMainStatTag> statTags) {
            List<TagTableData> tableData = new List<TagTableData>();
            if (statTags == null || statTags.Count == 0) {
                return tableData;
            }
            for (int i = 0; i < statTags.Count; i++) {
                tableData.Add(new TagTableData {
                    Tag = statTags[i].Tag,
                    Count = statTags[i].TagCount
                });
                if (showingChildTags) {
                    // Add child tags
                    List<DreamChildStatTag> childTags = statTags[i].ChildStatTags;
                    for (int j = 0; j < childTags.Count; j++) {
                        tableData.Add(new TagTableData {
                            Tag = EvilTools.StringUtils.GenerateSpaces(childTab)
                            + childTags[j].Tag, Count = childTags[j].TagCount
                        });
                    }
                }
            }
            return tableData;
        }

        /// <summary>
        /// Searches a list of dream tags for the string provided and returns
        /// a list of any matching tags parsed to be shown in the table
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        private List<TagTableData> SearchData(List<DreamMainStatTag> stats, 
            string search) {
            List<TagTableData> finds = new List<TagTableData>();
            search = search.ToLower();
                for (int i = 0; i < stats.Count; i++) {
                    if (stats[i].Tag.ToLower().IndexOf(search) != -1) {
                        finds.Add(new TagTableData() {
                            Tag = stats[i].Tag,
                            Count = stats[i].TagCount
                        });
                    }
                    // Add child tags
                    if (showingChildTags) {
                        List<DreamChildStatTag> childTags = statTags[i].ChildStatTags;
                        for (int j = 0; j < childTags.Count; j++) {
                            finds.Add(new TagTableData {
                                Tag = childTab + childTags[j].Tag,
                                Count = childTags[j].TagCount
                            });
                        }
                    }
                }
                return finds;
        }

        /// <summary>
        /// If there is a number of rows (tags) selected in the table, it will
        /// add them at the end of the tagBox text
        /// </summary>
        private void AddSelectedTags() {
            DataGridViewSelectedRowCollection theRows = tagsTable.SelectedRows;
            if (theRows == null || theRows.Count == 0) {
                return;
            }
            string[] tagsToAdd = new string[theRows.Count];
            for (int i = 0; i < theRows.Count; i++) {
                tagsToAdd[i] = (string)theRows[i].Cells[0].Value;
            }
            // Eliminate any whitespace (child tags are tabbed) 
            for (int i = 0; i < tagsToAdd.Length; i++) {
                tagsToAdd[i] = tagsToAdd[i].Trim();
            }
            // Insert each tag if it matches the conditions such as not being already there
            for (int i = tagsToAdd.Length-1; i >= 0; i--) {
                InsertTag(tagsToAdd[i]);
            }
        }

        /// <summary>
        /// Perform tag search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(tagSearch.Text)) {
                ResetInitialData();
            }
            SetTableData(SearchData(statTags, tagSearch.Text));
        }

        /// <summary>
        /// Resets the tag search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            tagSearch.Text = string.Empty;
            ResetInitialData();
        }

        /// <summary>
        /// If enter key pressed in tag search dialog, perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetectKey(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Return) {
                e.SuppressKeyPress = true;
                searchButton_Click(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Add the currently selected tag when double clicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableDoubleClick(object sender, EventArgs e) {
            AddSelectedTags();
        }

        /// <summary>
        /// From the context menu, add all the tags currently selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSelectedEntriesToolStripMenuItem_Click(object sender, 
            EventArgs e) {
            AddSelectedTags();
        }

        /// <summary>
        /// Close the window (the tags should already by in the parent connected
        /// to text changed event)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createButton_Click(object sender, EventArgs e) {
            Dispose();
        }

        /// <summary>
        /// Reset the tag text and set it back to the original ones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReset_Click(object sender, EventArgs e) {
            tagBox.Text = initialTags;
        }

        /// <summary>
        /// When display child tags checkbox changed, set the showingChildTags
        /// flag to the value and repopulate table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayChildTagsCheck_CheckedChanged(object sender, EventArgs e) {
            // If status changed from last time, regenerate table
            if (displayChildTagsCheck.Checked != showingChildTags) {
                showingChildTags = displayChildTagsCheck.Checked;
                SetTableData(GenerateData(statTags));
            }
        }

        /// <summary>
        /// Emit signal from the form to indicate its tag box was modified
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void emitTextChanged(object sender, EventArgs e) {
            if (OnTagTextChange != null) {
                OnTagTextChange(this, e);
            }
        }

        /// <summary>
        /// Returns the name of the parent tag or String.Empty if no parent
        /// tag. If there are more than one parent tag corresponding to the
        /// child tag, will return the parent tag with higher count
        /// </summary>
        /// <param name="childTag"></param>
        /// <returns></returns>
        private string FindParentTag(string childTag) {
            int maxCount = 0;
            string parent = string.Empty;
            for (int i = 0; i < statTags.Count; i++) {
                for (int j = 0; j < statTags[i].ChildStatTags.Count; j++) {
                    if (string.Compare(statTags[i].ChildStatTags[j].Tag,
                        childTag, true) == 0) {
                            if (maxCount < statTags[i].TagCount) {
                                parent = statTags[i].Tag;
                                maxCount = statTags[i].TagCount;
                            }
                    }
                }
            }
            return parent;
        }

        /// <summary>
        /// Tries to insert a child tag into the box.
        /// If it is a main tag, it will try to insert if it isn't already
        /// in the text.
        /// Otherwise, it will try to find the parent tag:
        /// if it is found, it will add the child tag provided it's not already
        /// present. If the parent tag is not present, it will add the
        /// parent tag together with the child tag
        /// <param name="childTag"></param>
        /// <returns></returns>
        private void InsertTag(string tag) {
            tag = tag.Trim();
            TagText = TagText.Trim();
            string parent = FindParentTag(tag);
            // If it is main
            if (parent == string.Empty) {
                if (!MatchMainTag(tag)) {
                    WriteTag(tag);
                }
                return;
            }
            // If it is child
            // If parent tag can't be found, add the child tag with the parent 
            if (!MatchMainTag(parent)) {
                WriteTag(parent + " " + DreamTagTokens.OpenChildTags +
                    tag + DreamTagTokens.CloseChildTags);
                return;
            }
            // If parent found check if it contains no child tags: add them
            if (!Regex.IsMatch(TagText, @"\b(,|\s*)" + parent + @"\s*\(",
                RegexOptions.IgnoreCase)) {
                    TagText = 
                        Regex.Replace(TagText, @"\b\s*" + parent + @"\s*",
                        parent + " " + DreamTagTokens.OpenChildTags + tag +
                        DreamTagTokens.CloseChildTags, RegexOptions.IgnoreCase);
                    return;
            }

            /* If parent already has child tags, we check if the child tag
             * is already present. If not, add it */
            Match mTag = Regex.Match(TagText, @"\b(,|\s*)" + Regex.Escape(parent)
                + @"\s*\(.*" + Regex.Escape(tag) + @".*\)",
                RegexOptions.IgnoreCase);
            if (mTag.Success) {
                return;
            }
            mTag = Regex.Match(TagText, @"\b(,|\s*)" + Regex.Escape(parent) +
                @"\s*\(.*\)", RegexOptions.IgnoreCase);
            if (!mTag.Success) {
                return;
            }
            string nTag = mTag.Value.Substring(0, 
                mTag.Value.LastIndexOf(DreamTagTokens.CloseChildTags)).Trim();
            string sep = nTag[nTag.Length - 1] == 
                DreamTagTokens.MainTagSeparator ? " " : 
                char.ToString(DreamTagTokens.MainTagSeparator) + " ";
            TagText = Regex.Replace(TagText, Regex.Escape(mTag.Value),
                nTag+ sep + tag + char.ToString(DreamTagTokens.CloseChildTags),
                RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Writes the current tag to the last position of the tagBox text
        /// </summary>
        /// <param name="str"></param>
        private void WriteTag(string str) {
            string sep = " ";
            if (TagText.Length > 1 && 
                TagText[TagText.Length - 1] != 
                DreamTagTokens.MainTagSeparator) {
                    sep = char.ToString(DreamTagTokens.MainTagSeparator) + " ";
            }
            TagText += sep + str;
            TagText = TagText.Trim();
        }

        /// <summary>
        /// Returns true if it can find a main tag in the tagBox, false otherwise.
        /// A main tag is a tag preceded by whitespace or , and that ends with
        /// whitespace, ( or comma
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool MatchMainTag(string str) {
            if (Regex.IsMatch(TagText, @"\b(,|\s*)" + str + @"(\s|\b|\(|,)",
                RegexOptions.IgnoreCase)) {
                    return true;
            }
            return false;
        }

        /// <summary>
        /// If parent has emitted its signal and is closing, close this as well
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentClosing(object sender, EventArgs e) {
            Dispose();
        }
   }
}
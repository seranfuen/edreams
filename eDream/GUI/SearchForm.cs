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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using eDream.program;
using eDream.libs;

namespace eDream.GUI {
    /// <summary>
    /// This form performs searches of the dream entries loaded and emits a signal
    /// when the search is ready
    /// </summary>
    internal partial class SearchForm : Form {
        
        /// <summary>
        /// Constants for the type of search last performed
        /// </summary>
        public enum ESearchType {
            textSearch,
            tagsSearch,
            dateSearch
        }
        /// <summary>
        /// The entries to be searched
        /// </summary>
        private List<DreamEntry> dreamEntries;

        /// <summary>
        /// THe searchUtils object
        /// </summary>
        private DreamSearchUtils searchUtils = new DreamSearchUtils();

        /// <summary>
        /// The results
        /// </summary>
        private List<DreamEntry> results = new List<DreamEntry>();

        /// <summary>
        /// Type of last search
        /// </summary>
        private ESearchType searchType;

        /// <summary>
        /// The text that we tried to find, or the tags that we tried to find,
        /// in the last search
        /// </summary>
        private string lastSearchText;

        /// <summary>
        /// The delegate for search events, that will be emitted whenever a new
        /// search has been performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        public delegate void SearchEvent(object sender, EventArgs arguments);

        /// <summary>
        /// This event is fired whenever a search has been performed
        /// </summary>
        public SearchEvent OnSearchCompleted;

        /// <summary>
        /// Signal send when the clear buttons are pressed
        /// </summary>
        public EventHandler OnClear;

        /// <summary>
        /// Returns a list of dreamentry objects with the results or an emtpy
        /// list if no results found
        /// </summary>
        public List<DreamEntry> Results {
            get {
                return results;
            }
        }

        /// <summary>
        /// Sets a list of DreamEntries
        /// </summary>
        public List<DreamEntry> DreamEntries {
            set {
                this.dreamEntries = value;
            }
        }

        /// <summary>
        /// The type of the last search performed
        /// </summary>
        public ESearchType SearchType {
            get {
                return searchType;
            }
        }

        /// <summary>
        /// The text that we tried to find, or the tags that we tried to find,
        /// in the last search
        /// </summary>
        public string LastSearchText {
            get {
                return lastSearchText;
            }
        }

        public SearchForm(List<DreamEntry> entries) {
            InitializeComponent();
            this.dreamEntries = entries;
            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;
            textSearchFindButton.Enabled = false;
            findTagsButton.Enabled = false;
            andRadio.Checked = true;
            clearTagsButton.Enabled = false;
            textClearButton.Enabled = false;
            fromTimePicker.Value = DateTime.Now;
            toTimePicker.Value = DateTime.Now;
            this.FormClosing += new FormClosingEventHandler(PreventDestroy);
            textSearchBox.TextChanged += new EventHandler(TextSearchBoxChanged);
            tagsTextBox.TextChanged += new EventHandler(TextSearchBoxChanged);
            textSearchBox.KeyDown += new KeyEventHandler(EnterKeyPressed);
            tagsTextBox.KeyDown += new KeyEventHandler(EnterKeyPressed);
            clearTagsButton.Click += new EventHandler(SendClear);
            textClearButton.Click += new EventHandler(SendClear);
            dateClearButton.Click += new EventHandler(SendClear);
            this.KeyDown += new KeyEventHandler(CloseWindow);
        }

        /// <summary>
        /// Handle events by the text box where the search words are typed 
        /// It will enable or disable the search button
        /// depending on whether there is any text present
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TextSearchBoxChanged(object sender, EventArgs args) {
            TextBox box = (TextBox)sender;
            /* If the text in the box is whitespace, decide which button,
             * depending on which textBox the object is, to disable. Otherwise,
             * enable it
             */
            bool isEmpty = String.IsNullOrWhiteSpace(box.Text);
            if (box.Equals(textSearchBox)) {
                textSearchFindButton.Enabled = !isEmpty;
            } else if (box.Equals(tagsTextBox)) {
                findTagsButton.Enabled = !isEmpty;
            }
        }

        /// <summary>
        /// Peform a text search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textSearchFindButton_Click(object sender, EventArgs e) {
            results = searchUtils.SearchEntriesText(dreamEntries,
                textSearchBox.Text);
            lastSearchText = textSearchBox.Text;
            searchType =  ESearchType.textSearch;
            if (OnSearchCompleted != null) {
                OnSearchCompleted(this, new EventArgs());
                clearTagsButton.Enabled = true;
                textClearButton.Enabled = true;
                dateClearButton.Enabled = true;
            }

        }

        /// <summary>
        /// Perform a tag search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findTagsButton_Click(object sender, EventArgs e) {
            string[] tags = 
                tagsTextBox.Text.Split(DreamTagParser.mainTagSeparator);
            DreamSearchUtils.TagSearchType searchType;
            if (orRadio.Checked) {
                searchType = DreamSearchUtils.TagSearchType.ORSearch;
            }
            else {
                searchType = DreamSearchUtils.TagSearchType.ANDSearch;
            }
            results = searchUtils.SearchEntriesTags(dreamEntries, tags,
                checkChildTags.Checked, searchType);
            lastSearchText = String.Join(", ", tags);
            this.searchType = ESearchType.tagsSearch;
            if (OnSearchCompleted != null) {
                OnSearchCompleted(this, new EventArgs());
                clearTagsButton.Enabled = true;
                textClearButton.Enabled = true;
                dateClearButton.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the pressing of Enter Keys inside text search boxes, to 
        /// perform the said of the currently active tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterKeyPressed(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                TabPage activePage = searchTabs.SelectedTab;
                if (activePage == textSearchPage) {
                    textSearchFindButton.PerformClick();
                }
                else if (activePage == tagsSearchPage) {
                    findTagsButton.PerformClick();
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Close window when pressing escape
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
            }
        }


        /// <summary>
        /// Prevents the window from being destroyed upon closing: it will just
        /// be hidden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreventDestroy(object sender, FormClosingEventArgs e) {
            // If closing app or parent do not prevent it
            if (e.CloseReason == CloseReason.ApplicationExitCall ||
                e.CloseReason == CloseReason.FormOwnerClosing) {
                return;
            }
            e.Cancel = true;
            this.Visible = false;
        }

        /// <summary>
        /// Sends clear signal to inform that the clear buttons were pressed
        /// and that the main window should clear any searches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendClear(object sender, EventArgs e) {
            clearTagsButton.Enabled = false;
            textClearButton.Enabled = false;
            dateClearButton.Enabled = false;
            if (OnClear != null) {
                OnClear(this, e);
            }
        }

        /// <summary>
        /// When Single Day is clicked on, disabled the "to date" pickers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            if (checkBox2.Checked) {
                toTimePicker.Enabled = false;
                fromDateLabel.Text = "On this date";
            }
            else {
                toTimePicker.Enabled = true;
                fromDateLabel.Text = "From";
            }
        }

        /// <summary>
        /// Perform a date search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            searchType = ESearchType.dateSearch;
            if (checkBox2.Checked) {
                results = searchUtils.SearchEntriesOnDate(dreamEntries,
                    fromTimePicker.Value);
            }
            else {
                results = searchUtils.SearchEntriesDateRange(dreamEntries,
                    fromTimePicker.Value, toTimePicker.Value);
            }
            if (OnSearchCompleted != null) {
                OnSearchCompleted(this, new EventArgs());
                clearTagsButton.Enabled = true;
                textClearButton.Enabled = true;
                dateClearButton.Enabled = true;
            }
        }
    }
}

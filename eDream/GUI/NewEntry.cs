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
using System.Media;
using System.Text.RegularExpressions;

namespace eDream.GUI {
    /// <summary>
    /// A form used to input the data to create or to edit a dream entry
    /// </summary>
    internal partial class NewEntryForm : Form {

        /// <summary>
        /// Modified flag is set to true if the entry is modified from its 
        /// original state
        /// </summary>
        private bool modified = false;
        
        /// <summary>
        /// The new or modified dream entry that can be retrived by the caller
        /// </summary>
        private DreamEntry newEntry;

        /// <summary>
        /// The original entry to be edited
        /// </summary>
        private DreamEntry editEntry;

        /// <summary>
        /// Let the caller know if a new entry was created, or the changed to
        /// an entry were saved, so newEntry can be retrieved
        /// </summary>
        private bool createdEntry = false;
        
        /// <summary>
        /// If this flag is true, the form is considered to be in "Edition" 
        /// mode, editing an already existing entry. If false, a new entry
        /// is bieng created
        /// </summary>
        private bool inEdition = false;

        /// <summary>
        /// Tag Wizard form object
        /// </summary>
        private CreateTags tagWiz;

        /// <summary>
        /// A list of all the tags contained by the entries, passed by the
        /// calling object to use with the Tag Wizard
        /// </summary>
        private List<DreamMainStatTag> tagStats;

        /// <summary>
        /// Create a new form. The tag stats will be used by the tag wizard
        /// </summary>
        /// <param name="tagStats">Tag statistics</param>
        public NewEntryForm(List<DreamMainStatTag> tagStats) {
            InitializeComponent();
            SetUpForm();
            this.tagStats = tagStats;
            datePicker.Text = DateTime.Today.ToString();
        }

        /// <summary>
        /// Create a new form and load an already existing entry for edition
        /// </summary>
        /// <param name="editEntry">The dream entry to modify</param>
        /// <param name="tagStats">Tag statistics</param>
        public NewEntryForm(DreamEntry editEntry, List<DreamMainStatTag> tagStats) {
            InitializeComponent();
            SetUpForm();
            this.tagStats = tagStats;
            this.addEntryButton.Click -= addEntryButton_Click;
            this.addEntryButton.Click += new EventHandler(saveEdit);
            this.editEntry = editEntry;
            addEntryButton.Text = "Edit";
            this.Text = "Edit dream entry";
            inEdition = true;
            LoadEditEntry();
        }

        /// <summary>
        /// Common actions when setting up the form
        /// </summary>
        private void SetUpForm() {
            this.FormClosing += new FormClosingEventHandler(PreventChildClose);
            this.StartPosition = FormStartPosition.CenterParent;
            tagsBox.TextChanged += new EventHandler(entryModified);
            textBox.TextChanged += new EventHandler(entryModified);
            tagsBox.TextChanged += new EventHandler(notifyTagW);
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(SendForm);
        }

        /// <summary>
        /// Returns the new DreamEntry object that was created, or an edited
        /// version that was saved
        /// </summary>
        public DreamEntry NewEntry {
            get {
                return newEntry;
            }
        }

        /// <summary>
        /// True if user clicked on "Add/Save Entry" and created or modified
        /// an entry. This means that NewEntry can be retrieved
        /// </summary>
        public bool CreatedEntry {
            get {
                return createdEntry;
            }
        }

        /// <summary>
        /// When editing an entry, loads its contents to the text boxes and
        /// date picker
        /// </summary>
        private void LoadEditEntry() {
            this.textBox.Text = editEntry.Text;
            this.tagsBox.Text = editEntry.GetTagsAsString();
            datePicker.Text = editEntry.GetDateAsStr();
            CountWords();
        }


        /// <summary>
        /// Handles "TextModified" events from the text boxes to set the
        /// Modified flag accordingly or count the words in the main text
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="a"></param>
        private void entryModified(Object caller, EventArgs a) {
            if (caller.Equals(textBox)) {
                CountWords();
            }
            /* Due to the way this is written, any methods not related to the
             * Modified flag should go before this block, since the
             * function may end in the ifs
             */ 
            if (caller.Equals(textBox) || caller.Equals(tagsBox)) {
                if (inEdition) {
                    /**
                     * If in edition mode, it will be considered to be modified
                     * when the contents in the box don't match the ones in the
                     * original entry
                     */ 
                    if (String.Compare(textBox.Text, editEntry.Text)
                        == 0 && String.Compare(tagsBox.Text, 
                        editEntry.GetTagsAsString()) == 0) {
                        modified = false;
                        return;
                    }
                }
                /**
                 * In new entry mode, it will be modified when any of the boxes
                 * do not have a length of 0
                 */ 
                else if (textBox.Text.Length == 0 && 
                        tagsBox.Text.Length == 0) {
                    modified = false;
                    return;
                }
            }
            modified = true;
        }

        /// <summary>
        /// Close form, or asks, if Modified flag is set, user for confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButtonClick(object sender, EventArgs e) {
            if (!modified) {
                CloseTagWizard();
                this.Dispose();
            } else {
                SystemSounds.Exclamation.Play();
                DialogResult result = MessageBox.Show("Do you want to close " +
                    "the window? All information will be lost",
                        "Close", MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                if (result == DialogResult.Yes) {
                    CloseTagWizard();
                    this.Dispose();
                }
            }
        }

        /// <summary>
        /// This event is handled when we are editing an entry: if it's 
        /// possible to save the entry, it will set the editEntry parameters
        /// to the ones we have modified so the caller does not have to
        /// retrieve it, just use the object that it passed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveEdit(object sender, EventArgs e) {
            if (textBox.Text.Length == 0) {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("The dream entry has no text!", "Can't save " +
                    "this entry", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            editEntry.Text = textBox.Text;
            editEntry.SetTags(tagsBox.Text);
            editEntry.SetDate(datePicker.Text);
            createdEntry = true;
            this.Dispose();
        }

        /// <summary>
        /// If a valid entry has been input, create a DreamEntry object and
        /// make it available to the caller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEntryButton_Click(object sender, EventArgs e) {
            // Validate first: text cannot be empty
            if (textBox.Text.Length == 0) {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("The dream entry has no text!", "Can't add " +
                    "new entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            newEntry = new DreamEntry(datePicker.Text, tagsBox.Text, textBox.Text);
            createdEntry = true;
            this.Dispose();
        }

        /// <summary>
        /// Calls the tagCreator window and, if new or different tags are
        /// created in this form, changes the text in the tags box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTagButton_Click(object sender, EventArgs e) {
            // If already being shown, focus
            if (tagWiz != null && tagWiz.Visible) {
                tagWiz.Activate();
                return;
            }
            tagWiz = new CreateTags(this, tagStats, tagsBox.Text);
            tagWiz.OnTagTextChange += new EventHandler(wizardChanged);
            tagWiz.Show();
        }

        /// <summary>
        /// Whenever there is an open "Create Tag Wizard" window and its text
        /// is modified, modify the one here as well
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wizardChanged(object sender, EventArgs e) {
            CreateTags tagW = (CreateTags)sender;
            tagsBox.Text = tagW.TagText;
        }

        /// <summary>
        /// If there is a Tag Wizard Window open and the text that has been
        /// changed differs from the one in the tag wizard form (to prevent
        /// loops, since that window also notifies wizardChanged), change it
        /// to the one here. That way both windows are synchronized and there
        /// is no need for a "create tags" or "cancel button"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyTagW(object sender, EventArgs e) {
            if (tagWiz == null || tagWiz.Visible == false) {
                return;
            }
            if (tagsBox.Text != tagWiz.TagText) {
                tagWiz.TagText = tagsBox.Text;
            }
        }

        /// <summary>
        /// Counts the number of words in the main text and displays it
        /// </summary>
        private void CountWords() {
            string theText = textBox.Text.Trim();
            labelWordCount.Text = "Word count: " +
                Regex.Split(theText, @"\W").Length;
        }

        /// <summary>
        /// Sends/adds entry when pressing Control + Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendForm(object sender, KeyEventArgs e) {
            if (e.KeyData == (Keys.Control | Keys.Enter)) {
                e.SuppressKeyPress = true;
                addEntryButton.PerformClick();
            }
            else if (e.KeyData == Keys.Escape && sender.Equals(this)) {
                e.SuppressKeyPress = true;
                cancelButton.PerformClick();
            }
        }

        /// <summary>
        /// Prevents this form by being closed by its modal child tagCreator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreventChildClose(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.None) {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Close the tag wizard if the new entry window is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseTagWizard() {
            if (tagWiz != null) {
                tagWiz.Dispose();
            }
        }
    }
}
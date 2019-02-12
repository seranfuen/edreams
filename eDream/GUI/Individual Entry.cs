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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using eDream.program;
using System.Media;

namespace eDream.GUI {
    /// <summary>
    /// This control is used to display each dream entry
    /// </summary>
    internal partial class Individual_Entry : UserControl {

        private DreamEntry theEntry;
        
        // Its entry number within a day, to display "Entry n" in the interface
        private int entryN;
       
        /// <summary>
        /// A reference to the parent object, the eDreams class. We will not use
        /// an event handler here since this class isn't supposed to just
        /// support events and delegates in a standard way, but to interact
        /// with eDreams specifically, and we also need to retrieve certain data
        /// from eDreams
        /// </summary>
        private eDreams parent;



        /// <summary>
        /// Create a new instance of individual entry
        /// </summary>
        /// <param name="theEntry"></param>
        /// <param name="entryN"></param>
        /// <param name="parent"></param>
        public Individual_Entry(DreamEntry theEntry, int entryN, eDreams parent) {
            InitializeComponent();
            this.theEntry = theEntry;
            this.entryN = entryN;
            this.parent = parent;
            this.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | 
                AnchorStyles.Right | AnchorStyles.Top);
            SetEntry();
        }

        /// <summary>
        /// Set the entry contents
        /// </summary>
        public void SetEntry() {
            dateLabel.Text = theEntry.GetDateAsDayStr();
            tagsLabel.Text = theEntry.GetTagsAsString();
            dreamText.Text = theEntry.Text;
            wrapperBox.Text = "Entry " + entryN;
        }

        /// <summary>
        /// Prompt and, in case its affirmative, delete entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, EventArgs e) {
            SystemSounds.Exclamation.Play();
            if (MessageBox.Show("Do you really want to delete this entry?", 
                "Delete entry?", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes) {
                theEntry.ToDelete = true;
                this.Dispose();
                parent.SetModifiedState();
                parent.RefreshEntries();
            }
        }

        /// <summary>
        /// Open a NewEntry form to edit the current entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editButton_Click(object sender, EventArgs e) {
            NewEntryForm editEntry = new NewEntryForm(theEntry, parent.TagStatistics);
            editEntry.ShowDialog();
            if (editEntry.CreatedEntry) {
                parent.SetModifiedState();
                this.SetEntry();
            }
        }
    }
}

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
using System.Text.RegularExpressions;
using System.IO;
using eDream.libs;
using System.Media;

namespace eDream.GUI {
    /// <summary>
    /// Displays a window used to create a new database
    /// </summary>
    public partial class NewFileBox : Form {


        private string Folder;
        // Default new file
        private string NewFile = "my_dreams.xml";
        // The file extension
        private const string extension = ".xml";
        // Describes the action taken after the form is closed
        public enum EAction {
            Cancel,
            Created
        }

        private EAction action = EAction.Cancel;

        /// <summary>
        /// Gets the action (EAction enum) that has been taken by the user
        /// </summary>
        public EAction Action {
            get {
                return action;
            }
        }

        /// <summary>
        /// Gets the path to the file the user wants to create
        /// </summary>
        public string File {
            get {
                return Folder + "\\" + NewFile;
            }
        }

        /// <summary>
        /// Create a new form to create a new eDream database
        /// </summary>
        public NewFileBox() {
            InitializeComponent();
            Folder = Application.StartupPath;
            chooseFolderDialog.Description = "Choose or create a new folder";
            SetFolderText(Folder);
            SetNewFileText(NewFile);
        }

        /// <summary>
        /// Cancel the form and do nothing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e) {
            action = EAction.Cancel;
            this.Close();
        }


        private void SetFolderText(string text) {
            folderText.Text = text;
        }

        private void SetNewFileText(string text) {
            newFileText.Text = text;
        }

        /// <summary>
        /// Choose a folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chooseFolderButton_Click(object sender, EventArgs e) {
            chooseFolderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            chooseFolderDialog.SelectedPath = Application.StartupPath;
            DialogResult result = chooseFolderDialog.ShowDialog();
            if (result == DialogResult.OK) {
                Folder = chooseFolderDialog.SelectedPath;
                SetFolderText(Folder);
            }
        }

        /// <summary>
        /// Create new file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createButton_Click(object sender, EventArgs e) {
            NewFile = newFileText.Text;
            if (NewFile.Length < 4 || 
                String.Compare(NewFile.Substring(NewFile.Length-4), extension, 
                true) != 0) {
                NewFile = NewFile + extension;
            }
            // Check if file name is valid
            if (!CheckValidFile(NewFile)) {
                SystemSounds.Beep.Play();
                MessageBox.Show("The file contains illegal characters", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                action = EAction.Cancel;
                return;
            }
            // If file already exists, prompt to ask if user wants to replace it
            if (System.IO.File.Exists(Folder + "\\" + NewFile)) {
                SystemSounds.Beep.Play();
                DialogResult res = MessageBox.Show(this, NewFile +
                    " already exists. Do you wish to overwrite it?", 
                "Overwrite file?", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Exclamation);
                if (res == System.Windows.Forms.DialogResult.No) {
                    return;
                }
            }
            XMLWriter writer = new XMLWriter();
            if (writer.CreateFile(Folder + "\\" + NewFile)) {
                action = EAction.Created;
                this.Close();
            }
        }

        /// <summary>
        /// Check if the file has a correct name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool CheckValidFile(string fileName) {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1) {
                return false;
            }
            return true;
        }
    }
}

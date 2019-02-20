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
using System.IO;
using System.Media;
using System.Windows.Forms;
using eDream.libs;

namespace eDream.GUI
{
    /// <summary>
    ///     Displays a window used to create a new database
    /// </summary>
    public partial class NewFileBox : Form
    {
        public enum CreateFileAction
        {
            Cancel,
            Created
        }

        private const string Extension = ".xml";

        private string _folder;
        private string _newFile = "my_dreams.xml";

        public NewFileBox()
        {
            InitializeComponent();
            _folder = Application.StartupPath;
            chooseFolderDialog.Description = "Choose or create a new folder";
            SetFolderText(_folder);
            SetNewFileText(_newFile);
        }

        public CreateFileAction Action { get; private set; } = CreateFileAction.Cancel;

        public string File => _folder + "\\" + _newFile;

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Action = CreateFileAction.Cancel;
            Close();
        }


        private void SetFolderText(string text)
        {
            folderText.Text = text;
        }

        private void SetNewFileText(string text)
        {
            newFileText.Text = text;
        }

        private void ChooseFolderButton_Click(object sender, EventArgs e)
        {
            chooseFolderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            chooseFolderDialog.SelectedPath = Application.StartupPath;
            if (chooseFolderDialog.ShowDialog() != DialogResult.OK) return;

            _folder = chooseFolderDialog.SelectedPath;
            SetFolderText(_folder);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            _newFile = newFileText.Text;
            if (_newFile.Length < 4 ||
                string.Compare(_newFile.Substring(_newFile.Length - 4), Extension,
                    true) != 0)
                _newFile = _newFile + Extension;
            // Check if file name is valid
            if (!CheckValidFile(_newFile))
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("The file contains illegal characters", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Action = CreateFileAction.Cancel;
                return;
            }

            // If file already exists, prompt to ask if user wants to replace it
            if (System.IO.File.Exists(_folder + "\\" + _newFile))
            {
                SystemSounds.Beep.Play();
                var res = MessageBox.Show(this, _newFile +
                                                " already exists. Do you wish to overwrite it?",
                    "Overwrite file?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation);
                if (res == DialogResult.No) return;
            }

            var writer = new XMLWriter();
            if (writer.CreateFile(_folder + "\\" + _newFile))
            {
                Action = CreateFileAction.Created;
                Close();
            }
        }
        private static bool CheckValidFile(string fileName)
        {
            return fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        }
    }
}
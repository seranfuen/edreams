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
using System.Windows.Forms;

namespace eDream.GUI
{
    public partial class NewFileBox : Form
    {
        public enum CreateFileAction
        {
            Cancel,
            Created
        }

        private readonly NewFileViewModel _viewModel;

        public NewFileBox()
        {
            InitializeComponent();
            _viewModel = new NewFileViewModel();
        }

        public CreateFileAction Action { get; private set; } = CreateFileAction.Cancel;

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Action = CreateFileAction.Cancel;
            Close();
        }

        private void ChooseFolderButton_Click(object sender, EventArgs e)
        {
            ChooseFolderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            ChooseFolderDialog.SelectedPath = Application.StartupPath;
            if (ChooseFolderDialog.ShowDialog() != DialogResult.OK) return;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (!_viewModel.IsValid)
            {
                MessageBox.Show("The file contains illegal characters", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Action = CreateFileAction.Cancel;
            }

            //// If file already exists, prompt to ask if user wants to replace it
            //if (System.IO.File.Exists(_folder + "\\" + _newFile))
            //{
            //    SystemSounds.Beep.Play();
            //    var res = MessageBox.Show(this, _newFile +
            //                                    " already exists. Do you wish to overwrite it?",
            //        "Overwrite file?", MessageBoxButtons.YesNo,
            //        MessageBoxIcon.Exclamation);
            //    if (res == DialogResult.No) return;
            //}

            //var writer = new XMLWriter();
            //if (writer.CreateFile(_folder + "\\" + _newFile))
            //{
            //    Action = CreateFileAction.Created;
            //    Close();
            //}
        }
    }
}
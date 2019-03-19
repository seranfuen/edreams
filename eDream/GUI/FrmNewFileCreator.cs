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
using System.Windows.Forms;
using eDream.libs;

namespace eDream.GUI
{
    public partial class FrmNewFileCreator : Form
    {
        public enum CreateFileAction
        {
            Cancel,
            Created
        }

        private readonly NewFileViewModel _viewModel;

        public FrmNewFileCreator(IDreamFileService dreamFileService)
        {
            InitializeComponent();
            _viewModel = new NewFileViewModel(dreamFileService);
            BindingSource.DataSource = _viewModel;
        }

        public CreateFileAction Action { get; private set; } = CreateFileAction.Cancel;
        public string ChosenPath => _viewModel.FilePath;

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
                MessageBox.Show(GuiStrings.NewFileBox_CreateButton_FileNameValidationErrorMessage,
                    GuiStrings.NewFileBox_CreateButton_FileNameValidationErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Action = CreateFileAction.Cancel;
            }

            if (_viewModel.FileAlreadyExists)
            {
                var res = MessageBox.Show(this,
                    string.Format(GuiStrings.NewFileBox_CreateButton_FileAlreadyExistsMessage, _viewModel.FilePath),
                    GuiStrings.NewFileBox_CreateButton_FileAlreadyExistsTitle, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation);
                if (res == DialogResult.No) return;
            }

            _viewModel.CreateNewFile();
            Action = CreateFileAction.Created;
            Close();
        }
    }
}
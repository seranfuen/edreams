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
using System.Windows.Forms;
using eDream.libs;
using eDream.program;

namespace eDream.GUI
{
    internal partial class FrmDreamEntry : Form
    {
        private readonly DreamEntry _editEntry;

        private readonly IList<TagStatistic> _tagStatistics;
        private readonly DreamEntryViewModel _viewModel;

        private FrmTagWizard _tagWiz;

        public FrmDreamEntry(IList<TagStatistic> tagStatistics)
        {
            InitializeComponent();
            SetUpForm();
            _tagStatistics = tagStatistics;
            _viewModel = DreamEntryViewModel.ForNewDream();
            BindingSource.DataSource = _viewModel;
        }

        public FrmDreamEntry(DreamEntry editEntry, IList<TagStatistic> tagStatistics)
        {
            InitializeComponent();
            SetUpForm();
            _tagStatistics = tagStatistics;
            _viewModel = DreamEntryViewModel.FromExistingEntry(editEntry);
            BindingSource.DataSource = _viewModel;
            CmdSave.Click -= AddEntryButton_Click;
            CmdSave.Click += SaveEdit;
            _editEntry = editEntry;
            LoadEditEntry();
        }

        public DreamEntry NewEntry { get; private set; }

        public bool CreatedEntry { get; private set; }

        private void AddEntryButton_Click(object sender, EventArgs e)
        {
            if (!_viewModel.HasTextOrTags)
            {
                ShowNoTextOrTagsError();
                return;
            }

            NewEntry = _viewModel.ToDreamEntry();
            CreatedEntry = true;
            Dispose();
        }

        private void AddTagButton_Click(object sender, EventArgs e)
        {
            _tagWiz = new FrmTagWizard(_tagStatistics, DreamTagParser.ParseStringToDreamTags(_viewModel.Tags));
            _tagWiz.ShowDialog();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            CloseTagWizard();
            Dispose();
        }

        private void CloseTagWizard()
        {
            _tagWiz?.Dispose();
        }

        private void LoadEditEntry()
        {
            DreamTextBox.Text = _editEntry.Text;
            TagsBox.Text = _editEntry.GetTagString();
            DreamDatePicker.Text = _editEntry.GetDateAsStr();
        }

        private void NotifyTagW(object sender, EventArgs e)
        {
            if (_tagWiz == null || _tagWiz.Visible == false) return;
            if (TagsBox.Text != _tagWiz.TagText) _tagWiz.TagText = TagsBox.Text;
        }

        private static void PreventChildClose(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None) e.Cancel = true;
        }

        private void SaveEdit(object sender, EventArgs e)
        {
            if (!_viewModel.HasTextOrTags)
            {
                ShowNoTextOrTagsError();
                return;
            }

            _editEntry.Text = DreamTextBox.Text;
            _editEntry.SetTags(TagsBox.Text);
            _editEntry.Date = DreamDatePicker.Value;
            CreatedEntry = true;
            Dispose();
        }

        private void SendForm(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Control | Keys.Enter:
                    e.SuppressKeyPress = true;
                    CmdSave.PerformClick();
                    break;
                case Keys.Escape when sender.Equals(this):
                    e.SuppressKeyPress = true;
                    CmdCancel.PerformClick();
                    break;
            }
        }

        private void SetUpForm()
        {
            FormClosing += PreventChildClose;
            StartPosition = FormStartPosition.CenterParent;
            TagsBox.TextChanged += NotifyTagW;
            KeyPreview = true;
            KeyDown += SendForm;
        }

        private static void ShowNoTextOrTagsError()
        {
            MessageBox.Show(GuiStrings.NewEntryForm_SaveEdit_NoTextOrTagsMessage,
                GuiStrings.NewEntryForm_SaveEdit_NoTextOrTagsTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
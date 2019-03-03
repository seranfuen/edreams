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
    internal partial class SearchForm : Form
    {
        public delegate void SearchEvent(object sender, EventArgs arguments);


        public enum ESearchType
        {
            TextSearch,
            TagsSearch,
            DateSearch
        }

        private readonly List<DreamEntry> _dreamEntries;

        private readonly DreamSearchUtils _searchUtils = new DreamSearchUtils();

        public EventHandler OnClear;

        public SearchEvent OnSearchCompleted;

        public SearchForm(List<DreamEntry> entries)
        {
            InitializeComponent();
            _dreamEntries = entries;
            StartPosition =
                FormStartPosition.CenterScreen;
            textSearchFindButton.Enabled = false;
            findTagsButton.Enabled = false;
            andRadio.Checked = true;
            clearTagsButton.Enabled = false;
            textClearButton.Enabled = false;
            fromTimePicker.Value = DateTime.Now;
            toTimePicker.Value = DateTime.Now;
            FormClosing += PreventDestroy;
            textSearchBox.TextChanged += TextSearchBoxChanged;
            tagsTextBox.TextChanged += TextSearchBoxChanged;
            textSearchBox.KeyDown += EnterKeyPressed;
            tagsTextBox.KeyDown += EnterKeyPressed;
            clearTagsButton.Click += SendClear;
            textClearButton.Click += SendClear;
            dateClearButton.Click += SendClear;
            KeyDown += CloseWindow;
        }

        public List<DreamEntry> Results { get; private set; } = new List<DreamEntry>();

        public ESearchType SearchType { get; private set; }

        public string LastSearchText { get; private set; }

        private void TextSearchBoxChanged(object sender, EventArgs args)
        {
            var box = (TextBox) sender;

            if (box.Equals(textSearchBox))
                textSearchFindButton.Enabled = !string.IsNullOrWhiteSpace(box.Text);
            else if (box.Equals(tagsTextBox)) findTagsButton.Enabled = !string.IsNullOrWhiteSpace(box.Text);
        }

        private void textSearchFindButton_Click(object sender, EventArgs e)
        {
            Results = _searchUtils.SearchEntriesText(_dreamEntries,
                textSearchBox.Text);
            LastSearchText = textSearchBox.Text;
            SearchType = ESearchType.TextSearch;
            if (OnSearchCompleted == null) return;

            OnSearchCompleted(this, new EventArgs());
            clearTagsButton.Enabled = true;
            textClearButton.Enabled = true;
            dateClearButton.Enabled = true;
        }


        private void FindTagsButton_Click(object sender, EventArgs e)
        {
            var tags =
                tagsTextBox.Text.Split(DreamTagTokens.MainTagSeparator);
            var searchType = orRadio.Checked ? DreamSearchUtils.TagSearchType.ORSearch : DreamSearchUtils.TagSearchType.ANDSearch;
            Results = _searchUtils.SearchEntriesTags(_dreamEntries, tags,
                checkChildTags.Checked, searchType);
            LastSearchText = string.Join(", ", tags);
            SearchType = ESearchType.TagsSearch;
            if (OnSearchCompleted == null) return;

            OnSearchCompleted(this, new EventArgs());
            clearTagsButton.Enabled = true;
            textClearButton.Enabled = true;
            dateClearButton.Enabled = true;
        }


        private void EnterKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var activePage = searchTabs.SelectedTab;
            if (activePage == textSearchPage)
                textSearchFindButton.PerformClick();
            else if (activePage == tagsSearchPage) findTagsButton.PerformClick();
            e.Handled = true;
        }

        private void CloseWindow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void PreventDestroy(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall ||
                e.CloseReason == CloseReason.FormOwnerClosing)
                return;
            e.Cancel = true;
            Visible = false;
        }

        private void SendClear(object sender, EventArgs e)
        {
            clearTagsButton.Enabled = false;
            textClearButton.Enabled = false;
            dateClearButton.Enabled = false;
            OnClear?.Invoke(this, e);
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                toTimePicker.Enabled = false;
                fromDateLabel.Text = "On this date";
            }
            else
            {
                toTimePicker.Enabled = true;
                fromDateLabel.Text = "From";
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SearchType = ESearchType.DateSearch;
            if (checkBox2.Checked)
                Results = _searchUtils.SearchEntriesOnDate(_dreamEntries,
                    fromTimePicker.Value);
            else
                Results = _searchUtils.SearchEntriesDateRange(_dreamEntries,
                    fromTimePicker.Value, toTimePicker.Value);
            if (OnSearchCompleted != null)
            {
                OnSearchCompleted(this, new EventArgs());
                clearTagsButton.Enabled = true;
                textClearButton.Enabled = true;
                dateClearButton.Enabled = true;
            }
        }
    }
}
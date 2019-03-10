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
using eDream.Annotations;
using eDream.libs;
using eDream.program;

namespace eDream.GUI
{
    internal partial class FrmSearchForm : Form
    {
        public enum ESearchType
        {
            TextSearch,
            TagsSearch,
            DateSearch
        }

        private readonly DreamSearchUtils _searchUtils = new DreamSearchUtils();
        private readonly IDreamEntryProvider _provider;

        public EventHandler<SearchPerformedEventArgs> SearchCompleted;

        public FrmSearchForm([NotNull] IDreamEntryProvider provider)
        {
            InitializeComponent();
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
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

        public IEnumerable<DreamEntry> Results { get; private set; } = new List<DreamEntry>();

        public ESearchType SearchType { get; private set; }

        public string LastSearchText { get; private set; }

        private void Button2_Click(object sender, EventArgs e)
        {
            SearchType = ESearchType.DateSearch;
            if (checkBox2.Checked)
                Results = _searchUtils.SearchEntriesOnDate(_provider.DreamEntries,
                    fromTimePicker.Value);
            else
                Results = _searchUtils.SearchEntriesDateRange(_provider.DreamEntries,
                    fromTimePicker.Value, toTimePicker.Value);

            OnSearchCompleted();
            clearTagsButton.Enabled = true;
            textClearButton.Enabled = true;
            dateClearButton.Enabled = true;
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

        private void CloseWindow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
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


        private void FindTagsButton_Click(object sender, EventArgs e)
        {
            var tags =
                tagsTextBox.Text.Split(DreamTagTokens.MainTagSeparator);
            var searchType = orRadio.Checked
                ? DreamSearchUtils.TagSearchType.ORSearch
                : DreamSearchUtils.TagSearchType.ANDSearch;
            Results = _searchUtils.SearchEntriesTags(_provider.DreamEntries, tags,
                checkChildTags.Checked, searchType);
            LastSearchText = string.Join(", ", tags);
            SearchType = ESearchType.TagsSearch;

            OnSearchCompleted();
            clearTagsButton.Enabled = true;
            textClearButton.Enabled = true;
            dateClearButton.Enabled = true;
        }

        private void OnSearchCompleted()
        {
            SearchCompleted?.Invoke(this, new SearchPerformedEventArgs(Results));
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
            SearchCompleted?.Invoke(this, new SearchPerformedEventArgs(null));
        }

        private void TextSearchBoxChanged(object sender, EventArgs args)
        {
            var box = (TextBox) sender;

            if (box.Equals(textSearchBox))
                textSearchFindButton.Enabled = !string.IsNullOrWhiteSpace(box.Text);
            else if (box.Equals(tagsTextBox)) findTagsButton.Enabled = !string.IsNullOrWhiteSpace(box.Text);
        }

        private void TextSearchFindButton_Click(object sender, EventArgs e)
        {
            Results = _searchUtils.SearchEntriesText(_provider.DreamEntries, textSearchBox.Text);
            LastSearchText = textSearchBox.Text;
            SearchType = ESearchType.TextSearch;

            OnSearchCompleted();
            clearTagsButton.Enabled = true;
            textClearButton.Enabled = true;
            dateClearButton.Enabled = true;
        }
    }
}
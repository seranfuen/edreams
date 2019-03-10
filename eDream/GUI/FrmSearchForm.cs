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
    public partial class FrmSearchForm : Form
    {
        public enum ESearchType
        {
            TextSearch,
            TagsSearch,
            DateSearch
        }

        private readonly IDreamEntryProvider _provider;

        public EventHandler<SearchPerformedEventArgs> SearchCompleted;

        public FrmSearchForm([NotNull] IDreamEntryProvider provider)
        {
            InitializeComponent();
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            StartPosition =
                FormStartPosition.CenterScreen;
            TextSearchFindButton.Enabled = false;
            FindTagsButton.Enabled = false;
            andRadio.Checked = true;
            ClearTagsButton.Enabled = false;
            TextClearButton.Enabled = false;
            FromTimePicker.Value = DateTime.Now;
            ToTimePicker.Value = DateTime.Now;
            FormClosing += PreventDestroy;
            TextSearchBox.TextChanged += TextSearchBoxChanged;
            tagsTextBox.TextChanged += TextSearchBoxChanged;
            TextSearchBox.KeyDown += EnterKeyPressed;
            tagsTextBox.KeyDown += EnterKeyPressed;
            ClearTagsButton.Click += SendClear;
            TextClearButton.Click += SendClear;
            DateClearButton.Click += SendClear;
            KeyDown += CloseWindow;
        }

        public IEnumerable<DreamEntry> Results { get; private set; } = new List<DreamEntry>();

        public ESearchType SearchType { get; private set; }

        public string LastSearchText { get; private set; }

        private void CloseWindow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }


        private void EnterKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var activePage = searchTabs.SelectedTab;
            if (activePage == textSearchPage)
                TextSearchFindButton.PerformClick();
            else if (activePage == tagsSearchPage) FindTagsButton.PerformClick();
            e.Handled = true;
        }

        private void FindDateButton_Click(object sender, EventArgs e)
        {
            var diarySearch = GetDreamDiarySearch();
            SearchType = ESearchType.DateSearch;
            if (SingleDayCheckBox.Checked)
                Results = diarySearch.SearchEntriesOnDate(FromTimePicker.Value);
            else
                Results = diarySearch.SearchEntriesBetweenDates(FromTimePicker.Value, ToTimePicker.Value);

            OnSearchCompleted();
            ClearTagsButton.Enabled = true;
            TextClearButton.Enabled = true;
            DateClearButton.Enabled = true;
        }


        private void FindTagsButton_Click(object sender, EventArgs e)
        {
            var tags =
                tagsTextBox.Text.Split(DreamTagTokens.MainTagSeparator);
            var searchType = orRadio.Checked
                ? TagSearchType.OrSearch
                : TagSearchType.AndSearch;
            var diarySearch = GetDreamDiarySearch();
            Results = diarySearch.SearchEntriesTags(_provider.DreamEntries, tags,
                checkChildTags.Checked, searchType);
            LastSearchText = string.Join(", ", tags);
            SearchType = ESearchType.TagsSearch;

            OnSearchCompleted();
            ClearTagsButton.Enabled = true;
            TextClearButton.Enabled = true;
            DateClearButton.Enabled = true;
        }

        private DreamDiarySearch GetDreamDiarySearch()
        {
            return new DreamDiarySearch(_provider.DreamEntries);
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
            ClearTagsButton.Enabled = false;
            TextClearButton.Enabled = false;
            DateClearButton.Enabled = false;
            SearchCompleted?.Invoke(this, new SearchPerformedEventArgs(null));
        }

        private void SingleDayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SingleDayCheckBox.Checked)
            {
                ToTimePicker.Enabled = false;
                fromDateLabel.Text = "On this date";
            }
            else
            {
                ToTimePicker.Enabled = true;
                fromDateLabel.Text = "From";
            }
        }

        private void TextSearchBoxChanged(object sender, EventArgs args)
        {
            var box = (TextBox) sender;

            if (box.Equals(TextSearchBox))
                TextSearchFindButton.Enabled = !string.IsNullOrWhiteSpace(box.Text);
            else if (box.Equals(tagsTextBox)) FindTagsButton.Enabled = !string.IsNullOrWhiteSpace(box.Text);
        }

        private void TextSearchFindButton_Click(object sender, EventArgs e)
        {
            var diarySearch = GetDreamDiarySearch();
            Results = diarySearch.SearchEntriesText(_provider.DreamEntries, TextSearchBox.Text);
            LastSearchText = TextSearchBox.Text;
            SearchType = ESearchType.TextSearch;

            OnSearchCompleted();
            ClearTagsButton.Enabled = true;
            TextClearButton.Enabled = true;
            DateClearButton.Enabled = true;
        }
    }
}
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
using System.Linq;
using System.Windows.Forms;
using eDream.Annotations;
using eDream.libs;
using eDream.program;

namespace eDream.GUI
{
    internal partial class FrmTagWizard : Form
    {
        private readonly IList<TagStatistic> _tagStatistics;
        private readonly TagWizardViewModel _viewModel;

        public EventHandler OnTagTextChange;

        public FrmTagWizard([NotNull] IList<TagStatistic> tagStatistics,
            [NotNull] IEnumerable<DreamMainTag> currentTags)
        {
            InitializeComponent();
            if (currentTags == null) throw new ArgumentNullException(nameof(currentTags));
            _tagStatistics = tagStatistics ?? throw new ArgumentNullException(nameof(tagStatistics));
            BindingSource.DataSource = _tagStatistics;
            _viewModel = new TagWizardViewModel(currentTags);
            ViewModelBindingSource.DataSource = _viewModel;

            TagSearch.KeyDown += DetectKey;
            TagsToAddTextBox.TextChanged += EmitTextChanged;

            TagsGrid.DoubleClick += TableDoubleClick;
        }

        public string TagText
        {
            set => TagsToAddTextBox.Text = value;
            get => TagsToAddTextBox.Text;
        }

        private void AddSelectedEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSelectedTags();
        }

        private void AddSelectedTags()
        {
            var selectedRows = GetSelectedRows();
            foreach (var tag in selectedRows) _viewModel.AddTag(tag);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            TagSearch.Text = "";
            ResetInitialData();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DetectKey(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Return) return;

            e.SuppressKeyPress = true;
            SearchButton_Click(sender, new EventArgs());
        }

        private void EmitTextChanged(object sender, EventArgs e)
        {
            OnTagTextChange?.Invoke(this, e);
        }

        private IEnumerable<TagStatistic> GetSelectedRows()
        {
            var rows = new List<TagStatistic>();
            foreach (DataGridViewRow row in TagsGrid.SelectedRows) rows.Add((TagStatistic) row.DataBoundItem);

            return rows;
        }

        private static bool IsContainedBy(IDreamTag tagStatistic, IEnumerable<IDreamTag> dreamTags)
        {
            return dreamTags.Any(tag => tag.Tag == tagStatistic.Tag && tag.ParentTag == tagStatistic.ParentTag);
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(TagSearch.Text)) ResetInitialData();

            var searcher = new DreamTagSearch(_tagStatistics);
            SetSearchResult(searcher.SearchForTags(TagSearch.Text));
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            _viewModel.Reset();
        }

        private void ResetInitialData()
        {
            BindingSource.DataSource = _tagStatistics;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void SetSearchResult(IEnumerable<IDreamTag> dreamTags)
        {
            BindingSource.DataSource = _tagStatistics.Where(tag => IsContainedBy(tag, dreamTags)).ToList();
        }

        private void TableDoubleClick(object sender, EventArgs e)
        {
            AddSelectedTags();
        }

        private void TagSearch_TextChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }
    }
}
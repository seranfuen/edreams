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
    internal partial class FrmTagWizard : Form
    {
        private readonly TagWizardViewModel _viewModel;


        public FrmTagWizard([NotNull] IList<TagStatistic> tagStatistics,
            [NotNull] IEnumerable<DreamMainTag> currentTags)
        {
            InitializeComponent();
            if (currentTags == null) throw new ArgumentNullException(nameof(currentTags));
            _viewModel = new TagWizardViewModel(currentTags, tagStatistics);
            ViewModelBindingSource.DataSource = _viewModel;

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
            _viewModel.SearchTerm = "";
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private IEnumerable<TagStatistic> GetSelectedRows()
        {
            var rows = new List<TagStatistic>();
            foreach (DataGridViewRow row in TagsGrid.SelectedRows) rows.Add((TagStatistic) row.DataBoundItem);

            return rows;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            _viewModel.Reset();
        }

        private void TableDoubleClick(object sender, EventArgs e)
        {
            AddSelectedTags();
        }

        private void TagSearch_TextChanged(object sender, EventArgs e)
        {
            TagSearch.DataBindings[0].WriteValue();
        }
    }
}
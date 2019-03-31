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
using eDream.Annotations;

namespace eDream.GUI
{
    public partial class FrmDreamStatistics : Form
    {
        public FrmDreamStatistics([NotNull] DreamStatisticsViewModel viewModel)
        {
            InitializeComponent();
            ViewModelBindingSource.DataSource = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SearchTagsTextBox_TextChanged(object sender, EventArgs e)
        {
            SearchTagsTextBox.DataBindings[0].WriteValue();
        }
    }
}
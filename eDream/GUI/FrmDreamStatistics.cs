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
using eDream.libs;

namespace eDream.GUI
{
    public partial class FrmDreamStatistics : Form
    {
        public FrmDreamStatistics(DreamTagStatisticsGenerator statistics)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            SetTableData(statistics.GetStatistics());
            DisplayStatisticsLabel(statistics.TotalEntries, statistics.TotalDays);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DisplayStatisticsLabel(int totalEntries, int totalDays)
        {
            var str = $"There are {totalEntries} dreams in {totalDays} days";
            if (totalDays > 0)
                str += $" ({totalEntries / (float) totalDays:0.00} dreams/day)";
            totalDreamsLabel.Text = str;
        }

        private void SetTableData(IEnumerable<TagStatistic> data)
        {
            var tagStatisticsViewModels = data.ToList();
            if (!tagStatisticsViewModels.Any())
            {
                SetTableDefaultData();
                return;
            }

            BindingSource.DataSource = tagStatisticsViewModels;
            StatsTable.Enabled = true;
        }


        private void SetTableDefaultData()
        {
            StatsTable.DataSource = null;
            StatsTable.Enabled = false;
            BindingSource.DataSource = null;
        }
    }
}
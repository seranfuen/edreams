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
    public partial class CtrEntryViewer : UserControl
    {
        public CtrEntryViewer()
        {
            InitializeComponent();
        }

        public void SetViewModel([NotNull] EntryViewerModel viewModel)
        {
            BindingSource.DataSource = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }


        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //SystemSounds.Exclamation.Play();
            //if (MessageBox.Show("Do you really want to delete this entry?",
            //        "Delete entry?", MessageBoxButtons.YesNo,
            //        MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    theEntry.ToDelete = true;
            //    Dispose();
            //    parent.SaveXmlFile();
            //    parent.RefreshEntries();
            //}
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            //var editEntry = new NewEntryForm(theEntry, parent.TagStatistics);
            //editEntry.ShowDialog();
            //if (editEntry.CreatedEntry)
            //{
            //    parent.SaveXmlFile();
            //}
        }
    }
}
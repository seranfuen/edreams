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
    along with FrmMain.  If not, see <http://www.gnu.org/licenses/>.]
****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using eDream.Annotations;
using eDream.GUI;
using eDream.libs;
using eDream.program;

namespace eDream
{
    internal partial class FrmMain : Form
    {
        private readonly IDreamDiaryBus _dreamDiaryBus;
        private readonly IEdreamsFactory _factory;


        private readonly IDreamDiaryViewModel _viewModel;

        private List<DreamDayEntry> _currentDayList;

        public FrmMain([NotNull] IEdreamsFactory factory)
        {
            InitializeComponent();
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _viewModel = factory.CreateDreamDiaryViewModel();
            BindingSource.DataSource = _viewModel;
            _dreamDiaryBus = new DreamDiaryBus(_viewModel, factory.CreateDreamReaderWriterFactory());
            _dreamDiaryBus.DiaryPersisted += (s, e) => RefreshEntries();
            _dreamDiaryBus.SearchPerformed += (s, e) => RefreshEntries();
            InitializeInterface();
        }

        public void SetStatusBarMsg(string text)
        {
            entriesStatsStatus.Text = text;
        }

        public void ShowErrorMessage(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmAboutBox().ShowDialog();
        }

        private void AddEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewEntry();
        }

        private void AddNewEntry()
        {
            _dreamDiaryBus.AddNewEntry();
        }

        private void ClearRightPanel()
        {
            TableLayoutPanel.RowCount = 0;
            TableLayoutPanel.Controls.Clear();
        }

        private void CloseDiary()
        {
            _viewModel.CurrentDatabasePath = string.Empty;
            SetUnloadedState();
            DreamListBox.Enabled = false;
            DreamListBox.DataSource = null;
            SetStatusBarMsg(GuiStrings.StatusBar_NoDreamDiaryLoaded);
            Text = _viewModel.FormText;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseDiary();
        }

        private void CreateNewDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewDreamDatabase();
        }

        private void CreateNewDreamDatabase()
        {
            var newFile = _factory.CreateNewFileCreator();
            newFile.ShowDialog();
            if (newFile.Action != FrmNewFileCreator.CreateFileAction.Created) return;

            CloseToolStripMenuItem.PerformClick();
            _viewModel.CurrentDatabasePath = newFile.ChosenPath;
            LoadDiary();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void FocusEntryPanel(object sender, MouseEventArgs e)
        {
            TableLayoutPanel.Focus();
        }

        private void FocusList(object sender, MouseEventArgs e)
        {
            DreamListBox.Focus();
        }

        private DreamStatisticsViewModel GetDreamStatisticsViewModel()
        {
            var dreamTagStatisticsGenerator = _viewModel.GetDreamTagStatistics();
            return new DreamStatisticsViewModel(dreamTagStatisticsGenerator.GetStatistics(),
                new DreamCountSummary(dreamTagStatisticsGenerator.TotalEntries,
                    dreamTagStatisticsGenerator.TotalDays));
        }

        private void InitializeInterface()
        {
            SetUnloadedState();

            DreamListBox.SelectedIndexChanged += (sender, args) => LoadCurrentlySelectedDay();
            DreamListBox.MouseWheel += ScrollList;
            DreamListBox.MouseClick += FocusList;
            TableLayoutPanel.MouseClick +=
                FocusEntryPanel;

            _viewModel.PersistenceFailed += (s, e) => OnPersistenceFailed();
            _viewModel.PersistenceSucceeded += (s, e) => OnPersistenceSucceeded();
            _viewModel.LoadingSucceeded += (s, e) => RefreshEntries();
            _viewModel.LoadingFailed += (s, e) => ShowLoadingErrorMessage();
            Shown += LoadLastDatabase;
        }

        private void LoadCurrentlySelectedDay()
        {
            if (_currentDayList == null || _currentDayList.Count <= 0) return;
            if (DreamListBox.SelectedIndex < 0 || DreamListBox.SelectedIndex >= _currentDayList.Count) return;

            LoadDayEntries(_currentDayList[DreamListBox.SelectedIndex]);
        }


        private void LoadDayEntries(DreamDayEntry day)
        {
            TableLayoutPanel.Controls.Clear();
            var entries = day.DreamEntries;
            if (!entries.Any()) return;


            TableLayoutPanel.Visible = false;
            var entryCount = 1;
            foreach (var entry in entries)
            {
                if (entry.ToDelete) continue;
                var newEntry = new CtrEntryViewer();
                newEntry.SetViewModel(EntryViewerModel.FromEntry(entry, entryCount++,
                    _dreamDiaryBus));
                TableLayoutPanel.Controls.Add(newEntry);
            }


            for (var i = 0; i < TableLayoutPanel.RowStyles.Count; i++)
                TableLayoutPanel.RowStyles[i].SizeType = SizeType.AutoSize;

            try
            {
                for (var i = 0; i < TableLayoutPanel.Controls.Count; i++)
                for (var j = 0; j < TableLayoutPanel.Controls[i].Controls.Count; j++)
                {
                    TableLayoutPanel.Controls[i].Controls[j].MouseClick +=
                        FocusEntryPanel;
                    for (var k = 0; k < TableLayoutPanel.Controls[i].Controls[j].Controls.Count; k++)
                    {
                        TableLayoutPanel.Controls[i].Controls[j].Controls[k].MouseClick +=
                            FocusEntryPanel;
                        for (var l = 0; l < TableLayoutPanel.Controls[i].Controls[j].Controls[k].Controls.Count; l++)
                            TableLayoutPanel.Controls[i].Controls[j].Controls[k].Controls[l].MouseClick +=
                                FocusEntryPanel; //This is the panel where we click on the most
                    }
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                TableLayoutPanel.Visible = true;
            }
        }


        private void LoadDiary()
        {
            SetBusyStatus();
            _viewModel.LoadDiary();
        }


        private void LoadEntriesToList(IReadOnlyCollection<DreamDayEntry> entries)
        {
            TableLayoutPanel.Visible = false;
            DreamListBox.Enabled = false;
            if (entries == null || entries.Count == 0)
            {
                ClearRightPanel();
                return;
            }

            DreamListBox.Enabled = true;
            var sortedEntries = entries.OrderBy(day => day.Date);

            _currentDayList = sortedEntries.ToList();

            DreamListBox.DataSource = _currentDayList;
            DreamListBox.SelectedIndex = Math.Max(0, entries.Count - 1);
            LoadCurrentlySelectedDay();
            TableLayoutPanel.Visible = true;
            SetStatusBarStats();
        }

        private void LoadLastDatabase(object sender, EventArgs e)
        {
            _viewModel.LoadLastDiary();
        }

        private void LoadRecentDatabase(object sender, EventArgs a)
        {
            _viewModel.CurrentDatabasePath = ((RecentlyOpenedMenuItem) sender).FilePath;
            LoadDiary();
        }

        private void OnPersistenceFailed()
        {
            BeginInvoke((Action) (() =>
            {
                ShowErrorMessage(GuiStrings.SavingDiaryFailed_Title,
                    string.Format(GuiStrings.SavingDiaryFailed_Message, _viewModel.CurrentDatabasePath));
                SetActiveStatus();
                SetLoadedState();
            }));
        }

        private void OnPersistenceSucceeded()
        {
            SetActiveStatus();
            SetLoadedState();
        }

        private void OpenDatabaseToolStripMenuItem_Click(object sender,
            EventArgs e)
        {
            var result = OpenFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;

            _viewModel.CurrentDatabasePath = OpenFileDialog.FileName;
            LoadDiary();
        }

        private void PersistDiary()
        {
            SetBusyStatus();
            _viewModel.Persist();
        }


        private void RefreshEntries()
        {
            SetActiveStatus();
            LoadEntriesToList(_viewModel.GetDayList());
            SetCurrentFile(_viewModel.CurrentDatabasePath);
            SetLoadedState();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resultSave = saveFileDialog1.ShowDialog();
            if (resultSave != DialogResult.OK) return;
            _viewModel.CurrentDatabasePath = saveFileDialog1.FileName;
            PersistDiary();
            saveFileDialog1.FileName = "";
        }

        private void ScrollList(object sender, MouseEventArgs e)
        {
            var selected = DreamListBox.SelectedIndex;
            var ee = (HandledMouseEventArgs) e;

            if (ee.Delta < 0)
            {
                if (ee.Delta < -120 && selected < _currentDayList.Count - 2)
                    DreamListBox.SelectedIndex = selected + 2;
                else if (ee.Delta < -600 && selected < _currentDayList.Count - 3)
                    DreamListBox.SelectedIndex = selected + 3;
                else if (selected < _currentDayList.Count - 1) DreamListBox.SelectedIndex = selected + 1;
            }

            else if (ee.Delta > 0)
            {
                if (ee.Delta > 120 && selected > 1)
                    DreamListBox.SelectedIndex = selected - 2;
                else if (ee.Delta > 600 && selected > 2)
                    DreamListBox.SelectedIndex = selected - 3;
                else if (selected > 0) DreamListBox.SelectedIndex = selected - 1;
            }


            ee.Handled = true;
        }

        private void SearchStripButton_Click(object sender, EventArgs e)
        {
            _dreamDiaryBus.OpenSearchDialog();
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dreamDiaryBus.OpenSearchDialog();
        }


        private void SetActiveStatus()
        {
            Enabled = true;
            progressBar1.Visible = false;
            Cursor = Cursors.Default;
        }

        private void SetBusyStatus()
        {
            Enabled = false;
            progressBar1.Visible = true;
            Cursor = Cursors.WaitCursor;
        }

        private void SetCurrentFile(string file)
        {
            _viewModel.CurrentDatabasePath = file;
            SetRecentFilesMenu();
        }

        private void SetLoadedState()
        {
            entryToolStripMenuItem.Enabled = true;
            SaveAsToolStripMenuItem.Enabled = true;
            CloseToolStripMenuItem.Enabled = true;
            AddEntryToolStripMenuItem.Enabled = true;
            ImportFromAnotherDiaryToolStripMenuItem.Enabled = true;
            toolStripAdd.Enabled = true;
            toolStripStats.Enabled = true;
            ClearRightPanel();
            LoadEntriesToList(_viewModel.DreamDays);
            SetStatusBarStats();
            SearchToolStripMenuItem.Enabled = true;
            Text = _viewModel.FormText;
        }


        private void SetRecentFilesMenu()
        {
            menuRecent.Enabled = false;

            var paths = _viewModel.GetRecentlyOpenedDiaryPaths().ToList();
            if (!paths.Any()) return;
            menuRecent.Enabled = true;
            menuRecent.DropDownItems.Clear();
            foreach (var path in paths)
            {
                var newI = new RecentlyOpenedMenuItem(path, Path.GetFileName(path));
                newI.Click += LoadRecentDatabase;
                menuRecent.DropDownItems.Add(newI);
            }
        }

        private void SetStatusBarStats()
        {
            SetStatusBarMsg(_viewModel.StatusBarMessage);
        }

        private void SetUnloadedState()
        {
            entryToolStripMenuItem.Enabled = false;
            SaveAsToolStripMenuItem.Enabled = false;
            CloseToolStripMenuItem.Enabled = false;
            AddEntryToolStripMenuItem.Enabled = false;
            ImportFromAnotherDiaryToolStripMenuItem.Enabled = false;
            toolStripAdd.Enabled = false;
            toolStripStats.Enabled = false;
            SearchToolStripMenuItem.Enabled = false;
            ClearRightPanel();
        }

        private void ShowLoadingErrorMessage()
        {
            SetActiveStatus();

            ShowErrorMessage(GuiStrings.FrmMain_ShowLoadingErrorTitle,
                string.Format(GuiStrings.FrmMain_ShowLoadingErrorMessage, _viewModel.CurrentDatabasePath));
        }

        private void ShowStatistics()
        {
            new FrmDreamStatistics(GetDreamStatisticsViewModel()).Show();
        }

        private void StatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowStatistics();
        }

        private void ToolStripAdd_Click(object sender, EventArgs e)
        {
            AddNewEntry();
        }

        private void ToolStripStats_Click(object sender, EventArgs e)
        {
            ShowStatistics();
        }

        private void ImportFromAnotherDiaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                _dreamDiaryBus.ImportDiary(OpenFileDialog.FileName);
            }
        }
    }
}
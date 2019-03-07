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
using System.Media;
using System.Windows.Forms;
using eDream.GUI;
using eDream.libs;
using eDream.program;
using EvilTools;

namespace eDream
{
    internal partial class FrmMain : Form
    {
        private readonly Debug _debug = new Debug(Debug.DebugParameters.ToConsoleAndFile);
        private readonly IDreamDiaryBus _dreamDiaryBus;


        private readonly DreamDiaryViewModel _viewModel;

        private List<DreamDayEntry> _currentDayList;

        private List<DreamDayEntry> _dayList;

        private Debug.OnDebugIncidence _debugIncidence;

        private List<DreamEntry> _dreamEntries;

        private SearchForm _searchWindow;

        public FrmMain()
        {
            InitializeComponent();
            _viewModel = new DreamDiaryViewModel(InjectionKernel.Get<IDreamDiaryPersistenceService>(),
                InjectionKernel.Get<IDreamDiaryPaths>());
            BindingSource.DataSource = _viewModel;
            _dreamDiaryBus = new DreamDiaryBus(_viewModel);
            _dreamDiaryBus.DiaryPersisted += (s, e) => RefreshEntries();
            InitializeInterface();
        }

        public void SetStatusBarMsg(string text)
        {
            entriesStatsStatus.Text = text;
        }

        public void ShowCurrentDay(object sender, EventArgs e)
        {
            if (_currentDayList == null || _currentDayList.Count <= 0) return;
            if (DreamListBox.SelectedIndex < 0 || DreamListBox.SelectedIndex >= _currentDayList.Count) return;

            LoadDayEntries(_currentDayList[DreamListBox.SelectedIndex]);
        }

        public void ShowErrorMessage(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
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

        private void ClearSearch(object sender, EventArgs e)
        {
            SetFindsMessage("Showing all dreams");
            LoadEntriesToList(_dayList);
            _currentDayList = _dayList;
        }

        private void CloseDiary()
        {
            _viewModel.CurrentDatabasePath = string.Empty;
            _dreamEntries = new List<DreamEntry>();
            _dayList = new List<DreamDayEntry>();
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
            var newFile = new FrmNewFileCreator();
            newFile.ShowDialog();
            if (newFile.Action != FrmNewFileCreator.CreateFileAction.Created) return;

            closeToolStripMenuItem.PerformClick();
            _viewModel.CurrentDatabasePath = newFile.ChosenPath;
            LoadDiary();
        }

        private void EmptyEntryList()
        {
            LoadEntriesToList(null);
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

        private void InitializeInterface()
        {
            SetUnloadedState();

            DreamListBox.SelectedIndexChanged +=
                ShowCurrentDay;
            DreamListBox.MouseWheel += ScrollList;
            DreamListBox.MouseClick += FocusList;
            TableLayoutPanel.MouseClick +=
                FocusEntryPanel;

            _viewModel.PersistenceFailed += (s, e) => OnPersistenceFailed();
            _viewModel.PersistenceSucceeded += (s, e) => OnPersistenceSucceeded();
            _viewModel.LoadingSucceeded += (s, e) => RefreshEntries();
            _viewModel.LoadingFailed += (s, e) => ShowLoadingErrorMessage();
            _debugIncidence += SendToDebugger;
            Shown += LoadLastDatabase;
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
            catch (Exception e)
            {
                ProcessIncidence(e, @"Error setting focus to right panel: when
 setting focus to right panel we loop through all controls in the
 layout panel down to a third level, adding a mouse click handler which is
 supposed to process all clicks on the interface except inside buttons and the
 text box");
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
            ShowCurrentDay(DreamListBox, new EventArgs());
            TableLayoutPanel.Visible = true;
            SetStatusBarStats();
        }

        private void LoadLastDatabase(object sender, EventArgs e)
        {
            _viewModel.LoadLastDiary();
        }

        private void LoadRecentDatabase(object sender, EventArgs a)
        {
            var theSender = (RecentlyOpenedMenuItem) sender;

            _viewModel.CurrentDatabasePath = theSender.FilePath;
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
            var result = openFileDialog1.ShowDialog();
            if (result != DialogResult.OK) return;

            _viewModel.CurrentDatabasePath = openFileDialog1.FileName;
            LoadDiary();
        }

        private void PersistDiary()
        {
            SetBusyStatus();
            _viewModel.Persist();
        }


        private void ProcessIncidence(Exception e, string msg)
        {
            var args = new Debug.DebugArgs
            {
                Message = msg,
                ExceptionMessage = e.Message,
                StackTrace = e.StackTrace
            };
            _debugIncidence?.Invoke(args);
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


        private void SearchPerformed(object sender, EventArgs e)
        {
            var searcher = (SearchForm) sender;
            SystemSounds.Beep.Play();
            LoadEntriesToList(DreamCalendarCreator.GetDreamDayList(searcher.Results)
            );
            string searchType;
            if (searcher.SearchType == SearchForm.ESearchType.TextSearch)
                searchType = "text";
            else if (searcher.SearchType == SearchForm.ESearchType.DateSearch)
                searchType = "date";
            else
                searchType = "tags";
            var entry = "entries";
            if (searcher.Results.Count == 1) entry = "entry";
            var msg = $"Your search for the {searchType} \"{searcher.LastSearchText}\" found" +
                      $" {searcher.Results.Count} {entry}";
            SetFindsMessage(msg);
            Focus();
        }


        private void SearchStripButton_Click(object sender, EventArgs e)
        {
            ShowSearch(this, e);
        }


        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearch(this, e);
        }


        private void SendToDebugger(Debug.DebugArgs args)
        {
            _debug.AcceptDebug(args);
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


        private void SetFindsMessage(string str)
        {
            textBoxFinds.Text = str;
        }

        private void SetLoadedState()
        {
            entryToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            addEntryToolStripMenuItem.Enabled = true;
            toolStripAdd.Enabled = true;
            toolStripStats.Enabled = true;
            ClearRightPanel();
            LoadEntriesToList(_viewModel.DreamDays);
            SetStatusBarStats();
            searchToolStripMenuItem.Enabled = true;
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
            saveAsToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            addEntryToolStripMenuItem.Enabled = false;
            toolStripAdd.Enabled = false;
            toolStripStats.Enabled = false;
            searchToolStripMenuItem.Enabled = false;
            EmptyEntryList();
            ClearRightPanel();
        }

        private void ShowLoadingErrorMessage()
        {
            SetActiveStatus();

            ShowErrorMessage(GuiStrings.FrmMain_ShowLoadingErrorTitle,
                string.Format(GuiStrings.FrmMain_ShowLoadingErrorMessage, _viewModel.CurrentDatabasePath));
        }


        private void ShowSearch(object sender, EventArgs e)
        {
            if (_searchWindow != null && _searchWindow.Visible)
            {
                _searchWindow.Focus();
            }
            else if (_searchWindow == null)
            {
                _searchWindow = new SearchForm(_dreamEntries);
                _searchWindow.OnSearchCompleted +=
                    SearchPerformed;
                _searchWindow.OnClear += ClearSearch;
                _searchWindow.Show(this);
            }
            else
            {
                _searchWindow.Visible = true;
            }
        }

        private void StatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var statWindow = new DreamsStatisticsShow(_viewModel.GetDreamTagStatistics());
            statWindow.ShowDialog();
        }

        private void ToolStripAdd_Click(object sender, EventArgs e)
        {
            AddNewEntry();
        }
    }
}
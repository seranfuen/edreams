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
using System.Linq;
using System.Media;
using System.Windows.Forms;
using eDream.GUI;
using eDream.libs;
using eDream.program;
using EvilTools;
using Settings = EvilTools.Settings;

namespace eDream
{
    internal partial class FrmMain : Form
    {
        public const string LoadLastDbSetting = "loadLastDatabase";

        public const string ShowWelcomeSetting = "showWelcomeWindow";

        private const string SettingsFile = "settings.ini";


        private readonly Debug _debug = new Debug(Debug.DebugParameters.ToConsoleAndFile);

        private readonly List<Settings.SettingsPair> _defaultSettings =
            new List<Settings.SettingsPair>();

        private readonly LastOpened _recentlyOpened = new LastOpened();


        private readonly DreamDatabaseViewModel _viewModel =
            new DreamDatabaseViewModel(InjectionKernel.Get<IDreamDiaryPersistenceService>());


        private List<DreamDayEntry> _currentDayList;

        private List<DreamDayEntry> _dayList;

        private Debug.OnDebugIncidence _debugIncidence;

        private List<DreamEntry> _dreamEntries;

        private DreamTagStatistics _dreamStats;

        private bool _loadedFirstTime;
        private SearchForm _searchWindow;
        private Settings _settings;

        public FrmMain()
        {
            InitializeComponent();
            BindingSource.DataSource = _viewModel;
            LoadDefaultSettings();
            InitializeInterface();
        }

        public List<DreamMainStatTag> TagStatistics => _dreamStats.TagStatistics;
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
            _dreamStats.GenerateStatistics(_dreamEntries, _dayList);
            var addEntryBox = new NewEntryForm(_dreamStats.TagStatistics);
            addEntryBox.ShowDialog();
            if (!addEntryBox.CreatedEntry) return;
            _dreamEntries.Add(addEntryBox.NewEntry);
            _dayList = DreamCalendarCreator.GetDreamDayList(_dreamEntries);
            LoadEntriesToList(_dayList);
            PersistDiary();
        }

        /// <summary>
        ///     Clear the right hand panel: delete any associated controls
        /// </summary>
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

        private void CloseDatabase()
        {
            _viewModel.CurrentDatabasePath = string.Empty;
            _dreamEntries = new List<DreamEntry>();
            _dayList = new List<DreamDayEntry>();
            SetUnloadedState();
            DreamListBox.Enabled = false;
            SetStatusBarMsg(GuiStrings.StatusBar_NoDreamDiaryLoaded);
            Text = _viewModel.FormText;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersistDiary();
            CloseDatabase();
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
            _viewModel.LoadingSucceeded += (s, e) => LoadEntriesFromLoader();
            _viewModel.LoadingFailed += (s, e) => ShowLoadingErrorMessage();
            _debugIncidence += SendToDebugger;
            Shown += LoadLastDatabase;

            _settings = new Settings(SettingsFile,
                _defaultSettings);
        }


        private void LoadDayEntries(DreamDayEntry day)
        {
            var entries = day.DreamEntries;
            ClearRightPanel();
            if (entries.Count == 0)
            {
                ShowErrorMessage("Error loading entries",
                    "This day contains no entries. Database may be" +
                    " corrupted");
                return;
            }

            // Improves visuals when quickly transitioning through different days
            TableLayoutPanel.Visible = false;
            var entryCount = 1;
            foreach (var entry in entries)
            {
                if (entry.ToDelete) continue;
                var newEntry = new CtrEntryViewer();
                newEntry.SetViewModel(EntryViewerModel.FromEntry(entry, entryCount++,
                    InjectionKernel.Get<IDreamDiaryBus>()));
                TableLayoutPanel.Controls.Add(newEntry);
            }

            /**
             * Set autosize to avoid visual problems, all entries will have
             * same size
             */
            for (var i = 0; i < TableLayoutPanel.RowStyles.Count; i++)
                TableLayoutPanel.RowStyles[i].SizeType = SizeType.AutoSize;
            /**
             * Add handlers to capture a click and focus on the panel if
             * any entry clicked. Since it becomes pretty complicated
             * (Maybe there's another way?), I'm enclosing it in a try-catch
             * structure to prevent unexpected exceptions
             */
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

        // Loads the default settings to the settings object
        private void LoadDefaultSettings()
        {
            _defaultSettings.Add(new Settings.SettingsPair
            {
                Key = LoadLastDbSetting,
                Value = "yes"
            });

            _defaultSettings.Add(new Settings.SettingsPair
            {
                Key = ShowWelcomeSetting,
                Value = "yes"
            });
        }

        private void LoadDiary()
        {
            SetBusyStatus();
            _viewModel.LoadDiary();
        }

        private void LoadEntriesFromLoader()
        {
            SetActiveStatus();
            LoadEntriesToList(_viewModel.GetDayList());
            SetCurrentFile(_viewModel.CurrentDatabasePath);
            SetLoadedState();
        }


        private void LoadEntriesToList(List<DreamDayEntry> entries)
        {
            TableLayoutPanel.Visible = false;
            DreamListBox.Enabled = false;
            if (entries == null || entries.Count == 0)
            {
                ClearRightPanel();
                return;
            }

            DreamListBox.Enabled = true;
            entries.Sort();

            _currentDayList = entries;
            _viewModel.DreamList = entries;

            DreamListBox.SelectedIndex = Math.Max(0, entries.Count - 1);
            ShowCurrentDay(DreamListBox, new EventArgs());
            TableLayoutPanel.Visible = true;
            SetStatusBarStats();
        }

        private void LoadLastDatabase(object sender, EventArgs e)
        {
            if (_loadedFirstTime || _settings.GetValue(LoadLastDbSetting) != "yes") return;

            _recentlyOpened.LoadPaths();
            SetRecentFilesMenu();
            var recent = _recentlyOpened.GetPaths();
            if (recent.Length > 0)
                if (!string.IsNullOrEmpty(recent[0]))
                {
                    _viewModel.CurrentDatabasePath = recent[0];
                    LoadDiary();
                }

            _loadedFirstTime = true;
        }

        private void LoadRecentDatabase(object sender, EventArgs a)
        {
            var theSender = (RecentlyOpenedMenuItem) sender;

            var resultM =
                MessageBox.Show("Your current database has been modified. Do " +
                                "you wish to save it before closing it?", "Save database?",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (resultM == DialogResult.Cancel)
                return;
            if (resultM == DialogResult.Yes) PersistDiary();

            _viewModel.CurrentDatabasePath = theSender.FilePath;
            LoadDiary();
        }

        private void OnDiaryLoaded(object sender, FinishedLoadingEventArgs e)
        {
            Invoke((Action) LoadEntriesFromLoader);
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

        /// <summary>
        ///     When the saving process is finished, unfreeze the interface and
        ///     inform, if necessary, that there was an error
        /// </summary>
        private void OnPersistenceSucceeded()
        {
            BeginInvoke((Action) (() =>
            {
                SetActiveStatus();
                SetLoadedState();
            }));
        }

        private void OpenDatabaseToolStripMenuItem_Click(object sender,
            EventArgs e)
        {
            var resultM =
                MessageBox.Show("Your current database has been modified. Do " +
                                "you wish to save it before closing it?", "Save database?",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (resultM == DialogResult.Cancel)
                return;
            if (resultM == DialogResult.Yes) PersistDiary();


            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                _viewModel.CurrentDatabasePath = openFileDialog1.FileName;
                // If it was correct, we set it through SetCurrentFile to
                // add it to recently opened
                _viewModel.CurrentDatabasePath = openFileDialog1.FileName;
                LoadDiary();
            }
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

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resultSave = saveFileDialog1.ShowDialog();
            if (resultSave == DialogResult.OK)
            {
                _viewModel.CurrentDatabasePath = saveFileDialog1.FileName;
                PersistDiary();
                saveFileDialog1.FileName = "";
            }
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
            if (searcher.SearchType == SearchForm.ESearchType.textSearch)
                searchType = "text";
            else if (searcher.SearchType == SearchForm.ESearchType.dateSearch)
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
            _recentlyOpened.AddPath(file);
            _recentlyOpened.SavePaths();
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
            LoadEntriesToList(_viewModel.DreamList);
            SetStatusBarStats();
            searchToolStripMenuItem.Enabled = true;
            _dreamStats = new DreamTagStatistics();
            //_dreamStats.GenerateStatistics(_dreamEntries, _dayList);
            Text = _viewModel.FormText;
        }


        private void SetRecentFilesMenu()
        {
            menuRecent.Enabled = false;
            _recentlyOpened.LoadPaths();
            var thePaths = _recentlyOpened.GetPaths().Reverse();

            var paths = thePaths.ToList();
            if (!paths.Any()) return;
            menuRecent.Enabled = true;
            menuRecent.DropDownItems.Clear();
            var menuItems = new
                ToolStripItem[paths.Count];
            var count = 0;
            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path)) continue;
                var newI = new RecentlyOpenedMenuItem(path,
                    _recentlyOpened.ShortenPath(path));
                newI.Click += LoadRecentDatabase;
                menuItems[count] = newI;
                count++;
            }

            var menuItemsFinal = new
                ToolStripItem[count];
            for (var i = 0; i < count; i++) menuItemsFinal[i] = menuItems[i];
            menuRecent.DropDownItems.AddRange(menuItemsFinal);
        }

        /// <summary>
        ///     Display statistics about number of loaded entries in status bar
        /// </summary>
        private void SetStatusBarStats()
        {
            SetStatusBarMsg(_viewModel.StatusBarMessage);
        }


        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new GUI.Settings(_settings);
            settings.ShowDialog();
            if (settings.Result == GUI.Settings.enumResult.Changed) _settings.SaveFile();
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
            _dreamStats.GenerateStatistics(_dreamEntries, _dayList);
            var statWindow = new DreamsStatisticsShow(_dreamStats);
            statWindow.ShowDialog();
        }
    }
}
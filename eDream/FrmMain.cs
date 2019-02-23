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


        private readonly DreamSaveLoad _saveLoad = new DreamSaveLoad();
        private readonly DreamDatabaseViewModel _viewModel = new DreamDatabaseViewModel();

        private string _changingFile;


        private List<DreamDayEntry> _currentDayList;

        private string _currentFile;
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

        /// <summary>
        ///     Gets a list of parsed Main tag statistics
        /// </summary>
        public List<DreamMainStatTag> TagStatistics => _dreamStats.TagStatistics;

        // Loads the default settings to the settings object
        private void LoadDefaultSettings()
        {
            // By default load the last database
            _defaultSettings.Add(new Settings.SettingsPair
            {
                Key = LoadLastDbSetting,
                Value = "yes"
            });
            // By default show the welcome window
            _defaultSettings.Add(new Settings.SettingsPair
            {
                Key = ShowWelcomeSetting,
                Value = "yes"
            });
        }

        /// <summary>
        ///     Changes the status bar message
        /// </summary>
        /// <param name="text">Text to display in status bar</param>
        public void SetStatusBarMsg(string text)
        {
            entriesStatsStatus.Text = text;
        }

        /// <summary>
        ///     Performs any one-time operations when loading the interface for
        ///     the first time
        /// </summary>
        private void InitializeInterface()
        {
            try
            {
                StartPosition = FormStartPosition.CenterScreen;
                SetUnloadedState();

                toolStripLoad.Click +=
                    openDatabaseToolStripMenuItem_Click;
                toolStripButtonSave.Click +=
                    SaveToolStripMenuItem_Click;
                toolStripAdd.Click += AddEntryToolStripMenuItem_Click;
                toolStripNewDB.Click += CreateNewDatabaseToolStripMenuItem_Click;
                toolStripStats.Click += statisticsToolStripMenuItem_Click;
                DreamListBox.SelectedIndexChanged +=
                    ShowCurrentDay;
                DreamListBox.MouseWheel += ScrollList;
                DreamListBox.MouseClick += FocusList;
                tableLayoutPanel1.MouseClick +=
                    FocusEntryPanel;
                _saveLoad.FinishedLoading += OnEntriesLoaded;
                _saveLoad.FinishedSaving += OnEntriesSaved;
                _debugIncidence += SendToDebugger;
                Shown += LoadLastDatabase;
                // Try to load settings
                _settings = new Settings(SettingsFile,
                    _defaultSettings);
            }
            catch (Exception e)
            {
                ProcessIncidence(e, "Error loading the interface");
            }
        }

        private void LoadLastDatabase(object sender, EventArgs e)
        {
            if (!_loadedFirstTime &&
                _settings.GetValue(LoadLastDbSetting) == "yes")
            {
                _recentlyOpened.LoadPaths();
                SetRecentFilesMenu();
                var recent = _recentlyOpened.GetPaths();
                if (recent.Length > 0)
                    if (!string.IsNullOrEmpty(recent[0]))
                    {
                        _changingFile = recent[0];
                        LoadXmlFile();
                    }

                _loadedFirstTime = true;
            }
        }

        /// <summary>
        ///     Display statistics about number of loaded entries in status bar
        /// </summary>
        private void SetStatusBarStats()
        {
            var str = "No entries loaded";
            var dayListCount = 0;
            var entriesCount = 0;
            /**
             * Sometimes a day list object or a dream entry object may be
             * loaded in memory but be flagged for deletion, or empty.
             * Therefore we will only count valid elements
             */
            for (var i = 0; i < _dayList.Count; i++)
                if (_dayList[i].Count > 0)
                    dayListCount++;
            for (var i = 0; i < _dreamEntries.Count; i++)
                if (!_dreamEntries[i].ToDelete &&
                    _dreamEntries[i].GetIfValid())
                    entriesCount++;
            if (entriesCount > 0 && dayListCount > 0)
                str = "Database loaded." +
                      $" {entriesCount} dreams in {dayListCount} days ({(entriesCount / (float) dayListCount).ToString("0.00")} dreams/day)";
            SetStatusBarMsg(str);
        }

        public void ShowCurrentDay(object sender, EventArgs e)
        {
            if (_currentDayList != null && _currentDayList.Count > 0)
                LoadDayEntries(_currentDayList[DreamListBox.SelectedIndex]);
        }

        /// <summary>
        ///     Refresh the list and display all loaded entries
        /// </summary>
        public void RefreshEntries()
        {
            LoadEntriesToList(DreamCalendarCreator.GetDreamDayList(_dreamEntries)
            );
        }

        /// <summary>
        ///     Clear the right hand panel: delete any associated controls
        /// </summary>
        private void ClearRightPanel()
        {
            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.Controls.Clear();
        }

        /// <summary>
        ///     Try to load the entries from the current XML file. It will set
        ///     the status of the application to busy (disallows interaction with
        ///     the GUI showing a progress bar), waiting for the signal from
        ///     the parser
        /// </summary>
        private void LoadXmlFile()
        {
            _saveLoad.CurrentFile = _changingFile;
            SetBusyStatus();
            _saveLoad.LoadEntries();
        }

        /// <summary>
        ///     Act when the parser informs that the loading process is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntriesLoaded(object sender, EventArgs e)
        {
            // Invoke since it will be run from outside the main thread
            Invoke(new MethodInvoker(LoadEntriesFromLoader));
        }

        /// <summary>
        ///     Load entries from loader when it has emitted the singal that it's
        ///     finished
        /// </summary>
        private void LoadEntriesFromLoader()
        {
            SetActiveStatus();
            if (_saveLoad.LoadStatus == DreamSaveLoad.enumLoadStatus.Error)
            {
                ShowErrorMessage("Error", "The file " + _changingFile + " is " +
                                          "not a valid FrmMain XML file or is corrupted", true);
                return;
            }

            _dreamEntries = _saveLoad.EntriesFromXML;
            _dayList = DreamCalendarCreator.GetDreamDayList(_dreamEntries);
            LoadEntriesToList(_dayList);
            SetCurrentFile(_changingFile);
            SetLoadedState();
        }

        /// <summary>
        ///     Send the entries loaded in memory to the XML saver, set the interface
        ///     to busy (frozen) state after which we will wait for a signal
        /// </summary>
        public void SaveXmlFile()
        {
            _saveLoad.CurrentFile = _currentFile;
            _saveLoad.EntriesToXML = _dreamEntries;
            SetBusyStatus();
            _saveLoad.SaveEntries();
        }

        /// <summary>
        ///     Act when the parser informs that the saving process is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntriesSaved(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(EvaluateSavedEntries));
        }

        /// <summary>
        ///     When the saving process is finished, unfreeze the interface and
        ///     inform, if necessary, that there was an error
        /// </summary>
        private void EvaluateSavedEntries()
        {
            SetActiveStatus();
            if (_saveLoad.SaveStatus == DreamSaveLoad.enumSaveStatus.Error)
            {
                ShowErrorMessage("Error", "There was an error saving the database" +
                                          " to" + _currentFile, true);
                return;
            }

            SetLoadedState();
        }

        /// <summary>
        ///     Loads DreamList objects to the list, allowing their dream entries
        ///     to be shown on the right when selected
        /// </summary>
        /// <param name="entries">A list of DreamDayEntry objects</param>
        /// <param name="reverseOrder">
        ///     True if ordered in descending order,
        ///     false if ascending
        /// </param>
        private void LoadEntriesToList(List<DreamDayEntry> entries)
        {
            tableLayoutPanel1.Visible = false;
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
            tableLayoutPanel1.Visible = true;
            SetStatusBarStats();
        }

        /// <summary>
        ///     Empties the entries list and adds a "no entries" dummy entry
        /// </summary>
        private void EmptyEntryList()
        {
            // Just send a null value to LoadEntriesToList and let it handle it
            LoadEntriesToList(null);
        }

        /// <summary>
        ///     Loads the contents of the entries in a DreamDayEntry object (represents
        ///     a day as shown in the list) in the right panel
        /// </summary>
        /// <param name="day"></param>
        private void LoadDayEntries(DreamDayEntry day)
        {
            var entries = day.DreamEntries;
            ClearRightPanel();
            if (entries.Count == 0)
            {
                ShowErrorMessage("Error loading entries",
                    "This day contains no entries. Database may be" +
                    " corrupted", true);
                return;
            }

            // Improves visuals when quickly transitioning through different days
            tableLayoutPanel1.Visible = false;
            for (var i = 0; i < entries.Count; i++)
            {
                if (entries[i].ToDelete) continue;
                var newEntry = new Individual_Entry(entries[i],
                    i + 1, this);
                tableLayoutPanel1.Controls.Add(newEntry);
            }

            /**
             * Set autosize to avoid visual problems, all entries will have
             * same size
             */
            for (var i = 0; i < tableLayoutPanel1.RowStyles.Count; i++)
                tableLayoutPanel1.RowStyles[i].SizeType = SizeType.AutoSize;
            /**
             * Add handlers to capture a click and focus on the panel if
             * any entry clicked. Since it becomes pretty complicated
             * (Maybe there's another way?), I'm enclosing it in a try-catch
             * structure to prevent unexpected exceptions
             */
            try
            {
                for (var i = 0; i < tableLayoutPanel1.Controls.Count; i++)
                for (var j = 0; j < tableLayoutPanel1.Controls[i].Controls.Count; j++)
                {
                    tableLayoutPanel1.Controls[i].Controls[j].MouseClick +=
                        FocusEntryPanel;
                    for (var k = 0; k < tableLayoutPanel1.Controls[i].Controls[j].Controls.Count; k++)
                    {
                        tableLayoutPanel1.Controls[i].Controls[j].Controls[k].MouseClick +=
                            FocusEntryPanel;
                        for (var l = 0; l < tableLayoutPanel1.Controls[i].Controls[j].Controls[k].Controls.Count; l++)
                            tableLayoutPanel1.Controls[i].Controls[j].Controls[k].Controls[l].MouseClick +=
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
                tableLayoutPanel1.Visible = true;
            }
        }

        private void CloseDatabase()
        {
            _currentFile = string.Empty;
            _dreamEntries = new List<DreamEntry>();
            _dayList = new List<DreamDayEntry>();
            SetUnloadedState();
            DreamListBox.Enabled = false;
            SetStatusBarMsg("No database loaded");
            Text = Application.ProductName;
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
            LoadEntriesToList(_dayList);
            SetStatusBarStats();
            searchToolStripMenuItem.Enabled = true;
            _dreamStats = new DreamTagStatistics();
            _dreamStats.GenerateStatistics(_dreamEntries, _dayList);
            Text = Application.ProductName + " - " + GetFileName();
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

        private void SetBusyStatus()
        {
            Enabled = false;
            progressBar1.Visible = true;
            Cursor = Cursors.WaitCursor;
        }

        private void SetActiveStatus()
        {
            Enabled = true;
            progressBar1.Visible = false;
            Cursor = Cursors.Default;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmAboutBox().ShowDialog();
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
            _changingFile = newFile.ChosenPath;
            LoadXmlFile();
        }

        /// <summary>
        ///     Close the database, ask if user wishes to save it before
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Your dream database has been " +
                                      "modified. Do you wish to save it before closing it?" +
                                      " If you don't, all changes will be lost.", "Save database?",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Cancel)
                return;
            if (res == DialogResult.Yes) SaveXmlFile();


            CloseDatabase();
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
            if (resultM == DialogResult.Yes) SaveXmlFile();

            _changingFile = theSender.FilePath;
            LoadXmlFile();
        }

        private void openDatabaseToolStripMenuItem_Click(object sender,
            EventArgs e)
        {
            var resultM =
                MessageBox.Show("Your current database has been modified. Do " +
                                "you wish to save it before closing it?", "Save database?",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (resultM == DialogResult.Cancel)
                return;
            if (resultM == DialogResult.Yes) SaveXmlFile();


            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var oldFile = _currentFile;
                _currentFile = openFileDialog1.FileName;
                // If it was correct, we set it through SetCurrentFile to
                // add it to recently opened
                _changingFile = openFileDialog1.FileName;
                LoadXmlFile();
            }
        }

        private string GetFileName()
        {
            var i = _currentFile.LastIndexOf("\\");
            var fileName = _currentFile;
            if (i > -1) fileName = _currentFile.Substring(_currentFile.LastIndexOf("\\") + 1);
            return fileName;
        }

        public void ShowErrorMessage(string title, string text, bool playSound)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void SetCurrentFile(string file)
        {
            _currentFile = file;
            _recentlyOpened.AddPath(file);
            _recentlyOpened.SavePaths();
            SetRecentFilesMenu();
        }


        private void SetRecentFilesMenu()
        {
            menuRecent.Enabled = false;
            _recentlyOpened.LoadPaths();
            var thePaths = _recentlyOpened.GetPaths();
            thePaths.Reverse();
            if (thePaths.Length == 0) return;
            menuRecent.Enabled = true;
            menuRecent.DropDownItems.Clear();
            var menuItems = new
                ToolStripItem[thePaths.Length];
            var count = 0;
            for (var i = 0; i < thePaths.Length; i++)
            {
                if (string.IsNullOrEmpty(thePaths[i])) continue;
                var newI = new RecentlyOpenedMenuItem(thePaths[i],
                    _recentlyOpened.ShortenPath(thePaths[i]));
                newI.Click += LoadRecentDatabase;
                menuItems[count] = newI;
                count++;
            }

            var menuItemsFinal = new
                ToolStripItem[count];
            for (var i = 0; i < count; i++) menuItemsFinal[i] = menuItems[i];
            menuRecent.DropDownItems.AddRange(menuItemsFinal);
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
            SaveXmlFile();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveXmlFile();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resultSave = saveFileDialog1.ShowDialog();
            if (resultSave == DialogResult.OK)
            {
                _currentFile = saveFileDialog1.FileName;
                SaveXmlFile();
                saveFileDialog1.FileName = "";
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dreamStats.GenerateStatistics(_dreamEntries, _dayList);
            var statWindow = new DreamsStatisticsShow(_dreamStats);
            statWindow.ShowDialog();
        }


        private void SetFindsMessage(string str)
        {
            textBoxFinds.Text = str;
        }

        private void ClearSearch(object sender, EventArgs e)
        {
            SetFindsMessage("Showing all dreams");
            LoadEntriesToList(_dayList);
            _currentDayList = _dayList;
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

        private void FocusList(object sender, MouseEventArgs e)
        {
            DreamListBox.Focus();
        }


        private void FocusEntryPanel(object sender, MouseEventArgs e)
        {
            tableLayoutPanel1.Focus();
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


        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearch(this, e);
        }


        private void SearchStripButton_Click(object sender, EventArgs e)
        {
            ShowSearch(this, e);
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


        private void SendToDebugger(Debug.DebugArgs args)
        {
            _debug.AcceptDebug(args);
        }


        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new GUI.Settings(_settings);
            settings.ShowDialog();
            if (settings.Result == GUI.Settings.enumResult.Changed) _settings.SaveFile();
        }
    }
}
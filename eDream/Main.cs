/****************************************************************************
 * eDreams: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Ángel Verbo
 ****************************************************************************/
/****************************************************************************
    This file is part of eDreams.

    eDreams is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    eDreams is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with eDreams.  If not, see <http://www.gnu.org/licenses/>.]
****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eDream.GUI;
using eDream.libs;
using eDream.program;
using System.Media;
using EvilTools;

namespace eDream
{
    /// <summary>
    /// The main interface and controller of processes carried out by the user
    /// </summary>
    internal partial class eDreams : Form
    {

        /* This flags is true when the application is considered to be in 
         * an unmodified state, and it will be like that until it is saved */
        private bool modified = false;
        
        // The string representation of the path to the currently loaded file
        private string currentFile;

        /// <summary>
        /// Settings keys
        /// </summary>
        public const string loadLastDBSetting = "loadLastDatabase";
        public const string showWelcomeSetting = "showWelcomeWindow";

        /// <summary>
        /// A string representation of a file that is going to be loaded
        /// or saved as. If there is an error in the operation, the currentFile
        /// will not be changed to the changingFile value.
        /// </summary>
        private string changingFile;

        /// <summary>
        /// Used to tell whether the app is loading for the first time. It's used
        /// when events are raised by the shown signal of the form but
        /// we only want it to be triggered once
        /// </summary>
        private bool loadedFirstTime = false;

        /// <summary>
        /// Default settings
        /// </summary>
        private List<EvilTools.Settings.SettingsPair> defaultSettings =
              new List<EvilTools.Settings.SettingsPair>();

        // Settings
        private const string settingsFile = "settings.ini";
        private EvilTools.Settings settings;

        // Database operation object
        private DreamSaveLoad saveLoad = new DreamSaveLoad();
       
        /// <summary>
        /// These represent the default (i.e. whole database) prased entries
        /// </summary>
        private List<DreamEntry> DreamEntries;
        private List<DreamDayList> DayList;

        /// <summary>
        /// Represents the currently active day list, that will be displayed
        /// in the GUI. It could be, for examlpe, the result of a search
        /// </summary>
        private List<DreamDayList> CurrentDayList;

        // Loads the paths to the recently opened files
        private LastOpened recentlyOpened = new LastOpened();
        
        // Dream statistics generator
        private DreamTagStatistics dreamStats;

        /// <summary>
        /// Debugger object, set to file or disabled on release
        /// </summary>
        private Debug debug = new Debug(Debug.DebugParameters.ToConsoleAndFile);

        // Emit a debug incidence
        private Debug.OnDebugIncidence DebugIncidence;

        // Search form
        private SearchForm searchWindow;

        /// <summary>
        /// Gets the value of the flag that tells whether the program is
        /// considered to be in a modified (i.e. unsaved) state
        /// </summary>
        /// <returns></returns>
        public bool Modified {
            get {
                return modified;
            }
        }

        /// <summary>
        /// Gets a list of parsed Main tag statistics
        /// </summary>
        public List<DreamMainStatTag> TagStatistics {
            get {
                return dreamStats.TagStatistics;
            }
        }

        /// <summary>
        /// Create a new main interface
        /// </summary>
        public eDreams()
        {
            InitializeComponent();
            LoadDefaultSettings();
            InitializeInterface();
        }

        // Loads the default settings to the settings object
        private void LoadDefaultSettings() {
            // By default load the last database
            defaultSettings.Add(new EvilTools.Settings.SettingsPair() {
                Key = loadLastDBSetting,
                Value = "yes"
            });
            // By default show the welcome window
            defaultSettings.Add(new EvilTools.Settings.SettingsPair() {
                Key = showWelcomeSetting,
                Value = "yes"
            });
        }

        /// <summary>
        /// Changes the status bar message
        /// </summary>
        /// <param name="text">Text to display in status bar</param>
        public void SetStatusBarMsg(string text) {
            entriesStatsStatus.Text = text;
        }

        /// <summary>
        /// Performs any one-time operations when loading the interface for
        /// the first time
        /// </summary>
        private void InitializeInterface() {
            try {
                StartPosition = FormStartPosition.CenterScreen;
                SetUnloadedState();
                // Event handlers
                FormClosing += new FormClosingEventHandler(ClosingApp);
                toolStripLoad.Click +=
                    new EventHandler(openDatabaseToolStripMenuItem_Click);
                toolStripButtonSave.Click +=
                    new EventHandler(saveToolStripMenuItem_Click);
                toolStripAdd.Click += new
                    EventHandler(addEntryToolStripMenuItem_Click);
                toolStripNewDB.Click += new
                    EventHandler(createNewDatabaseToolStripMenuItem_Click);
                toolStripStats.Click += new
                    EventHandler(statisticsToolStripMenuItem_Click);
                listBox1.SelectedIndexChanged +=
                    new EventHandler(ShowCurrentDay);
                listBox1.MouseWheel += new MouseEventHandler(ScrollList);
                listBox1.MouseClick += new MouseEventHandler(FocusList);
                tableLayoutPanel1.MouseClick +=
                    new MouseEventHandler(FocusEntryPanel);
                saveLoad.FinishedLoading += new EventHandler(OnEntriesLoaded);
                saveLoad.FinishedSaving += new EventHandler(OnEntriesSaved);
                DebugIncidence += new Debug.OnDebugIncidence(SendToDebugger);
                Shown += new EventHandler(LoadLastDatabase);
                // Try to load settings
                settings = new EvilTools.Settings(settingsFile,
            defaultSettings);
            }  catch (Exception e) {
                ProcessIncidence(e, "Error loading the interface");
            }
        }

        /// <summary>
        /// When the program starts, automatically loads the last opened 
        /// database if the settings allow it. Furthermore, it's set so that
        /// it's not called until the form has been shown, because otherwise
        /// there were times when the tread that loads the entires loaded them
        /// and sent the signal before the main window and its controls were
        /// drawn, causing an exception to be thrown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadLastDatabase(object sender, EventArgs e) {
            if (!loadedFirstTime && 
                settings.GetValue(loadLastDBSetting) == "yes") {
                recentlyOpened.LoadPaths();
                SetRecentFilesMenu();
                string[] recent = recentlyOpened.GetPaths();
                if (recent.Length > 0) {
                    if (!string.IsNullOrEmpty(recent[0])) {
                        changingFile = (recent[0]);
                        LoadXMLFile();
                    }
                }
                loadedFirstTime = true;
            }
        }

        /// <summary>
        /// Display statistics about number of loaded entries in status bar
        /// </summary>
        private void SetStatusBarStats() {
            string str = "No entries loaded";
            int DayListCount = 0;
            int EntriesCount = 0;
            /**
             * Sometimes a day list object or a dream entry object may be
             * loaded in memory but be flagged for deletion, or empty.
             * Therefore we will only count valid elements
             */ 
            for (int i = 0; i < DayList.Count; i++) {
                if (DayList[i].GetNumberEntries() > 0) {
                    DayListCount++;
                }
            }
            for (int i = 0; i < DreamEntries.Count; i++) {
                if (!DreamEntries[i].ToDelete && 
                    DreamEntries[i].GetIfValid()) {
                        EntriesCount++;
                }
            }
            if (EntriesCount > 0 && DayListCount > 0) { // Should avoid division by zero too!
                    str = string.Format("Database loaded." +
                    " {0} dreams in {1} days ({2} dreams/day)",
                    EntriesCount, DayListCount,
                    ((float)EntriesCount/(float)DayListCount).ToString("0.00"));
            }
            SetStatusBarMsg(str);
        }

        /// <summary>
        /// This handler fires when we click on a list entry to display the
        /// entries associated to it on the right panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowCurrentDay(object sender, EventArgs e) {
            if (CurrentDayList != null && CurrentDayList.Count > 0) {
                LoadDayEntries(CurrentDayList[listBox1.SelectedIndex]);
            }
        }

        /// <summary>
        /// Refresh the list and display all loaded entries
        /// </summary>
        public void RefreshEntries() {
            LoadEntriesToList(DreamCalendarCreator.GetDreamDayList(DreamEntries),
                false);
        }

        /// <summary>
        /// Clear the right hand panel: delete any associated controls
        /// </summary>
        private void ClearRightPanel() {
            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.Controls.Clear();
        }

        /// <summary>
        /// Try to load the entries from the current XML file. It will set
        /// the status of the application to busy (disallows interaction with
        /// the GUI showing a progress bar), waiting for the signal from
        /// the parser
        /// </summary>
        private void LoadXMLFile() {
            saveLoad.CurrentFile = changingFile;
            SetBusyStatus();
            saveLoad.LoadEntries();
        }

        /// <summary>
        /// Act when the parser informs that the loading process is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntriesLoaded(object sender, EventArgs e) {
            // Invoke since it will be run from outside the main thread
            Invoke(new MethodInvoker(LoadEntriesFromLoader));
        }

        /// <summary>
        /// Load entries from loader when it has emitted the singal that it's
        /// finished
        /// </summary>
        private void LoadEntriesFromLoader() {
            SetActiveStatus();
            if (saveLoad.LoadStatus == DreamSaveLoad.enumLoadStatus.Error) {
                ShowErrorMessage("Error", "The file " + changingFile + " is " +
                    "not a valid eDreams XML file or is corrupted", true);
                return;
            }
            DreamEntries = saveLoad.EntriesFromXML;
            DayList = DreamCalendarCreator.GetDreamDayList(DreamEntries);
            LoadEntriesToList(DayList, false);
            SetCurrentFile(changingFile);
            SetLoadedState();
            SetUnModifiedState();
        }

        /// <summary>
        /// Send the entries loaded in memory to the XML saver, set the interface
        /// to busy (frozen) state after which we will wait for a signal
        /// </summary>
        private void SaveXMLFile() {
            saveLoad.CurrentFile = currentFile;
            saveLoad.EntriesToXML = DreamEntries;
            SetBusyStatus();
            saveLoad.SaveEntries();
        }

        /// <summary>
        /// Act when the parser informs that the saving process is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntriesSaved(object sender, EventArgs e) {
            Invoke(new MethodInvoker(EvaluateSavedEntries));
        }

        /// <summary>
        /// When the saving process is finished, unfreeze the interface and
        /// inform, if necessary, that there was an error
        /// </summary>
        private void EvaluateSavedEntries() {
            SetActiveStatus();
            if (saveLoad.SaveStatus == DreamSaveLoad.enumSaveStatus.Error) {
                ShowErrorMessage("Error", "There was an error saving the database" +
                    " to" + currentFile, true);
                return;
            }
            SetLoadedState();
            SetUnModifiedState();
        }

        /// <summary>
        /// Loads DreamList objects to the list, allowing their dream entries
        /// to be shown on the right when selected
        /// </summary>
        /// <param name="entries">A list of DreamDayList objects</param>
        /// <param name="ReverseOrder">True if ordered in descending order,
        /// false if ascending</param>
        private void LoadEntriesToList(List<DreamDayList> entries, 
         bool ReverseOrder) {
             tableLayoutPanel1.Visible = false;
             listBox1.Enabled = false;
             if (entries == null || entries.Count == 0) { // Empty list with "No entries"
                 List<string> noResults = new List<string>();
                 noResults.Add("No entries");
                 listBox1.DataSource = noResults;
                 ClearRightPanel();
                 return;
             }
             listBox1.Enabled = true;
             entries.Sort();
             if (ReverseOrder) {
                 entries.Reverse();
             }
             CurrentDayList = entries;
             listBox1.DataSource = entries;
             /* Select last item in list and fire the event (won't fire 
             * just by changing SelectedIndex) to display the day 
             * automatically selected */
             listBox1.SelectedIndex = entries.Count - 1;
             ShowCurrentDay(listBox1, new EventArgs());
             tableLayoutPanel1.Visible = true;
             SetStatusBarStats();
        }

        /// <summary>
        /// Empties the entries list and adds a "no entries" dummy entry
        /// </summary>
        private void EmptyEntryList() {
            // Just send a null value to LoadEntriesToList and let it handle it
            LoadEntriesToList(null, true);
        }

        /// <summary>
        /// Loads the contents of the entries in a DreamDayList object (represents
        /// a day as shown in the list) in the right panel
        /// </summary>
        /// <param name="day"></param>
        private void LoadDayEntries(DreamDayList day) {
            List<DreamEntry> entries = day.DreamEntries;
            ClearRightPanel();
            if (entries.Count == 0) {
                ShowErrorMessage("Error loading entries",
                    "This day contains no entries. Database may be" +
                    " corrupted", true);
                return;
            }
            // Improves visuals when quickly transitioning through different days
            tableLayoutPanel1.Visible = false;
            for (int i = 0; i < entries.Count; i++) {
                if (entries[i].ToDelete) {
                    continue;
                }
                Individual_Entry newEntry = new Individual_Entry(entries[i],
                    i + 1, this);
                tableLayoutPanel1.Controls.Add(newEntry);
            }
            /**
             * Set autosize to avoid visual problems, all entries will have
             * same size
             */ 
            for (int i = 0; i < tableLayoutPanel1.RowStyles.Count; i++) {
                tableLayoutPanel1.RowStyles[i].SizeType = SizeType.AutoSize;
            }
            /**
             * Add handlers to capture a click and focus on the panel if
             * any entry clicked. Since it becomes pretty complicated
             * (Maybe there's another way?), I'm enclosing it in a try-catch
             * structure to prevent unexpected exceptions
             */
            try {
                for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++) {
                    for (int j = 0; j < tableLayoutPanel1.Controls[i].Controls.Count; j++) {
                        tableLayoutPanel1.Controls[i].Controls[j].MouseClick +=
                            new MouseEventHandler(FocusEntryPanel);
                        for (int k = 0; k < tableLayoutPanel1.Controls[i].Controls[j].Controls.Count; k++) {
                            tableLayoutPanel1.Controls[i].Controls[j].Controls[k].MouseClick +=
                                new MouseEventHandler(FocusEntryPanel);
                            for (int l = 0; l < tableLayoutPanel1.Controls[i].Controls[j].Controls[k].Controls.Count; l++) {
                                tableLayoutPanel1.Controls[i].Controls[j].Controls[k].Controls[l].MouseClick +=
                                    new MouseEventHandler(FocusEntryPanel);//This is the panel where we click on the most
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                ProcessIncidence(e, @"Error setting focus to right panel: when
 setting focus to right panel we loop through all controls in the
 layout panel down to a third level, adding a mouse click handler which is
 supposed to process all clicks on the interface except inside buttons and the
 text box");
            }
            finally {
                tableLayoutPanel1.Visible = true; 
            }
        }

        /// <summary>
        /// Unloads from memory the current database and empties the GUI
        /// </summary>
        private void CloseDatabase() {
            currentFile = string.Empty;
            DreamEntries = new List<DreamEntry>();
            DayList = new List<DreamDayList>();
            SetUnloadedState();
            listBox1.Enabled = false;
            SetStatusBarMsg("No database loaded");
            Text = Application.ProductName;
            SetUnModifiedState();
        }

        /// <summary>
        /// Operations carried out when a database is loaded
        /// </summary>
        private void SetLoadedState() {
            entryToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            addEntryToolStripMenuItem.Enabled = true;
            toolStripAdd.Enabled = true;
            toolStripStats.Enabled = true;
            ClearRightPanel();
            LoadEntriesToList(DayList, false);
            SetStatusBarStats();
            searchToolStripMenuItem.Enabled = true;
            dreamStats = new DreamTagStatistics();
            dreamStats.GenerateStatistics(DreamEntries, DayList);
            Text = Application.ProductName + " - " + GetFileName();
        }

        /// <summary>
        /// Operations done when the database is unloaded
        /// </summary>
        private void SetUnloadedState() {
            entryToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            addEntryToolStripMenuItem.Enabled = false;
            toolStripAdd.Enabled = false;
            toolStripStats.Enabled = false;
            searchToolStripMenuItem.Enabled = false;
            EmptyEntryList();
            ClearRightPanel();
        }

        /// <summary>
        /// Locks the application by setting it to a "busy" status, as 
        /// shown by the cursor and the active progress bar, while loading
        /// or saving process are active
        /// </summary>
        private void SetBusyStatus() {
            Enabled = false;
            progressBar1.Visible = true;
            Cursor = Cursors.WaitCursor;
        }

        /// <summary>
        /// Enabled the application after the "busy" status, hides the
        /// progress bar
        /// </summary>
        private void SetActiveStatus() {
            Enabled = true;
            progressBar1.Visible = false;
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Do any operations when the database in memory has been modified
        /// but it hasn't been saved to a file yet
        /// </summary>
        public void SetModifiedState() {
            modified = true;
            saveToolStripMenuItem.Enabled = true;
            toolStripButtonSave.Enabled = true;
        }

        /// <summary>
        /// Do any operations when the database is saved, or newly loaded,
        /// and the db in memory hasn't changed from the one in the file
        /// </summary>
        public void SetUnModifiedState() {
            modified = false;
            saveToolStripMenuItem.Enabled = false;
            toolStripButtonSave.Enabled = false;
        }

        /// <summary>
        /// Exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        /// <summary>
        /// Show about dilaog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        /// <summary>
        /// Show dialog to create a new database, and if affirmative, create it,
        /// closing the current one, asking if user wishes to save the current
        /// one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewDatabaseToolStripMenuItem_Click(object sender, EventArgs e) {
            NewFileBox newFile = new NewFileBox();
            newFile.ShowDialog();
            if (newFile.Action == NewFileBox.EAction.Created) {
                closeToolStripMenuItem.PerformClick();
                changingFile = newFile.File;
                LoadXMLFile();
            }
        }

        /// <summary>
        /// Close the database, ask if user wishes to save it before
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (modified) {
                DialogResult res = MessageBox.Show("Your dream database has been " +
                    "modified. Do you wish to save it before closing it?" +
                    " If you don't, all changes will be lost.", "Save database?",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Cancel) {
                    return;
                }
                else if (res == DialogResult.Yes) {
                    SaveXMLFile();
                }
            }
            CloseDatabase();
        }

        /// <summary>
        /// Load a recently opened database when clicked in the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void LoadRecentDatabase(object sender, EventArgs a) {
            RecentlyOpenedMenuItem theSender = (RecentlyOpenedMenuItem)sender;
            if (modified) {
                DialogResult resultM =
                    MessageBox.Show("Your current database has been modified. Do " +
                    "you wish to save it before closing it?", "Save database?",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (resultM == DialogResult.Cancel) {
                    return;
                }
                else if (resultM == DialogResult.Yes) {
                    SaveXMLFile();
                }

            }
            changingFile = theSender.FilePath;
            LoadXMLFile();
        }

        /// <summary>
        /// Load another database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openDatabaseToolStripMenuItem_Click(object sender,
            EventArgs e) {
            if (modified) {
                DialogResult resultM = 
                    MessageBox.Show("Your current database has been modified. Do "+
                    "you wish to save it before closing it?", "Save database?",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (resultM == DialogResult.Cancel) {
                    return;
                }
                else if (resultM == DialogResult.Yes) {
                    SaveXMLFile();
                }

            }
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                string oldFile = currentFile;
                currentFile = openFileDialog1.FileName;
                // If it was correct, we set it through SetCurrentFile to
                // add it to recently opened
                changingFile = openFileDialog1.FileName;
                LoadXMLFile();
            }
        }

        /// <summary>
        /// Extract the filename from the path to the current database
        /// </summary>
        /// <returns></returns>
        private string GetFileName() {
            int i = currentFile.LastIndexOf("\\");
            string fileName = currentFile;
            if (i > -1) {
                fileName = currentFile.Substring(currentFile.LastIndexOf("\\")+1);
            }
            return fileName;
        }

        /// <summary>
        /// Display an error message
        /// </summary>
        /// <param name="title">Title of the message box</param>
        /// <param name="text">Text explaining the error</param>
        /// <param name="playSound">If true, play a sound</param>
        public void ShowErrorMessage(string title, string text, bool playSound) {
            if (playSound) {
                SystemSounds.Beep.Play();
            }
            MessageBox.Show(text, title, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Sets the current (active) file to the path given and adds it
        /// to the recently opened list
        /// </summary>
        /// <param name="file"></param>
        private void SetCurrentFile(string file) {
            currentFile = file;
            recentlyOpened.AddPath(file);
            recentlyOpened.SavePaths();
            SetRecentFilesMenu();
        }


        /// <summary>
        /// IF there are any recently opened paths in a text file, loads
        /// them to the menu
        /// </summary>
        private void SetRecentFilesMenu() {
            menuRecent.Enabled = false;
            recentlyOpened.LoadPaths();
            string[] thePaths = recentlyOpened.GetPaths();
            thePaths.Reverse();
            if (thePaths.Length == 0) {
                return;
            }
            menuRecent.Enabled = true;
            menuRecent.DropDownItems.Clear();
            ToolStripItem[] menuItems = new
                ToolStripItem[thePaths.Length];
            int count = 0;
            for (int i = 0; i < thePaths.Length;  i++) {
                if (string.IsNullOrEmpty(thePaths[i])) {
                    continue;
                }
                RecentlyOpenedMenuItem newI = new RecentlyOpenedMenuItem(thePaths[i],
                recentlyOpened.ShortenPath(thePaths[i]));
                newI.Click += new EventHandler(LoadRecentDatabase);
                menuItems[count] = newI;
                count++;
            }
            ToolStripItem[] menuItemsFinal = new
                ToolStripItem[count];
            for (int i = 0; i < count; i++) {
                menuItemsFinal[i] = menuItems[i];
            }
                menuRecent.DropDownItems.AddRange(menuItemsFinal);
        }

        /// <summary>
        /// Show the New Entry dialog and if a new entry was created get it
        /// and refresh the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEntryToolStripMenuItem_Click(object sender, EventArgs e) {
            dreamStats.GenerateStatistics(DreamEntries, DayList);
            NewEntryForm addEntryBox = new NewEntryForm(dreamStats.TagStatistics);
            addEntryBox.ShowDialog();
            if (addEntryBox.CreatedEntry) {
                DreamEntries.Add(addEntryBox.NewEntry);
                DayList = DreamCalendarCreator.GetDreamDayList(DreamEntries);
                LoadEntriesToList(DayList, false);
                SetModifiedState();  
            }
        }

        /// <summary>
        /// Save database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveXMLFile();
        }

        /// <summary>
        /// Show save as dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (modified) {
                DialogResult result = MessageBox.Show("Your current dream" +
                    " database has changed. Do you wish to save it before" +
                    " choosing a new name? If not, the changes in the" +
                    " current file will not be saved", "Save current database?",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) {
                    return;
                }
                else if (result == DialogResult.Yes) {
                    SaveXMLFile();
                }
            }
            DialogResult resultSave = saveFileDialog1.ShowDialog();
            if (resultSave == DialogResult.OK) {
                currentFile = saveFileDialog1.FileName;
                SaveXMLFile();
                saveFileDialog1.FileName = "";
            }
        }

        /// <summary>
        /// Intercept closing event and, if the database is modified, ask user
        /// if he wants to save it before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingApp(object sender, FormClosingEventArgs e) {
            if (modified) {
                SystemSounds.Question.Play();
                DialogResult result = MessageBox.Show("Your current dream" +
                    " database has changed. Do you wish to save it before " +
                    "closing the application? ", "Save database", 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) {
                    e.Cancel = true;
                }
                else if (result == DialogResult.Yes) {
                    SaveXMLFile();
                }
            }
        }

        /// <summary>
        /// Show statistics window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e) {
            dreamStats.GenerateStatistics(DreamEntries, DayList);
            DreamsStatisticsShow statWindow = new DreamsStatisticsShow(dreamStats);
            statWindow.ShowDialog();
        }


        /// <summary>
        /// Exit app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }



        /// <summary>
        /// A label below the list to show the number of search results and let
        /// the user know that a search has been conducted
        /// </summary>
        /// <param name="str"></param>
        private void SetFindsMessage(string str) {
            textBoxFinds.Text = str;            
        }

        /// <summary>
        /// Clears a search and shows the complete list of days with dreams
        /// </summary>
        private void ClearSearch(object sender, EventArgs e) {
            SetFindsMessage("Showing all dreams");
            LoadEntriesToList(DayList, false);
            CurrentDayList = DayList;
        }

        /// <summary>
        /// When displaying the string that was searched for, it may be too 
        /// long, so we shorten it and add ... to indicate ellipsis
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ShortenSearchTerm(string str) {
            const int maxSize = 15;
            if (str.Length > maxSize) {
                return str.Substring(0, maxSize - 3) + "...";
            }
            return str;
        }

        /// <summary>
        /// Scrolls up/down the list when moving the mouse wheel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollList(object sender, MouseEventArgs e) {
            int selected = listBox1.SelectedIndex;
            HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
            // If going down (different speeds)
            if (ee.Delta < 0) {
                if (ee.Delta < -120 && selected < CurrentDayList.Count - 2) {
                    listBox1.SelectedIndex = selected + 2;
                }
                else if (ee.Delta < -600 && selected < CurrentDayList.Count - 3) {
                    listBox1.SelectedIndex = selected + 3;
                }
                else if (selected < CurrentDayList.Count - 1) {
                    listBox1.SelectedIndex = selected + 1;
                }
            }
            // If going up
            else if (ee.Delta > 0) {
                if (ee.Delta > 120 && selected > 1 ) {
                    listBox1.SelectedIndex = selected-2;
                }
                else if (ee.Delta > 600 && selected > 2) {
                    listBox1.SelectedIndex = selected - 3;
                }
                else if (selected > 0) {
                    listBox1.SelectedIndex = selected - 1;
                }
            }
            // Prevent it being handled by autoscroll
            ee.Handled = true;
            
        }

        /// <summary>
        /// Focuses the day list when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocusList(object sender, MouseEventArgs e) {
            listBox1.Focus();
        }

        /// <summary>
        /// Focuses the entry panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocusEntryPanel(object sender, MouseEventArgs e) {
            tableLayoutPanel1.Focus();
        }

        /// <summary>
        /// Show the search window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSearch(object sender, EventArgs e) {
            if (searchWindow != null && searchWindow.Visible) {
                searchWindow.Focus();
            }
            else if (searchWindow == null) {
                searchWindow = new SearchForm(DreamEntries);
                searchWindow.OnSearchCompleted +=
                    new SearchForm.SearchEvent(SearchPerformed);
                searchWindow.OnClear += new EventHandler(ClearSearch);
                searchWindow.Show(this);
            }
            else {
                searchWindow.Visible = true;
            }
        }

        /// <summary>
        /// When a search has been performed, retrieve results and show them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchPerformed(object sender, EventArgs e) {
            SearchForm searcher = (SearchForm)sender;
            SystemSounds.Beep.Play();
            LoadEntriesToList(DreamCalendarCreator.GetDreamDayList(searcher.Results),
                false);
            string searchType;
            if (searcher.SearchType == SearchForm.ESearchType.textSearch) {
                searchType = "text";
            }
            else if (searcher.SearchType == SearchForm.ESearchType.dateSearch) {
                searchType = "date";
            } else {
                searchType = "tags";
            }
            string entry = "entries";
            if (searcher.Results.Count == 1) {
                entry = "entry";
            }
            string msg = string.Format("Your search for the {0} \"{1}\" found" +
            " {2} {3}", searchType, searcher.LastSearchText,
            searcher.Results.Count, entry);
            SetFindsMessage(msg);
            Focus();
        }

        /// <summary>
        /// Try to prevent mouse wheel from scrolling listBox
        /// </summary>
        /// <param name="e"></param>
        private void FormDetailedReport_MouseWheel(object sender, MouseEventArgs e) {
            HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
            ee.Handled = true;
        }

        /// <summary>
        /// Display search window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowSearch(this, e);
        }

        /// <summary>
        /// Display search window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchStripButton_Click(object sender, EventArgs e) {
            ShowSearch(this, e);
        }

        /// <summary>
        /// Process any incidence on this object
        /// </summary>
        /// <param name="e"></param>
        /// <param name="msg"></param>
        private void ProcessIncidence(Exception e, string msg) {
            Debug.DebugArgs args = new Debug.DebugArgs() {
                Message = msg,
                ExceptionMessage = e.Message,
                StackTrace = e.StackTrace
            };
            if (DebugIncidence != null) {
                DebugIncidence(args);
            }
        }

        /// <summary>
        /// Send incidence to debugger. This should be send to the delegates of
        /// any classes, including Main, that may send debug incidences.
        /// All debug events connected to this class should be sent here
        /// </summary>
        /// <param name="args"></param>
        private void SendToDebugger(Debug.DebugArgs args) {
            debug.AcceptDebug(args);
        }

        /// <summary>
        /// Opens the settings form and saves the new/modified settings if the
        /// result of the dialog is Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
            GUI.Settings settings = new GUI.Settings(this.settings);
            settings.ShowDialog();
            if (settings.Result == GUI.Settings.enumResult.Changed) {
                this.settings.SaveFile();
            }
        }
    }
}
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace eDream.GUI {
    /// <summary>
    /// Used to change the application settings                                
    /// </summary>
    // TODO close form when escape pressed
    public partial class Settings : Form {

        /// <summary>
        /// Indicates whether the user clicked on OK to change the settings or
        /// if he cancelled the box
        /// </summary>
        public enum enumResult {
            Changed,
            Cancelled
        }

        /// <summary>
        /// The string values that are used in the settings file to store
        /// boolean values
        /// </summary>
        private const string trueStr = "yes";
        private const string falseStr = "no";

        private enumResult result = enumResult.Cancelled;
        private EvilTools.Settings settings;


        /// <summary>
        /// Returns whether the user chose to accept the (new or same, no 
        /// distinction made) settings or cancelled the dialog
        /// </summary>
        public enumResult Result {
            get {
                return result;
            }
        }

        /// <summary>
        /// Creates a dialog to change the settings. An EvilTools.Settings
        /// object must be passed by the caller which will be used to extract
        /// the current settings that will be displayed in the form and to set
        /// the new settings, so no action by the caller (other than refreshing
        /// GUI if necessary) is needed upon settings changes.
        /// </summary>
        /// <param name="settings"></param>
        public Settings(EvilTools.Settings settings) {
            this.settings = settings;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            KeyDown += new KeyEventHandler(CloseForm);
            SetOriginalSettings();
        }

        /// <summary>
        /// Set the elements of the dialog to match the current settings
        /// </summary>
        private void SetOriginalSettings() {
            if (string.Compare(settings.GetValue(FrmMain.LoadLastDbSetting),
                trueStr, true) == 0) {
                checkLoadLastDB.Checked = true;
            }
            else {
                checkLoadLastDB.Checked = false;
            }
            if (string.Compare(settings.GetValue(FrmMain.ShowWelcomeSetting),
                trueStr, true) == 0) {
                checkShowWelcomeWindow.Checked = true;
            }
            else {
                checkShowWelcomeWindow.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {

        }

        /// <summary>
        /// Accept changes, so change the values in the settings object to
        /// match those in the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            string value;
            if (checkLoadLastDB.Checked == true) {
                value = trueStr;
            }
            else {
                value = falseStr;
            }
            settings.ChangeValue(FrmMain.LoadLastDbSetting, value);
            if (checkShowWelcomeWindow.Checked == true) {
                value = trueStr;
            }
            else {
                value = falseStr;
            }
            settings.ChangeValue(FrmMain.ShowWelcomeSetting, value);
            result = enumResult.Changed;
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e) {
            result = enumResult.Cancelled;
            Dispose();
        }

        private void CloseForm(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Escape) {
                Dispose();
            }
        }

    }
}

namespace eDream
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.entriesStatsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateNewDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxFinds = new System.Windows.Forms.RichTextBox();
            this.findsLabel = new System.Windows.Forms.Label();
            this.DreamListBox = new System.Windows.Forms.ListBox();
            this.EntriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAdd = new System.Windows.Forms.ToolStripButton();
            this.searchStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripStats = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusBar.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EntriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entriesStatsStatus});
            this.statusBar.Location = new System.Drawing.Point(0, 533);
            this.statusBar.Name = "statusBar";
            this.statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusBar.Size = new System.Drawing.Size(1016, 22);
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "statusStrip1";
            // 
            // entriesStatsStatus
            // 
            this.entriesStatsStatus.Name = "entriesStatsStatus";
            this.entriesStatsStatus.Size = new System.Drawing.Size(100, 17);
            this.entriesStatsStatus.Text = "No entries loaded";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fdToolStripMenuItem,
            this.editToolStripMenuItem,
            this.entryToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1016, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fdToolStripMenuItem
            // 
            this.fdToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateNewDatabaseToolStripMenuItem,
            this.toolStripSeparator1,
            this.OpenDatabaseToolStripMenuItem,
            this.toolStripSeparator6,
            this.CloseToolStripMenuItem,
            this.toolStripSeparator5,
            this.SaveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.menuRecent,
            this.toolStripSeparator3,
            this.ExitToolStripMenuItem});
            this.fdToolStripMenuItem.Name = "fdToolStripMenuItem";
            this.fdToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fdToolStripMenuItem.Text = "File";
            // 
            // CreateNewDatabaseToolStripMenuItem
            // 
            this.CreateNewDatabaseToolStripMenuItem.Name = "CreateNewDatabaseToolStripMenuItem";
            this.CreateNewDatabaseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.CreateNewDatabaseToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.CreateNewDatabaseToolStripMenuItem.Text = "New";
            this.CreateNewDatabaseToolStripMenuItem.Click += new System.EventHandler(this.CreateNewDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(190, 6);
            // 
            // OpenDatabaseToolStripMenuItem
            // 
            this.OpenDatabaseToolStripMenuItem.Name = "OpenDatabaseToolStripMenuItem";
            this.OpenDatabaseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenDatabaseToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.OpenDatabaseToolStripMenuItem.Text = "Open";
            this.OpenDatabaseToolStripMenuItem.Click += new System.EventHandler(this.OpenDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(190, 6);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.CloseToolStripMenuItem.Text = "Close";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(190, 6);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.SaveAsToolStripMenuItem.Text = "Save as...";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(190, 6);
            // 
            // menuRecent
            // 
            this.menuRecent.Enabled = false;
            this.menuRecent.Name = "menuRecent";
            this.menuRecent.Size = new System.Drawing.Size(193, 22);
            this.menuRecent.Text = "Recent Files";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(190, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SearchToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // SearchToolStripMenuItem
            // 
            this.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem";
            this.SearchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.SearchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SearchToolStripMenuItem.Text = "Search...";
            this.SearchToolStripMenuItem.Click += new System.EventHandler(this.SearchToolStripMenuItem_Click);
            // 
            // entryToolStripMenuItem
            // 
            this.entryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddEntryToolStripMenuItem,
            this.StatisticsToolStripMenuItem});
            this.entryToolStripMenuItem.Name = "entryToolStripMenuItem";
            this.entryToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.entryToolStripMenuItem.Text = "Entries";
            // 
            // AddEntryToolStripMenuItem
            // 
            this.AddEntryToolStripMenuItem.Name = "AddEntryToolStripMenuItem";
            this.AddEntryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.AddEntryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.AddEntryToolStripMenuItem.Text = "Add entry";
            this.AddEntryToolStripMenuItem.Click += new System.EventHandler(this.AddEntryToolStripMenuItem_Click);
            // 
            // StatisticsToolStripMenuItem
            // 
            this.StatisticsToolStripMenuItem.Name = "StatisticsToolStripMenuItem";
            this.StatisticsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.StatisticsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.StatisticsToolStripMenuItem.Text = "Statistics";
            this.StatisticsToolStripMenuItem.Click += new System.EventHandler(this.StatisticsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.AboutToolStripMenuItem.Text = "About...";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 52);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxFinds);
            this.splitContainer1.Panel1.Controls.Add(this.findsLabel);
            this.splitContainer1.Panel1.Controls.Add(this.DreamListBox);
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.TableLayoutPanel);
            this.splitContainer1.Panel2MinSize = 481;
            this.splitContainer1.Size = new System.Drawing.Size(1016, 481);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 3;
            // 
            // textBoxFinds
            // 
            this.textBoxFinds.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxFinds.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxFinds.Location = new System.Drawing.Point(11, 11);
            this.textBoxFinds.Name = "textBoxFinds";
            this.textBoxFinds.ReadOnly = true;
            this.textBoxFinds.Size = new System.Drawing.Size(174, 30);
            this.textBoxFinds.TabIndex = 3;
            this.textBoxFinds.Text = "Showing all dreams";
            // 
            // findsLabel
            // 
            this.findsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.findsLabel.AutoSize = true;
            this.findsLabel.ForeColor = System.Drawing.Color.Crimson;
            this.findsLabel.Location = new System.Drawing.Point(12, 103);
            this.findsLabel.Name = "findsLabel";
            this.findsLabel.Size = new System.Drawing.Size(0, 13);
            this.findsLabel.TabIndex = 2;
            // 
            // DreamListBox
            // 
            this.DreamListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DreamListBox.DataSource = this.EntriesBindingSource;
            this.DreamListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DreamListBox.FormattingEnabled = true;
            this.DreamListBox.ItemHeight = 16;
            this.DreamListBox.Location = new System.Drawing.Point(11, 50);
            this.DreamListBox.Name = "DreamListBox";
            this.DreamListBox.ScrollAlwaysVisible = true;
            this.DreamListBox.Size = new System.Drawing.Size(174, 420);
            this.DreamListBox.TabIndex = 0;
            // 
            // EntriesBindingSource
            // 
            this.EntriesBindingSource.DataMember = "DreamDays";
            this.EntriesBindingSource.DataSource = this.BindingSource;
            // 
            // BindingSource
            // 
            this.BindingSource.DataSource = typeof(eDream.GUI.DreamDiaryViewModel);
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.AutoScroll = true;
            this.TableLayoutPanel.ColumnCount = 1;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowCount = 1;
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel.Size = new System.Drawing.Size(811, 479);
            this.TableLayoutPanel.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAdd,
            this.searchStripButton,
            this.toolStripStats});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripAdd
            // 
            this.toolStripAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAdd.Image")));
            this.toolStripAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAdd.Name = "toolStripAdd";
            this.toolStripAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripAdd.Text = "Add new entry";
            this.toolStripAdd.Click += new System.EventHandler(this.ToolStripAdd_Click);
            // 
            // searchStripButton
            // 
            this.searchStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchStripButton.Image = ((System.Drawing.Image)(resources.GetObject("searchStripButton.Image")));
            this.searchStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchStripButton.Name = "searchStripButton";
            this.searchStripButton.Size = new System.Drawing.Size(23, 22);
            this.searchStripButton.Text = "Search";
            this.searchStripButton.Click += new System.EventHandler(this.SearchStripButton_Click);
            // 
            // toolStripStats
            // 
            this.toolStripStats.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStats.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStats.Image")));
            this.toolStripStats.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStats.Name = "toolStripStats";
            this.toolStripStats.Size = new System.Drawing.Size(23, 22);
            this.toolStripStats.Text = "Dream Statistics";
            this.toolStripStats.Click += new System.EventHandler(this.ToolStripStats_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "XML Files|*.xml";
            this.openFileDialog1.Title = "Load a dream database";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "my_dreams.xml";
            this.saveFileDialog1.Filter = "Dream Database|*.xml";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(742, 536);
            this.progressBar1.MarqueeAnimationSpeed = 30;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(258, 16);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Visible = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 555);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 300);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "eDreams";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EntriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StatisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox DreamListBox;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripStatusLabel entriesStatsStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem fdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateNewDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem OpenDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuRecent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripAdd;
        private System.Windows.Forms.ToolStripButton toolStripStats;
        private System.Windows.Forms.Label findsLabel;
        private System.Windows.Forms.RichTextBox textBoxFinds;
        private System.Windows.Forms.ToolStripMenuItem SearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton searchStripButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.BindingSource BindingSource;
        private System.Windows.Forms.BindingSource EntriesBindingSource;
    }
}


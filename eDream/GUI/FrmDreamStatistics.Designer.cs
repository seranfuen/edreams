namespace eDream.GUI {
    partial class FrmDreamStatistics {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button CloseButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDreamStatistics));
            this.TotalDreamsLabel = new System.Windows.Forms.Label();
            this.StatsTable = new System.Windows.Forms.DataGridView();
            this.PercentOfTotalDaysDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShowOnlyParentTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.SearchTagsTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            CloseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.StatsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CloseButton.Location = new System.Drawing.Point(613, 555);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(75, 23);
            CloseButton.TabIndex = 1;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // TotalDreamsLabel
            // 
            this.TotalDreamsLabel.AutoSize = true;
            this.TotalDreamsLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ViewModelBindingSource, "DreamCountSummary", true));
            this.TotalDreamsLabel.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalDreamsLabel.Location = new System.Drawing.Point(12, 9);
            this.TotalDreamsLabel.Name = "TotalDreamsLabel";
            this.TotalDreamsLabel.Size = new System.Drawing.Size(366, 13);
            this.TotalDreamsLabel.TabIndex = 0;
            this.TotalDreamsLabel.Text = "There are no dream entries in your database right now";
            // 
            // StatsTable
            // 
            this.StatsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatsTable.AutoGenerateColumns = false;
            this.StatsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StatsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tagDataGridViewTextBoxColumn,
            this.countDataGridViewTextBoxColumn,
            this.percentageDataGridViewTextBoxColumn,
            this.PercentOfTotalDaysDisplay});
            this.StatsTable.DataMember = "TagStatistics";
            this.StatsTable.DataSource = this.ViewModelBindingSource;
            this.StatsTable.Location = new System.Drawing.Point(15, 58);
            this.StatsTable.Name = "StatsTable";
            this.StatsTable.ReadOnly = true;
            this.StatsTable.Size = new System.Drawing.Size(673, 491);
            this.StatsTable.TabIndex = 2;
            // 
            // PercentOfTotalDaysDisplay
            // 
            this.PercentOfTotalDaysDisplay.DataPropertyName = "PercentOfTotalDaysDisplay";
            this.PercentOfTotalDaysDisplay.HeaderText = "% of days";
            this.PercentOfTotalDaysDisplay.Name = "PercentOfTotalDaysDisplay";
            this.PercentOfTotalDaysDisplay.ReadOnly = true;
            // 
            // ShowOnlyParentTagsCheckBox
            // 
            this.ShowOnlyParentTagsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowOnlyParentTagsCheckBox.AutoSize = true;
            this.ShowOnlyParentTagsCheckBox.Checked = true;
            this.ShowOnlyParentTagsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowOnlyParentTagsCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.ViewModelBindingSource, "ShowOnlyParentTags", true));
            this.ShowOnlyParentTagsCheckBox.Location = new System.Drawing.Point(476, 559);
            this.ShowOnlyParentTagsCheckBox.Name = "ShowOnlyParentTagsCheckBox";
            this.ShowOnlyParentTagsCheckBox.Size = new System.Drawing.Size(131, 17);
            this.ShowOnlyParentTagsCheckBox.TabIndex = 3;
            this.ShowOnlyParentTagsCheckBox.Text = "Show only parent tags";
            this.ShowOnlyParentTagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // SearchTagsTextBox
            // 
            this.SearchTagsTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ViewModelBindingSource, "TagSearch", true));
            this.SearchTagsTextBox.Location = new System.Drawing.Point(82, 32);
            this.SearchTagsTextBox.Name = "SearchTagsTextBox";
            this.SearchTagsTextBox.Size = new System.Drawing.Size(265, 20);
            this.SearchTagsTextBox.TabIndex = 4;
            this.SearchTagsTextBox.TextChanged += new System.EventHandler(this.SearchTagsTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Search tags";
            // 
            // ViewModelBindingSource
            // 
            this.ViewModelBindingSource.DataSource = typeof(eDream.GUI.DreamStatisticsViewModel);
            // 
            // tagDataGridViewTextBoxColumn
            // 
            this.tagDataGridViewTextBoxColumn.DataPropertyName = "TagDisplay";
            this.tagDataGridViewTextBoxColumn.HeaderText = "Tag Name";
            this.tagDataGridViewTextBoxColumn.Name = "tagDataGridViewTextBoxColumn";
            this.tagDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // countDataGridViewTextBoxColumn
            // 
            this.countDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.countDataGridViewTextBoxColumn.DataPropertyName = "Count";
            this.countDataGridViewTextBoxColumn.HeaderText = "Count";
            this.countDataGridViewTextBoxColumn.Name = "countDataGridViewTextBoxColumn";
            this.countDataGridViewTextBoxColumn.ReadOnly = true;
            this.countDataGridViewTextBoxColumn.Width = 60;
            // 
            // percentageDataGridViewTextBoxColumn
            // 
            this.percentageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.percentageDataGridViewTextBoxColumn.DataPropertyName = "PercentOfTotalEntriesDisplay";
            this.percentageDataGridViewTextBoxColumn.HeaderText = "% dreams with tag";
            this.percentageDataGridViewTextBoxColumn.Name = "percentageDataGridViewTextBoxColumn";
            this.percentageDataGridViewTextBoxColumn.ReadOnly = true;
            this.percentageDataGridViewTextBoxColumn.Width = 94;
            // 
            // FrmDreamStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = CloseButton;
            this.ClientSize = new System.Drawing.Size(700, 590);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SearchTagsTextBox);
            this.Controls.Add(this.ShowOnlyParentTagsCheckBox);
            this.Controls.Add(this.StatsTable);
            this.Controls.Add(CloseButton);
            this.Controls.Add(this.TotalDreamsLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "FrmDreamStatistics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Your dreams statistics";
            ((System.ComponentModel.ISupportInitialize)(this.StatsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewModelBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TotalDreamsLabel;
        private System.Windows.Forms.DataGridView StatsTable;
        private System.Windows.Forms.CheckBox ShowOnlyParentTagsCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn percentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercentOfTotalDaysDisplay;
        private System.Windows.Forms.BindingSource ViewModelBindingSource;
        private System.Windows.Forms.TextBox SearchTagsTextBox;
        private System.Windows.Forms.Label label1;
    }
}
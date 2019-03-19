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
            this.totalDreamsLabel = new System.Windows.Forms.Label();
            this.StatsTable = new System.Windows.Forms.DataGridView();
            this.tagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PercentOfTotalDaysDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.childTagsCheck = new System.Windows.Forms.CheckBox();
            CloseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.StatsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
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
            // totalDreamsLabel
            // 
            this.totalDreamsLabel.AutoSize = true;
            this.totalDreamsLabel.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalDreamsLabel.Location = new System.Drawing.Point(12, 9);
            this.totalDreamsLabel.Name = "totalDreamsLabel";
            this.totalDreamsLabel.Size = new System.Drawing.Size(366, 13);
            this.totalDreamsLabel.TabIndex = 0;
            this.totalDreamsLabel.Text = "There are no dream entries in your database right now";
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
            this.StatsTable.DataSource = this.BindingSource;
            this.StatsTable.Location = new System.Drawing.Point(15, 34);
            this.StatsTable.Name = "StatsTable";
            this.StatsTable.ReadOnly = true;
            this.StatsTable.Size = new System.Drawing.Size(673, 515);
            this.StatsTable.TabIndex = 2;
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
            // PercentOfTotalDaysDisplay
            // 
            this.PercentOfTotalDaysDisplay.DataPropertyName = "PercentOfTotalDaysDisplay";
            this.PercentOfTotalDaysDisplay.HeaderText = "% of days";
            this.PercentOfTotalDaysDisplay.Name = "PercentOfTotalDaysDisplay";
            this.PercentOfTotalDaysDisplay.ReadOnly = true;
            // 
            // BindingSource
            // 
            this.BindingSource.DataSource = typeof(eDream.libs.TagStatistic);
            // 
            // childTagsCheck
            // 
            this.childTagsCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.childTagsCheck.AutoSize = true;
            this.childTagsCheck.Checked = true;
            this.childTagsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.childTagsCheck.Location = new System.Drawing.Point(506, 559);
            this.childTagsCheck.Name = "childTagsCheck";
            this.childTagsCheck.Size = new System.Drawing.Size(101, 17);
            this.childTagsCheck.TabIndex = 3;
            this.childTagsCheck.Text = "Show child tags";
            this.childTagsCheck.UseVisualStyleBackColor = true;
            // 
            // FrmDreamStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = CloseButton;
            this.ClientSize = new System.Drawing.Size(700, 590);
            this.Controls.Add(this.childTagsCheck);
            this.Controls.Add(this.StatsTable);
            this.Controls.Add(CloseButton);
            this.Controls.Add(this.totalDreamsLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "FrmDreamStatistics";
            this.Text = "Your dreams statistics";
            ((System.ComponentModel.ISupportInitialize)(this.StatsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label totalDreamsLabel;
        private System.Windows.Forms.DataGridView StatsTable;
        private System.Windows.Forms.CheckBox childTagsCheck;
        private System.Windows.Forms.BindingSource BindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn percentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercentOfTotalDaysDisplay;
    }
}
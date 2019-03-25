namespace eDream.GUI {
    partial class FrmTagWizard {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTagWizard));
            this.TagsGrid = new System.Windows.Forms.DataGridView();
            this.tagDisplayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentOfTotalEntriesDisplayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSelectedEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.TagSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.TagsToAddTextBox = new System.Windows.Forms.TextBox();
            this.ViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ResetButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TagsGrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TagsGrid
            // 
            this.TagsGrid.AllowUserToAddRows = false;
            this.TagsGrid.AllowUserToDeleteRows = false;
            this.TagsGrid.AllowUserToOrderColumns = true;
            this.TagsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagsGrid.AutoGenerateColumns = false;
            this.TagsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TagsGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.TagsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.TagsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TagsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tagDisplayDataGridViewTextBoxColumn,
            this.countDataGridViewTextBoxColumn,
            this.percentOfTotalEntriesDisplayDataGridViewTextBoxColumn});
            this.TagsGrid.ContextMenuStrip = this.contextMenuStrip1;
            this.TagsGrid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TagsGrid.DataSource = this.BindingSource;
            this.TagsGrid.Location = new System.Drawing.Point(12, 69);
            this.TagsGrid.Name = "TagsGrid";
            this.TagsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TagsGrid.Size = new System.Drawing.Size(714, 319);
            this.TagsGrid.TabIndex = 0;
            // 
            // tagDisplayDataGridViewTextBoxColumn
            // 
            this.tagDisplayDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.tagDisplayDataGridViewTextBoxColumn.DataPropertyName = "TagDisplay";
            this.tagDisplayDataGridViewTextBoxColumn.HeaderText = "Tag";
            this.tagDisplayDataGridViewTextBoxColumn.Name = "tagDisplayDataGridViewTextBoxColumn";
            this.tagDisplayDataGridViewTextBoxColumn.ReadOnly = true;
            this.tagDisplayDataGridViewTextBoxColumn.Width = 51;
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
            // percentOfTotalEntriesDisplayDataGridViewTextBoxColumn
            // 
            this.percentOfTotalEntriesDisplayDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.percentOfTotalEntriesDisplayDataGridViewTextBoxColumn.DataPropertyName = "PercentOfTotalEntriesDisplay";
            this.percentOfTotalEntriesDisplayDataGridViewTextBoxColumn.HeaderText = "% total dreams";
            this.percentOfTotalEntriesDisplayDataGridViewTextBoxColumn.Name = "percentOfTotalEntriesDisplayDataGridViewTextBoxColumn";
            this.percentOfTotalEntriesDisplayDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSelectedEntriesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 26);
            // 
            // addSelectedEntriesToolStripMenuItem
            // 
            this.addSelectedEntriesToolStripMenuItem.Name = "addSelectedEntriesToolStripMenuItem";
            this.addSelectedEntriesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addSelectedEntriesToolStripMenuItem.Text = "Add selected entries";
            this.addSelectedEntriesToolStripMenuItem.Click += new System.EventHandler(this.AddSelectedEntriesToolStripMenuItem_Click);
            // 
            // BindingSource
            // 
            this.BindingSource.DataSource = typeof(eDream.libs.TagStatistic);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search tags";
            // 
            // TagSearch
            // 
            this.TagSearch.Location = new System.Drawing.Point(15, 25);
            this.TagSearch.Name = "TagSearch";
            this.TagSearch.Size = new System.Drawing.Size(151, 20);
            this.TagSearch.TabIndex = 2;
            this.TagSearch.TextChanged += new System.EventHandler(this.TagSearch_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 391);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Dream tags to add";
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(651, 433);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // TagsToAddTextBox
            // 
            this.TagsToAddTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagsToAddTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ViewModelBindingSource, "TagsToAdd", true));
            this.TagsToAddTextBox.Location = new System.Drawing.Point(15, 407);
            this.TagsToAddTextBox.Name = "TagsToAddTextBox";
            this.TagsToAddTextBox.Size = new System.Drawing.Size(711, 20);
            this.TagsToAddTextBox.TabIndex = 5;
            // 
            // ViewModelBindingSource
            // 
            this.ViewModelBindingSource.DataSource = typeof(eDream.GUI.TagWizardViewModel);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetButton.Location = new System.Drawing.Point(581, 433);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(64, 23);
            this.ResetButton.TabIndex = 9;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(220, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Double-click on a row to tag the dream with it";
            // 
            // ClearButton
            // 
            this.ClearButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ClearButton.Location = new System.Drawing.Point(172, 22);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(55, 23);
            this.ClearButton.TabIndex = 8;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // FrmTagWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 463);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.TagsToAddTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TagSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TagsGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "FrmTagWizard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Creator";
            ((System.ComponentModel.ISupportInitialize)(this.TagsGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewModelBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView TagsGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TagSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.TextBox TagsToAddTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addSelectedEntriesToolStripMenuItem;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.BindingSource BindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagDisplayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn percentOfTotalEntriesDisplayDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource ViewModelBindingSource;
        private System.Windows.Forms.Button ClearButton;
    }
}
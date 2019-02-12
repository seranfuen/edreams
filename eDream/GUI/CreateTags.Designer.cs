namespace eDream.GUI {
    partial class CreateTags {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateTags));
            this.tagsTable = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSelectedEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.tagSearch = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tagBox = new System.Windows.Forms.TextBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.displayChildTagsCheck = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tagsTable)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagsTable
            // 
            this.tagsTable.AllowUserToAddRows = false;
            this.tagsTable.AllowUserToDeleteRows = false;
            this.tagsTable.AllowUserToOrderColumns = true;
            this.tagsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tagsTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.tagsTable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.tagsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tagsTable.ContextMenuStrip = this.contextMenuStrip1;
            this.tagsTable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tagsTable.Location = new System.Drawing.Point(12, 51);
            this.tagsTable.Name = "tagsTable";
            this.tagsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tagsTable.Size = new System.Drawing.Size(393, 216);
            this.tagsTable.TabIndex = 0;
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
            this.addSelectedEntriesToolStripMenuItem.Click += new System.EventHandler(this.addSelectedEntriesToolStripMenuItem_Click);
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
            // tagSearch
            // 
            this.tagSearch.Location = new System.Drawing.Point(15, 25);
            this.tagSearch.Name = "tagSearch";
            this.tagSearch.Size = new System.Drawing.Size(151, 20);
            this.tagSearch.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.Image = ((System.Drawing.Image)(resources.GetObject("searchButton.Image")));
            this.searchButton.Location = new System.Drawing.Point(172, 23);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(25, 22);
            this.searchButton.TabIndex = 3;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 270);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Entry tags";
            // 
            // createButton
            // 
            this.createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.createButton.Location = new System.Drawing.Point(330, 312);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 6;
            this.createButton.Text = "Close";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(203, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tagBox
            // 
            this.tagBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tagBox.Location = new System.Drawing.Point(15, 286);
            this.tagBox.Name = "tagBox";
            this.tagBox.Size = new System.Drawing.Size(390, 20);
            this.tagBox.TabIndex = 5;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(260, 312);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(64, 23);
            this.buttonReset.TabIndex = 9;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // displayChildTagsCheck
            // 
            this.displayChildTagsCheck.AutoSize = true;
            this.displayChildTagsCheck.Location = new System.Drawing.Point(264, 25);
            this.displayChildTagsCheck.Name = "displayChildTagsCheck";
            this.displayChildTagsCheck.Size = new System.Drawing.Size(108, 17);
            this.displayChildTagsCheck.TabIndex = 10;
            this.displayChildTagsCheck.Text = "Display child tags";
            this.displayChildTagsCheck.UseVisualStyleBackColor = true;
            this.displayChildTagsCheck.CheckedChanged += new System.EventHandler(this.displayChildTagsCheck_CheckedChanged);
            // 
            // CreateTags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.createButton;
            this.ClientSize = new System.Drawing.Size(417, 342);
            this.Controls.Add(this.displayChildTagsCheck);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.tagBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.tagSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tagsTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "CreateTags";
            this.ShowInTaskbar = false;
            this.Text = "Tag Creator";
            ((System.ComponentModel.ISupportInitialize)(this.tagsTable)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView tagsTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tagSearch;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tagBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addSelectedEntriesToolStripMenuItem;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.CheckBox displayChildTagsCheck;

    }
}
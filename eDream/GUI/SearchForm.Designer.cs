namespace eDream.GUI {
    partial class SearchForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.searchTabs = new System.Windows.Forms.TabControl();
            this.textSearchPage = new System.Windows.Forms.TabPage();
            this.textClearButton = new System.Windows.Forms.Button();
            this.textSearchFindButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textSearchBox = new System.Windows.Forms.TextBox();
            this.tagsSearchPage = new System.Windows.Forms.TabPage();
            this.orRadio = new System.Windows.Forms.RadioButton();
            this.andRadio = new System.Windows.Forms.RadioButton();
            this.clearTagsButton = new System.Windows.Forms.Button();
            this.findTagsButton = new System.Windows.Forms.Button();
            this.checkChildTags = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tagsTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateSearchPage = new System.Windows.Forms.TabPage();
            this.dateClearButton = new System.Windows.Forms.Button();
            this.findDateButton = new System.Windows.Forms.Button();
            this.toTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.fromTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromDateLabel = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.searchTabs.SuspendLayout();
            this.textSearchPage.SuspendLayout();
            this.tagsSearchPage.SuspendLayout();
            this.dateSearchPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchTabs
            // 
            this.searchTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTabs.Controls.Add(this.textSearchPage);
            this.searchTabs.Controls.Add(this.tagsSearchPage);
            this.searchTabs.Controls.Add(this.dateSearchPage);
            this.searchTabs.Location = new System.Drawing.Point(1, 1);
            this.searchTabs.Name = "searchTabs";
            this.searchTabs.SelectedIndex = 0;
            this.searchTabs.Size = new System.Drawing.Size(372, 141);
            this.searchTabs.TabIndex = 0;
            // 
            // textSearchPage
            // 
            this.textSearchPage.BackColor = System.Drawing.SystemColors.Control;
            this.textSearchPage.Controls.Add(this.textClearButton);
            this.textSearchPage.Controls.Add(this.textSearchFindButton);
            this.textSearchPage.Controls.Add(this.label1);
            this.textSearchPage.Controls.Add(this.textSearchBox);
            this.textSearchPage.Location = new System.Drawing.Point(4, 22);
            this.textSearchPage.Name = "textSearchPage";
            this.textSearchPage.Padding = new System.Windows.Forms.Padding(3);
            this.textSearchPage.Size = new System.Drawing.Size(364, 115);
            this.textSearchPage.TabIndex = 0;
            this.textSearchPage.Text = "Text";
            // 
            // textClearButton
            // 
            this.textClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textClearButton.Location = new System.Drawing.Point(239, 86);
            this.textClearButton.Name = "textClearButton";
            this.textClearButton.Size = new System.Drawing.Size(56, 23);
            this.textClearButton.TabIndex = 8;
            this.textClearButton.Text = "Clear";
            this.textClearButton.UseVisualStyleBackColor = true;
            // 
            // textSearchFindButton
            // 
            this.textSearchFindButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textSearchFindButton.Location = new System.Drawing.Point(301, 86);
            this.textSearchFindButton.Name = "textSearchFindButton";
            this.textSearchFindButton.Size = new System.Drawing.Size(53, 23);
            this.textSearchFindButton.TabIndex = 7;
            this.textSearchFindButton.Text = "Find";
            this.textSearchFindButton.UseVisualStyleBackColor = true;
            this.textSearchFindButton.Click += new System.EventHandler(this.textSearchFindButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Find entries that contain the following text";
            // 
            // textSearchBox
            // 
            this.textSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSearchBox.Location = new System.Drawing.Point(9, 21);
            this.textSearchBox.Name = "textSearchBox";
            this.textSearchBox.Size = new System.Drawing.Size(345, 20);
            this.textSearchBox.TabIndex = 0;
            // 
            // tagsSearchPage
            // 
            this.tagsSearchPage.BackColor = System.Drawing.SystemColors.Control;
            this.tagsSearchPage.Controls.Add(this.orRadio);
            this.tagsSearchPage.Controls.Add(this.andRadio);
            this.tagsSearchPage.Controls.Add(this.clearTagsButton);
            this.tagsSearchPage.Controls.Add(this.findTagsButton);
            this.tagsSearchPage.Controls.Add(this.checkChildTags);
            this.tagsSearchPage.Controls.Add(this.richTextBox1);
            this.tagsSearchPage.Controls.Add(this.tagsTextBox);
            this.tagsSearchPage.Controls.Add(this.label2);
            this.tagsSearchPage.Location = new System.Drawing.Point(4, 22);
            this.tagsSearchPage.Name = "tagsSearchPage";
            this.tagsSearchPage.Padding = new System.Windows.Forms.Padding(3);
            this.tagsSearchPage.Size = new System.Drawing.Size(364, 115);
            this.tagsSearchPage.TabIndex = 1;
            this.tagsSearchPage.Text = "Tags";
            // 
            // orRadio
            // 
            this.orRadio.AutoSize = true;
            this.orRadio.Location = new System.Drawing.Point(10, 95);
            this.orRadio.Name = "orRadio";
            this.orRadio.Size = new System.Drawing.Size(41, 17);
            this.orRadio.TabIndex = 8;
            this.orRadio.TabStop = true;
            this.orRadio.Text = "OR";
            this.orRadio.UseVisualStyleBackColor = true;
            // 
            // andRadio
            // 
            this.andRadio.AutoSize = true;
            this.andRadio.Location = new System.Drawing.Point(10, 77);
            this.andRadio.Name = "andRadio";
            this.andRadio.Size = new System.Drawing.Size(48, 17);
            this.andRadio.TabIndex = 7;
            this.andRadio.TabStop = true;
            this.andRadio.Text = "AND";
            this.andRadio.UseVisualStyleBackColor = true;
            // 
            // clearTagsButton
            // 
            this.clearTagsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearTagsButton.Location = new System.Drawing.Point(239, 86);
            this.clearTagsButton.Name = "clearTagsButton";
            this.clearTagsButton.Size = new System.Drawing.Size(56, 23);
            this.clearTagsButton.TabIndex = 6;
            this.clearTagsButton.Text = "Clear";
            this.clearTagsButton.UseVisualStyleBackColor = true;
            // 
            // findTagsButton
            // 
            this.findTagsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findTagsButton.Location = new System.Drawing.Point(301, 86);
            this.findTagsButton.Name = "findTagsButton";
            this.findTagsButton.Size = new System.Drawing.Size(53, 23);
            this.findTagsButton.TabIndex = 5;
            this.findTagsButton.Text = "Find";
            this.findTagsButton.UseVisualStyleBackColor = true;
            this.findTagsButton.Click += new System.EventHandler(this.FindTagsButton_Click);
            // 
            // checkChildTags
            // 
            this.checkChildTags.AutoSize = true;
            this.checkChildTags.Location = new System.Drawing.Point(64, 86);
            this.checkChildTags.Name = "checkChildTags";
            this.checkChildTags.Size = new System.Drawing.Size(109, 17);
            this.checkChildTags.TabIndex = 4;
            this.checkChildTags.Text = "Include child tags";
            this.checkChildTags.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(10, 45);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(299, 32);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "Search logic: AND returns entries that have all the tags. \nOR entries that have a" +
    "t least one tag";
            // 
            // tagsTextBox
            // 
            this.tagsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagsTextBox.Location = new System.Drawing.Point(10, 19);
            this.tagsTextBox.Name = "tagsTextBox";
            this.tagsTextBox.Size = new System.Drawing.Size(344, 20);
            this.tagsTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(277, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Find dreams with the following tags (separate by commas)";
            // 
            // dateSearchPage
            // 
            this.dateSearchPage.BackColor = System.Drawing.SystemColors.Control;
            this.dateSearchPage.Controls.Add(this.dateClearButton);
            this.dateSearchPage.Controls.Add(this.findDateButton);
            this.dateSearchPage.Controls.Add(this.toTimePicker);
            this.dateSearchPage.Controls.Add(this.label3);
            this.dateSearchPage.Controls.Add(this.fromTimePicker);
            this.dateSearchPage.Controls.Add(this.fromDateLabel);
            this.dateSearchPage.Controls.Add(this.checkBox2);
            this.dateSearchPage.Location = new System.Drawing.Point(4, 22);
            this.dateSearchPage.Name = "dateSearchPage";
            this.dateSearchPage.Padding = new System.Windows.Forms.Padding(3);
            this.dateSearchPage.Size = new System.Drawing.Size(364, 115);
            this.dateSearchPage.TabIndex = 2;
            this.dateSearchPage.Text = "Date";
            // 
            // dateClearButton
            // 
            this.dateClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateClearButton.Location = new System.Drawing.Point(239, 86);
            this.dateClearButton.Name = "dateClearButton";
            this.dateClearButton.Size = new System.Drawing.Size(56, 23);
            this.dateClearButton.TabIndex = 8;
            this.dateClearButton.Text = "Clear";
            this.dateClearButton.UseVisualStyleBackColor = true;
            // 
            // findDateButton
            // 
            this.findDateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findDateButton.Location = new System.Drawing.Point(301, 86);
            this.findDateButton.Name = "findDateButton";
            this.findDateButton.Size = new System.Drawing.Size(53, 23);
            this.findDateButton.TabIndex = 7;
            this.findDateButton.Text = "Find";
            this.findDateButton.UseVisualStyleBackColor = true;
            this.findDateButton.Click += new System.EventHandler(this.Button2_Click);
            // 
            // toTimePicker
            // 
            this.toTimePicker.CustomFormat = "";
            this.toTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.toTimePicker.Location = new System.Drawing.Point(212, 23);
            this.toTimePicker.Name = "toTimePicker";
            this.toTimePicker.Size = new System.Drawing.Size(97, 20);
            this.toTimePicker.TabIndex = 4;
            this.toTimePicker.Value = new System.DateTime(2012, 1, 20, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "To";
            // 
            // fromTimePicker
            // 
            this.fromTimePicker.CustomFormat = "";
            this.fromTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromTimePicker.Location = new System.Drawing.Point(100, 23);
            this.fromTimePicker.Name = "fromTimePicker";
            this.fromTimePicker.Size = new System.Drawing.Size(97, 20);
            this.fromTimePicker.TabIndex = 2;
            this.fromTimePicker.Value = new System.DateTime(2012, 1, 20, 0, 0, 0, 0);
            // 
            // fromDateLabel
            // 
            this.fromDateLabel.AutoSize = true;
            this.fromDateLabel.Location = new System.Drawing.Point(97, 7);
            this.fromDateLabel.Name = "fromDateLabel";
            this.fromDateLabel.Size = new System.Drawing.Size(30, 13);
            this.fromDateLabel.TabIndex = 1;
            this.fromDateLabel.Text = "From";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(7, 23);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(75, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Single day";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 141);
            this.Controls.Add(this.searchTabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(342, 179);
            this.Name = "SearchForm";
            this.Text = "Search";
            this.TopMost = true;
            this.searchTabs.ResumeLayout(false);
            this.textSearchPage.ResumeLayout(false);
            this.textSearchPage.PerformLayout();
            this.tagsSearchPage.ResumeLayout(false);
            this.tagsSearchPage.PerformLayout();
            this.dateSearchPage.ResumeLayout(false);
            this.dateSearchPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl searchTabs;
        private System.Windows.Forms.TabPage textSearchPage;
        private System.Windows.Forms.TabPage tagsSearchPage;
        private System.Windows.Forms.TextBox textSearchBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tagsTextBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkChildTags;
        private System.Windows.Forms.Button clearTagsButton;
        private System.Windows.Forms.Button findTagsButton;
        private System.Windows.Forms.TabPage dateSearchPage;
        private System.Windows.Forms.DateTimePicker fromTimePicker;
        private System.Windows.Forms.Label fromDateLabel;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.DateTimePicker toTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button textClearButton;
        private System.Windows.Forms.Button textSearchFindButton;
        private System.Windows.Forms.Button dateClearButton;
        private System.Windows.Forms.Button findDateButton;
        private System.Windows.Forms.RadioButton andRadio;
        private System.Windows.Forms.RadioButton orRadio;
    }
}
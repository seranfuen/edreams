namespace eDream.GUI {
    partial class FrmSearchForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearchForm));
            this.searchTabs = new System.Windows.Forms.TabControl();
            this.textSearchPage = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.TextClearButton = new System.Windows.Forms.Button();
            this.TextSearchFindButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextSearchBox = new System.Windows.Forms.TextBox();
            this.tagsSearchPage = new System.Windows.Forms.TabPage();
            this.orRadio = new System.Windows.Forms.RadioButton();
            this.andRadio = new System.Windows.Forms.RadioButton();
            this.ClearTagsButton = new System.Windows.Forms.Button();
            this.FindTagsButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tagsTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateSearchPage = new System.Windows.Forms.TabPage();
            this.DateClearButton = new System.Windows.Forms.Button();
            this.FindDateButton = new System.Windows.Forms.Button();
            this.ToTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.FromTimePicker = new System.Windows.Forms.DateTimePicker();
            this.fromDateLabel = new System.Windows.Forms.Label();
            this.SingleDayCheckBox = new System.Windows.Forms.CheckBox();
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
            this.searchTabs.Size = new System.Drawing.Size(463, 141);
            this.searchTabs.TabIndex = 0;
            // 
            // textSearchPage
            // 
            this.textSearchPage.BackColor = System.Drawing.SystemColors.Control;
            this.textSearchPage.Controls.Add(this.label4);
            this.textSearchPage.Controls.Add(this.TextClearButton);
            this.textSearchPage.Controls.Add(this.TextSearchFindButton);
            this.textSearchPage.Controls.Add(this.label1);
            this.textSearchPage.Controls.Add(this.TextSearchBox);
            this.textSearchPage.Location = new System.Drawing.Point(4, 22);
            this.textSearchPage.Name = "textSearchPage";
            this.textSearchPage.Padding = new System.Windows.Forms.Padding(3);
            this.textSearchPage.Size = new System.Drawing.Size(455, 115);
            this.textSearchPage.TabIndex = 0;
            this.textSearchPage.Text = "Text";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(229, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "To find a phrase, put it between quotes \"a b c\"";
            // 
            // TextClearButton
            // 
            this.TextClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextClearButton.Location = new System.Drawing.Point(330, 86);
            this.TextClearButton.Name = "TextClearButton";
            this.TextClearButton.Size = new System.Drawing.Size(56, 23);
            this.TextClearButton.TabIndex = 8;
            this.TextClearButton.Text = "Clear";
            this.TextClearButton.UseVisualStyleBackColor = true;
            // 
            // TextSearchFindButton
            // 
            this.TextSearchFindButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextSearchFindButton.Location = new System.Drawing.Point(392, 86);
            this.TextSearchFindButton.Name = "TextSearchFindButton";
            this.TextSearchFindButton.Size = new System.Drawing.Size(53, 23);
            this.TextSearchFindButton.TabIndex = 7;
            this.TextSearchFindButton.Text = "Find";
            this.TextSearchFindButton.UseVisualStyleBackColor = true;
            this.TextSearchFindButton.Click += new System.EventHandler(this.TextSearchFindButton_Click);
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
            // TextSearchBox
            // 
            this.TextSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextSearchBox.Location = new System.Drawing.Point(9, 42);
            this.TextSearchBox.Name = "TextSearchBox";
            this.TextSearchBox.Size = new System.Drawing.Size(436, 20);
            this.TextSearchBox.TabIndex = 0;
            // 
            // tagsSearchPage
            // 
            this.tagsSearchPage.BackColor = System.Drawing.SystemColors.Control;
            this.tagsSearchPage.Controls.Add(this.orRadio);
            this.tagsSearchPage.Controls.Add(this.andRadio);
            this.tagsSearchPage.Controls.Add(this.ClearTagsButton);
            this.tagsSearchPage.Controls.Add(this.FindTagsButton);
            this.tagsSearchPage.Controls.Add(this.richTextBox1);
            this.tagsSearchPage.Controls.Add(this.tagsTextBox);
            this.tagsSearchPage.Controls.Add(this.label2);
            this.tagsSearchPage.Location = new System.Drawing.Point(4, 22);
            this.tagsSearchPage.Name = "tagsSearchPage";
            this.tagsSearchPage.Padding = new System.Windows.Forms.Padding(3);
            this.tagsSearchPage.Size = new System.Drawing.Size(455, 115);
            this.tagsSearchPage.TabIndex = 1;
            this.tagsSearchPage.Text = "Tags";
            // 
            // orRadio
            // 
            this.orRadio.AutoSize = true;
            this.orRadio.Location = new System.Drawing.Point(64, 77);
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
            // ClearTagsButton
            // 
            this.ClearTagsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearTagsButton.Location = new System.Drawing.Point(330, 86);
            this.ClearTagsButton.Name = "ClearTagsButton";
            this.ClearTagsButton.Size = new System.Drawing.Size(56, 23);
            this.ClearTagsButton.TabIndex = 6;
            this.ClearTagsButton.Text = "Clear";
            this.ClearTagsButton.UseVisualStyleBackColor = true;
            // 
            // FindTagsButton
            // 
            this.FindTagsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindTagsButton.Location = new System.Drawing.Point(392, 86);
            this.FindTagsButton.Name = "FindTagsButton";
            this.FindTagsButton.Size = new System.Drawing.Size(53, 23);
            this.FindTagsButton.TabIndex = 5;
            this.FindTagsButton.Text = "Find";
            this.FindTagsButton.UseVisualStyleBackColor = true;
            this.FindTagsButton.Click += new System.EventHandler(this.FindTagsButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(10, 45);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(435, 32);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "Search logic: AND returns dreams that contain ALL THE TAGS provided\nOR returns dr" +
    "eams that contain AT LEAST ONE TAG provided";
            // 
            // tagsTextBox
            // 
            this.tagsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagsTextBox.Location = new System.Drawing.Point(10, 19);
            this.tagsTextBox.Name = "tagsTextBox";
            this.tagsTextBox.Size = new System.Drawing.Size(435, 20);
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
            this.dateSearchPage.Controls.Add(this.DateClearButton);
            this.dateSearchPage.Controls.Add(this.FindDateButton);
            this.dateSearchPage.Controls.Add(this.ToTimePicker);
            this.dateSearchPage.Controls.Add(this.label3);
            this.dateSearchPage.Controls.Add(this.FromTimePicker);
            this.dateSearchPage.Controls.Add(this.fromDateLabel);
            this.dateSearchPage.Controls.Add(this.SingleDayCheckBox);
            this.dateSearchPage.Location = new System.Drawing.Point(4, 22);
            this.dateSearchPage.Name = "dateSearchPage";
            this.dateSearchPage.Padding = new System.Windows.Forms.Padding(3);
            this.dateSearchPage.Size = new System.Drawing.Size(364, 115);
            this.dateSearchPage.TabIndex = 2;
            this.dateSearchPage.Text = "Date";
            // 
            // DateClearButton
            // 
            this.DateClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DateClearButton.Location = new System.Drawing.Point(239, 86);
            this.DateClearButton.Name = "DateClearButton";
            this.DateClearButton.Size = new System.Drawing.Size(56, 23);
            this.DateClearButton.TabIndex = 8;
            this.DateClearButton.Text = "Clear";
            this.DateClearButton.UseVisualStyleBackColor = true;
            // 
            // FindDateButton
            // 
            this.FindDateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindDateButton.Location = new System.Drawing.Point(301, 86);
            this.FindDateButton.Name = "FindDateButton";
            this.FindDateButton.Size = new System.Drawing.Size(53, 23);
            this.FindDateButton.TabIndex = 7;
            this.FindDateButton.Text = "Find";
            this.FindDateButton.UseVisualStyleBackColor = true;
            this.FindDateButton.Click += new System.EventHandler(this.FindDateButton_Click);
            // 
            // ToTimePicker
            // 
            this.ToTimePicker.CustomFormat = "";
            this.ToTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ToTimePicker.Location = new System.Drawing.Point(212, 23);
            this.ToTimePicker.Name = "ToTimePicker";
            this.ToTimePicker.Size = new System.Drawing.Size(97, 20);
            this.ToTimePicker.TabIndex = 4;
            this.ToTimePicker.Value = new System.DateTime(2012, 1, 20, 0, 0, 0, 0);
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
            // FromTimePicker
            // 
            this.FromTimePicker.CustomFormat = "";
            this.FromTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.FromTimePicker.Location = new System.Drawing.Point(100, 23);
            this.FromTimePicker.Name = "FromTimePicker";
            this.FromTimePicker.Size = new System.Drawing.Size(97, 20);
            this.FromTimePicker.TabIndex = 2;
            this.FromTimePicker.Value = new System.DateTime(2012, 1, 20, 0, 0, 0, 0);
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
            // SingleDayCheckBox
            // 
            this.SingleDayCheckBox.AutoSize = true;
            this.SingleDayCheckBox.Location = new System.Drawing.Point(7, 23);
            this.SingleDayCheckBox.Name = "SingleDayCheckBox";
            this.SingleDayCheckBox.Size = new System.Drawing.Size(75, 17);
            this.SingleDayCheckBox.TabIndex = 0;
            this.SingleDayCheckBox.Text = "Single day";
            this.SingleDayCheckBox.UseVisualStyleBackColor = true;
            this.SingleDayCheckBox.CheckedChanged += new System.EventHandler(this.SingleDayCheckBox_CheckedChanged);
            // 
            // FrmSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 141);
            this.Controls.Add(this.searchTabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(342, 179);
            this.Name = "FrmSearchForm";
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
        private System.Windows.Forms.TextBox TextSearchBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tagsTextBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button ClearTagsButton;
        private System.Windows.Forms.Button FindTagsButton;
        private System.Windows.Forms.TabPage dateSearchPage;
        private System.Windows.Forms.DateTimePicker FromTimePicker;
        private System.Windows.Forms.Label fromDateLabel;
        private System.Windows.Forms.CheckBox SingleDayCheckBox;
        private System.Windows.Forms.DateTimePicker ToTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button TextClearButton;
        private System.Windows.Forms.Button TextSearchFindButton;
        private System.Windows.Forms.Button DateClearButton;
        private System.Windows.Forms.Button FindDateButton;
        private System.Windows.Forms.RadioButton andRadio;
        private System.Windows.Forms.RadioButton orRadio;
        private System.Windows.Forms.Label label4;
    }
}
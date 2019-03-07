namespace eDream.GUI {
    partial class CtrEntryViewer {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.wrapperBox = new System.Windows.Forms.GroupBox();
            this.tagsLabel = new System.Windows.Forms.TextBox();
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.entryStatusLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.DreamText = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.wrapperBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 309);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.wrapperBox);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(30, 17, 30, 16);
            this.panel2.Size = new System.Drawing.Size(654, 322);
            this.panel2.TabIndex = 0;
            // 
            // wrapperBox
            // 
            this.wrapperBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wrapperBox.Controls.Add(this.tagsLabel);
            this.wrapperBox.Controls.Add(this.label2);
            this.wrapperBox.Controls.Add(this.dateLabel);
            this.wrapperBox.Controls.Add(this.label1);
            this.wrapperBox.Controls.Add(this.entryStatusLabel);
            this.wrapperBox.Controls.Add(this.DeleteButton);
            this.wrapperBox.Controls.Add(this.EditButton);
            this.wrapperBox.Controls.Add(this.DreamText);
            this.wrapperBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSource, "EntryNumber", true));
            this.wrapperBox.Location = new System.Drawing.Point(13, 3);
            this.wrapperBox.Name = "wrapperBox";
            this.wrapperBox.Padding = new System.Windows.Forms.Padding(6);
            this.wrapperBox.Size = new System.Drawing.Size(628, 306);
            this.wrapperBox.TabIndex = 0;
            this.wrapperBox.TabStop = false;
            this.wrapperBox.Text = "Entry 1";
            // 
            // tagsLabel
            // 
            this.tagsLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tagsLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSource, "DreamTags", true));
            this.tagsLabel.Location = new System.Drawing.Point(52, 42);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.ReadOnly = true;
            this.tagsLabel.Size = new System.Drawing.Size(524, 13);
            this.tagsLabel.TabIndex = 15;
            this.tagsLabel.Text = "No tags";
            // 
            // BindingSource
            // 
            this.BindingSource.DataSource = typeof(eDream.GUI.EntryViewerModel);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Tags";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSource, "DreamDate", true));
            this.dateLabel.Location = new System.Drawing.Point(49, 19);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(45, 13);
            this.dateLabel.TabIndex = 13;
            this.dateLabel.Text = "No date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Date";
            // 
            // entryStatusLabel
            // 
            this.entryStatusLabel.AutoSize = true;
            this.entryStatusLabel.Location = new System.Drawing.Point(130, 388);
            this.entryStatusLabel.Name = "entryStatusLabel";
            this.entryStatusLabel.Size = new System.Drawing.Size(0, 13);
            this.entryStatusLabel.TabIndex = 11;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(67, 276);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(49, 23);
            this.DeleteButton.TabIndex = 10;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(12, 276);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(49, 23);
            this.EditButton.TabIndex = 9;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // DreamText
            // 
            this.DreamText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DreamText.AutoWordSelection = true;
            this.DreamText.BackColor = System.Drawing.SystemColors.Window;
            this.DreamText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSource, "DreamText", true));
            this.DreamText.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DreamText.Location = new System.Drawing.Point(12, 61);
            this.DreamText.Name = "DreamText";
            this.DreamText.ReadOnly = true;
            this.DreamText.Size = new System.Drawing.Size(604, 209);
            this.DreamText.TabIndex = 8;
            this.DreamText.Text = "";
            // 
            // CtrEntryViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(654, 0);
            this.Name = "CtrEntryViewer";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(654, 322);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.wrapperBox.ResumeLayout(false);
            this.wrapperBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox wrapperBox;
        private System.Windows.Forms.TextBox tagsLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label entryStatusLabel;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.RichTextBox DreamText;
        private System.Windows.Forms.BindingSource BindingSource;
    }
}

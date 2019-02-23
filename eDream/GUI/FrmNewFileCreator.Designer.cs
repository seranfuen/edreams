namespace eDream.GUI {
    partial class FrmNewFileCreator {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewFileCreator));
            this.label1 = new System.Windows.Forms.Label();
            this.NewFileTextBox = new System.Windows.Forms.TextBox();
            this.BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chooseFolderButton = new System.Windows.Forms.Button();
            this.FolderTextBox = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ChooseFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // NewFileTextBox
            // 
            this.NewFileTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSource, "FileName", true));
            this.NewFileTextBox.Location = new System.Drawing.Point(57, 6);
            this.NewFileTextBox.Name = "NewFileTextBox";
            this.NewFileTextBox.Size = new System.Drawing.Size(237, 20);
            this.NewFileTextBox.TabIndex = 1;
            // 
            // BindingSource
            // 
            this.BindingSource.DataSource = typeof(eDream.GUI.NewFileViewModel);
            // 
            // chooseFolderButton
            // 
            this.chooseFolderButton.Location = new System.Drawing.Point(15, 61);
            this.chooseFolderButton.Name = "chooseFolderButton";
            this.chooseFolderButton.Size = new System.Drawing.Size(89, 23);
            this.chooseFolderButton.TabIndex = 2;
            this.chooseFolderButton.Text = "Choose folder";
            this.chooseFolderButton.UseVisualStyleBackColor = true;
            this.chooseFolderButton.Click += new System.EventHandler(this.ChooseFolderButton_Click);
            // 
            // FolderTextBox
            // 
            this.FolderTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSource, "Folder", true));
            this.FolderTextBox.Location = new System.Drawing.Point(57, 34);
            this.FolderTextBox.Name = "FolderTextBox";
            this.FolderTextBox.ReadOnly = true;
            this.FolderTextBox.Size = new System.Drawing.Size(237, 20);
            this.FolderTextBox.TabIndex = 3;
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(219, 61);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 4;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(138, 61);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Folder";
            // 
            // ChooseFolderDialog
            // 
            this.ChooseFolderDialog.Description = "Choose or create a new folder";
            this.ChooseFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // FrmNewFileCreator
            // 
            this.AcceptButton = this.createButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(306, 94);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.FolderTextBox);
            this.Controls.Add(this.chooseFolderButton);
            this.Controls.Add(this.NewFileTextBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(322, 133);
            this.MinimizeBox = false;
            this.Name = "FrmNewFileCreator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a new dream database";
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NewFileTextBox;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.TextBox FolderTextBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog ChooseFolderDialog;
        private System.Windows.Forms.BindingSource BindingSource;
    }
}
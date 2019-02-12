namespace eDream.GUI {
    partial class Settings {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.checkLoadLastDB = new System.Windows.Forms.CheckBox();
            this.checkShowWelcomeWindow = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkLoadLastDB
            // 
            this.checkLoadLastDB.AutoSize = true;
            this.checkLoadLastDB.Location = new System.Drawing.Point(12, 12);
            this.checkLoadLastDB.Name = "checkLoadLastDB";
            this.checkLoadLastDB.Size = new System.Drawing.Size(245, 17);
            this.checkLoadLastDB.TabIndex = 0;
            this.checkLoadLastDB.Text = "Automatically load the last database on startup";
            this.checkLoadLastDB.UseVisualStyleBackColor = true;
            this.checkLoadLastDB.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkShowWelcomeWindow
            // 
            this.checkShowWelcomeWindow.AutoSize = true;
            this.checkShowWelcomeWindow.Location = new System.Drawing.Point(12, 35);
            this.checkShowWelcomeWindow.Name = "checkShowWelcomeWindow";
            this.checkShowWelcomeWindow.Size = new System.Drawing.Size(208, 17);
            this.checkShowWelcomeWindow.TabIndex = 1;
            this.checkShowWelcomeWindow.Text = "Show the Welcome window on startup";
            this.checkShowWelcomeWindow.UseVisualStyleBackColor = true;
            this.checkShowWelcomeWindow.Visible = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(231, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(42, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(163, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 136);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkShowWelcomeWindow);
            this.Controls.Add(this.checkLoadLastDB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(301, 174);
            this.MinimumSize = new System.Drawing.Size(301, 174);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkLoadLastDB;
        private System.Windows.Forms.CheckBox checkShowWelcomeWindow;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
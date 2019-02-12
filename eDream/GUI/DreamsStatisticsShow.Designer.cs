namespace eDream.GUI {
    partial class DreamsStatisticsShow {
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
            System.Windows.Forms.Button closeButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DreamsStatisticsShow));
            this.totalDreamsLabel = new System.Windows.Forms.Label();
            this.statsTable = new System.Windows.Forms.DataGridView();
            this.childTagsCheck = new System.Windows.Forms.CheckBox();
            closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.statsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            closeButton.Location = new System.Drawing.Point(354, 412);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(75, 23);
            closeButton.TabIndex = 1;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += new System.EventHandler(this.button1_Click);
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
            // statsTable
            // 
            this.statsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statsTable.Location = new System.Drawing.Point(15, 34);
            this.statsTable.Name = "statsTable";
            this.statsTable.Size = new System.Drawing.Size(414, 372);
            this.statsTable.TabIndex = 2;
            // 
            // childTagsCheck
            // 
            this.childTagsCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.childTagsCheck.AutoSize = true;
            this.childTagsCheck.Checked = true;
            this.childTagsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.childTagsCheck.Location = new System.Drawing.Point(247, 416);
            this.childTagsCheck.Name = "childTagsCheck";
            this.childTagsCheck.Size = new System.Drawing.Size(101, 17);
            this.childTagsCheck.TabIndex = 3;
            this.childTagsCheck.Text = "Show child tags";
            this.childTagsCheck.UseVisualStyleBackColor = true;
            this.childTagsCheck.CheckedChanged += new System.EventHandler(this.childTagsCheck_CheckedChanged);
            // 
            // DreamsStatisticsShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = closeButton;
            this.ClientSize = new System.Drawing.Size(441, 447);
            this.Controls.Add(this.childTagsCheck);
            this.Controls.Add(this.statsTable);
            this.Controls.Add(closeButton);
            this.Controls.Add(this.totalDreamsLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "DreamsStatisticsShow";
            this.Text = "Your dreams statistics";
            ((System.ComponentModel.ISupportInitialize)(this.statsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label totalDreamsLabel;
        private System.Windows.Forms.DataGridView statsTable;
        private System.Windows.Forms.CheckBox childTagsCheck;
    }
}
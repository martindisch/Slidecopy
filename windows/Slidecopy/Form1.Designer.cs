namespace Slidecopy
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.labelCode = new System.Windows.Forms.Label();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.labelDoubleClick = new System.Windows.Forms.Label();
            this.contextMenuStripIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStripIcon;
            this.notifyIcon.Text = "Slidecopy";
            this.notifyIcon.Visible = true;
            // 
            // contextMenuStripIcon
            // 
            this.contextMenuStripIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSettings,
            this.toolStripMenuItemExit});
            this.contextMenuStripIcon.Name = "contextMenuStripIcon";
            this.contextMenuStripIcon.Size = new System.Drawing.Size(117, 48);
            // 
            // toolStripMenuItemSettings
            // 
            this.toolStripMenuItemSettings.Name = "toolStripMenuItemSettings";
            this.toolStripMenuItemSettings.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemSettings.Text = "Settings";
            this.toolStripMenuItemSettings.Click += new System.EventHandler(this.toolStripMenuItemSettings_Click);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemExit.Text = "Exit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(13, 13);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(166, 13);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Unique code (same as in Android)";
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(16, 29);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(110, 20);
            this.textBoxCode.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(132, 27);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(13, 57);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(51, 13);
            this.labelIP.TabIndex = 4;
            this.labelIP.Text = "Server IP";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(16, 73);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(110, 20);
            this.textBoxIP.TabIndex = 1;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(133, 56);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(59, 13);
            this.labelPort.TabIndex = 6;
            this.labelPort.Text = "Server port";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(136, 73);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(71, 20);
            this.textBoxPort.TabIndex = 2;
            // 
            // labelDoubleClick
            // 
            this.labelDoubleClick.Location = new System.Drawing.Point(13, 105);
            this.labelDoubleClick.Name = "labelDoubleClick";
            this.labelDoubleClick.Size = new System.Drawing.Size(194, 47);
            this.labelDoubleClick.TabIndex = 7;
            this.labelDoubleClick.Text = "Double click the icon in the notification area to copy your current photo to clip" +
    "board";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 161);
            this.Controls.Add(this.labelDoubleClick);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.labelCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Slidecopy";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.contextMenuStripIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripIcon;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelDoubleClick;
    }
}


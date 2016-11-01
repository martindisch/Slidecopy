using System;
using System.Drawing;
using System.Windows.Forms;

namespace Slidecopy
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
            this.Resize += new EventHandler(FormSettings_Resize);
        }

        void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // TODO: Download & clipboard
        }

        void FormSettings_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) Hide();
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void toolStripMenuItemSettings_Click(object sender, EventArgs e)
        {
            Show();
            this.BringToFront();
            this.WindowState = FormWindowState.Normal;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }
    }
}

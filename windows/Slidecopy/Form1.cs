using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Slidecopy
{
    public partial class FormSettings : Form
    {
        private bool gotData = false;

        public FormSettings()
        {
            InitializeComponent();
            // initialize notifyIcon
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);

            // if they exist, load previously saved values from registry
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Slidecopy");
            if (regKey != null)
            {
                string code = (string) regKey.GetValue("code");
                string ip = (string) regKey.GetValue("ip");
                string port = (string) regKey.GetValue("port");
                textBoxCode.Text = code;
                textBoxIP.Text = ip;
                textBoxPort.Text = port;
                gotData = true;
            } else
            {
                // otherwise, let user enter required data
                this.WindowState = FormWindowState.Normal;
            }
        }

        void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (gotData)
            {
                // download image

            } else
            {
                // if no server settings available, show input form to user
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void toolStripMenuItemSettings_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // check input for errors
            string code = textBoxCode.Text;
            string ip = textBoxIP.Text;
            string port = textBoxPort.Text;
            Regex rgxPositiveInt = new Regex(@"^[1-9]\d*$");
            Regex rgxIP = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            if (!rgxPositiveInt.IsMatch(code))
            {
                MessageBox.Show("Invalid code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!rgxIP.IsMatch(ip))
            {
                MessageBox.Show("Invalid IP address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!rgxPositiveInt.IsMatch(port))
            {
                MessageBox.Show("Invalid port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // save values to registry
            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"Software\Slidecopy");
            regKey.SetValue("code", textBoxCode.Text);
            regKey.SetValue("ip", textBoxIP.Text);
            regKey.SetValue("port", textBoxPort.Text);
            gotData = true;

            // minimize
            this.WindowState = FormWindowState.Minimized;
        }
    }
}

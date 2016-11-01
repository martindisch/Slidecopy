using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Net;

namespace Slidecopy
{
    public partial class FormSettings : Form
    {
        private bool gotData = false;
        private string code, ip, port;

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
                code = (string) regKey.GetValue("code");
                ip = (string) regKey.GetValue("ip");
                port = (string) regKey.GetValue("port");
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
                // build request
                string url = @"http://" + ip + @":" + port + @"/photo/" + code + @".jpg";
                WebRequest request = WebRequest.Create(url);
                // show downloading notification
                notifyIcon.ShowBalloonTip(0, null, "Downloading image...", ToolTipIcon.Info);
                try
                { 
                    WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream = response.GetResponseStream();
                    // build bitmap from downloaded data
                    Bitmap image = new Bitmap(responseStream);
                    // put image in clipboard
                    Clipboard.SetImage(image);
                    // release bitmap
                    image.Dispose();
                    // ugly hack to clear old balloonTip
                    notifyIcon.Visible = false;
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(0, null, "Download complete", ToolTipIcon.Info);
                } catch (WebException)
                {
                    // ugly hack to clear old balloonTip
                    notifyIcon.Visible = false;
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(0, null, "Download failed", ToolTipIcon.Error);
                }
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
            gotData = false;
            // check input for errors
            code = textBoxCode.Text;
            ip = textBoxIP.Text;
            port = textBoxPort.Text;
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

using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Net;
using System.Resources;
using System.Reflection;

namespace Slidecopy
{
    public partial class FormSettings : Form
    {
        private bool gotCode = false;
        private string code;

        public FormSettings()
        {
            InitializeComponent();
            // initialize notifyIcon
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);

            // if they exist, load previously saved values from registry
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\Slidecopy");
            if (regKey != null)
            {
                code = (string) regKey.GetValue("code");
                textBoxCode.Text = code;
                gotCode = true;
            } else
            {
                // otherwise, show form to let user enter required data
                Show();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            // if the user closed the window, don't exit and hide instead
            e.Cancel = true;
            Hide();
        }

        void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (gotCode)
            {
                // get IP and port from creds.resx
                ResourceManager rm = new ResourceManager(typeof(creds));
                string ip = rm.GetString("server_ip");
                string port = rm.GetString("server_port");
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
                Show();
            }
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            // make sure it doesn't buggily hang around after the application is closed - thanks Windows!
            notifyIcon.Visible = false;
            // exit
            System.Windows.Forms.Application.Exit();
        }

        private void toolStripMenuItemSettings_Click(object sender, EventArgs e)
        {
            // show form
            Show();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            gotCode = false;
            // check input for errors
            code = textBoxCode.Text;
            Regex rgxPositiveInt = new Regex(@"^[1-9]\d*$");
            if (!rgxPositiveInt.IsMatch(code))
            {
                MessageBox.Show("Invalid code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // save values to registry
            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"Software\Slidecopy");
            regKey.SetValue("code", textBoxCode.Text);
            gotCode = true;

            // hide the input form
            Hide();
        }
    }
}

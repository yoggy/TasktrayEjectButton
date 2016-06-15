using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace TasktrayEjectButton
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            Eject();
        }

        private void ejectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eject();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        [DllImport("winmm", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        private void Eject()
        {
            foreach (string drive in Environment.GetLogicalDrives())
            {
                DriveInfo di = new DriveInfo(drive);
                if (di.DriveType == DriveType.CDRom)
                {
                    mciSendString("open " + drive + " type cdaudio alias orator", null, 0, IntPtr.Zero);
                    int canEject = mciSendString("capability orator can eject", null, 0, IntPtr.Zero);
                    if (canEject == 0)
                    {
                        mciSendString("set orator door open", null, 0, IntPtr.Zero);
                    }
                }
            }
        }
    }
}

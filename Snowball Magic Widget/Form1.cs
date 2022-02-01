using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace Snowball_Magic_Widget
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            FormBorderStyle = FormBorderStyle.None;
            AllowTransparency = true;
            BackColor = Color.AliceBlue;
            TransparencyKey = BackColor;
            if (File.Exists(Environment.CurrentDirectory + @"\res\link.txt"))
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + @"\res\link.txt"))
                {
                    string path = sr.ReadToEnd();
                    if (File.Exists(path))
                    {
                        pictureBox1.Image = Image.FromFile(path);
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + @"\res\b0bbcb738dcfb1743df61da3abd20185.gif");
                    }
                }
            }
            else
            {
                File.Create(Environment.CurrentDirectory + @"\res\link.txt");
                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\res\link.txt"))
                {
                    sw.Write(Environment.CurrentDirectory + @"\res\b0bbcb738dcfb1743df61da3abd20185.gif");
                    pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + @"\res\b0bbcb738dcfb1743df61da3abd20185.gif");
                }
            }
            Microsoft.Win32.RegistryKey Key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
            Key.SetValue("DoLinqToSql", Environment.CurrentDirectory + @"\Snowball Magic Widget.exe");
            Key.Close();
        }

        private Point mouseOffset;
        private bool isMouseDown = false;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int xOffset;
            int yOffset;

            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X - SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y - SystemInformation.CaptionHeight -
                    SystemInformation.FrameBorderSize.Height;
                mouseOffset = new Point(xOffset, yOffset);
                isMouseDown = true;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                Location = mousePos;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
            if (Location.X < 5)
            {
                Location = new Point(5, Location.Y);
            }
            if (Location.Y < 5)
            {
                Location = new Point(Location.X, 5);
            }
            if (Location.X + Width + 5 > Screen.PrimaryScreen.Bounds.Width)
            {
                Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width - 5, Location.Y);
            }
            if (Location.Y + Width + 5 > Screen.PrimaryScreen.Bounds.Height)
            {
                Location = new Point(Location.X, Screen.PrimaryScreen.Bounds.Height - Height - 5);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            notifyIcon1.Visible = false;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            Show();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.FileName != "")
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\res\link.txt"))
                    {
                        sw.Write(openFileDialog1.FileName.ToString());
                        pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            if (Location.X - 5 < Cursor.Position.X && Cursor.Position.X < Location.X + Width + 5 &&
                Location.Y - 5 < Cursor.Position.Y && Cursor.Position.Y < Location.Y + Height + 5)
            {
                pictureBox2.Visible = true;
                pictureBox3.Visible = true;
            }
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.AppStarting;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
        }
    }
}

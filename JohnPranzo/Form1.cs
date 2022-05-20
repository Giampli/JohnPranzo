using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JohnPranzo.Properties;
using NAudio.CoreAudioApi;

namespace JohnPranzo
{
    public partial class main : Form
    {

        private Size screenSize;
        private Size imgSize;
        private Random r;
        DateTime time = DateTime.MinValue;
        DateTime starTime = DateTime.MinValue;
        Point startMouse;
        bool started = false;

        private double waitTime;

        private List<Bitmap> imgs;
        private List<Bitmap> origImgs = new List<Bitmap>()
        {
            Resources._1,
            Resources._2,
            Resources._3,
            Resources._4,
            Resources._5,
            Resources._7,
            Resources._8,
            Resources._9,
            Resources._10,
            Resources._11,
            Resources._12,
            Resources._13,
            Resources._14,
            Resources._15,
            Resources._16,
            Resources._17,
            Resources._18
        };
        
        
        public main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowInTaskbar = false;
            BackColor = Color.DarkSlateGray;
            TransparencyKey = Color.DarkSlateGray;
            FormBorderStyle = FormBorderStyle.None;

            var a = Screen.PrimaryScreen.Bounds;
            screenSize = new Size(a.Width,a.Height);

            StartPosition = FormStartPosition.Manual;
            Location = new Point(0,0);
            Size = screenSize;
             r = new Random();



            Task t1 = new Task(() =>
            {
                var b = Utils.GetCursorPosition();
                var c = new Point(Utils.GetCursorPosition().X - b.X, Utils.GetCursorPosition().Y - b.Y);

                while (Math.Abs(c.X) < 50 && Math.Abs(c.Y) < 50)
                {
                    Thread.Sleep(100);
                    c = new Point(Utils.GetCursorPosition().X - b.X, Utils.GetCursorPosition().Y - b.Y);
                }

                Task t = new Task(() =>
                {
                    while (true)
                    {
                        KeyboardSimulator.VolumeUp();
                        Thread.Sleep(120);
                    }
                });

                t.Start();

                MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
                MMDevice defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                while (defaultDevice.AudioEndpointVolume.MasterVolumeLevel < -.1)
                {
                    Thread.Sleep(120);
                }

                Stream str = Resources.his_name; //new MemoryStream();
                System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                snd.PlayLooping();


                imgs = origImgs.ToList();

                started = true;
            });

            t1.Start();
        }




        private void timer1_Tick(object sender, EventArgs e)
        {
            if (started)
            {
                Focus();
                if (imgs.Count == 0) imgs = origImgs.ToList();

                if (starTime == DateTime.MinValue)
                {
                    starTime = DateTime.Now;
                    startMouse = Utils.GetCursorPosition();
                }

                if (time == DateTime.MinValue || (DateTime.Now - time).TotalSeconds > waitTime)
                {
                    var imgToRemove = r.Next(0, imgs.Count);
                    var selimg = imgs[imgToRemove];
                    pictureBox1.Image = selimg;
                    waitTime = (((float) r.Next(8, 15)) / 10);
                    time = DateTime.Now;
                    pictureBox1.Location =
                        new Point(r.Next(0, screenSize.Width - selimg.Width),
                            screenSize.Height - selimg.Height);

                    imgs.RemoveAt(imgToRemove);
                }

                var c = new Point(Utils.GetCursorPosition().X - startMouse.X, Utils.GetCursorPosition().Y - startMouse.Y);

                if ((DateTime.Now - starTime).TotalSeconds > 5 && (Math.Abs(c.X) > 50 || Math.Abs(c.Y) > 50))
                    Utils.RestartComputerApi();
                //Utils.RestartComputerCommand();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActivityLogger
{
    public partial class Form1 : Form
    {
        List<Point> ptList = new List<Point>();
        Timer timer;
        Point lastMousePostion;
        float scaleF;
        public Form1()
        {
            InitializeComponent();

            int x0 = 99999;
            int x1 = 0;
            int y0 = 99999;
            int y1 = 0;
            foreach (var scrn in Screen.AllScreens)
            {
                x0 = Math.Min(x0, scrn.Bounds.Left);
                x1 = Math.Max(x1, scrn.Bounds.Right);
                y0 = Math.Min(y0, scrn.Bounds.Top);
                y1 = Math.Max(y1, scrn.Bounds.Bottom);
            }

            double prop = ((double)(y1 - y0)) / (x1 - x0);
            panel1.Height = (int)(panel1.Width * prop);

            scaleF = ((float)panel1.Width) / (x1 - x0);

            lastMousePostion = new Point(0, 0);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var pos = MousePosition;
            if (!pos.Equals(lastMousePostion))
            {
                ptList.Add(pos);
                lastMousePostion = pos;

                StringBuilder sb = new StringBuilder();

                int a = ptList.Count - 10;
                if (a < 0) a = 0;
                for (int i = a; i < ptList.Count; i++)
                {
                    sb.AppendLine(i.ToString() + " : " + ptList[i].ToString());
                }

                label1.Text = sb.ToString();
                panel1.Invalidate();
            }
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.ScaleTransform(scaleF, scaleF);


            Brush br = new SolidBrush(Color.FromArgb(50, Color.Red));
            float sz = 4f / scaleF;
            for (int i = 1; i< ptList.Count; i++)
            {
                var pt = ptList[i];
                var pt1 = ptList[i - 1];
                //e.Graphics.DrawLine(Pens.Yellow, pt, pt1);
                e.Graphics.FillRectangle(br, pt.X, pt.Y, sz, sz);
            }

        }
    }
}

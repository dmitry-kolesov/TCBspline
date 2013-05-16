using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCBspline
{
    public partial class Form1 : Form
    {
        static Bitmap graphBmp;// = new Bitmap(pictureBox.Width, pictureBox.Height);
        public Form1()
        {
            InitializeComponent();
            pictureBox.Paint += pictureBox_Paint;
            pictureBox.MouseClick += pictureBox_MouseClick;
            tensionBar.ValueChanged += bar_ValueChanged;
            continuityBar.ValueChanged += bar_ValueChanged;
            biasBar.ValueChanged += bar_ValueChanged;

            graphBmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = graphBmp;
            Graphics g = Graphics.FromImage(graphBmp);
            SolidBrush b = new SolidBrush(Color.White);
            g.FillRectangle(b, 0, 0, graphBmp.Width, graphBmp.Height);
        }

        void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            DrawPoints(points, e.Graphics);
        }

        List<PointF> points = new List<PointF>();
        void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(new PointF(e.Location.X, e.Location.Y));
            DrawPoint(new Point(e.Location.X, e.Location.Y));
            //Draw();
        }

        void bar_ValueChanged(object sender, EventArgs e)
        {
            //Draw();
        }



        float devider = 10f;
        private void Draw()
        {
            try
            {
                DataGraphic dg = new DataGraphic(points.ToArray(), pictureBox.Size.Width, pictureBox.Size.Height, (float)tensionBar.Value / devider, (float)continuityBar.Value / devider, (float)biasBar.Value / devider);
                var result = dg.GetSpline();
                Draw(result);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private void DrawPoints(List<PointF> points, Graphics g)
        {
            //Graphics g = Graphics.FromImage(graphBmp);
            foreach (var point in points)
                g.FillEllipse( new SolidBrush(Color.Green), new Rectangle((int)point.X, (int)point.Y, 5, 5));
        } 

        private void DrawPoint(Point point)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            g.FillEllipse(Brushes.Green, new Rectangle(point.X, point.Y, 5, 5));
        }

        public void Draw(PointF[] spline)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            PointF p1 = spline[0];
            //Point p2;
            for (int i = 0; i < spline.Length - 1; i++)
            {                
                var p2 = spline[i + 1];
                g.DrawLine(new Pen(Color.Red, 2), p1.X, p1.Y, p2.X, p2.Y);

                p1 = p2;
            }

            //for (int i = from; (i < data.X.Count) && (i < to); i++)
            //{
            //    p2 = new Point(ScaleX(data.X[i], inXMin, inXMax, outXMin, outXMax), ScaleY(data.Y[i], inYMin, inYMax, outYMin, outYMax));

            //    if (i > from)
            //        g.DrawLine(new Pen(Color.Red, 2), p1.X, p1.Y, p2.X, p2.Y);

            //    Font drawFont = new Font("Arial", 7);
            //    SolidBrush drawBrush = new SolidBrush(Color.Black);
            //    g.DrawString(String.Format("P{0} ({1}, {2})", i.ToString(), data.X[i], data.Y[i]), drawFont, drawBrush, p2);
            //    p1 = p2;
            //}
        }
    }
}

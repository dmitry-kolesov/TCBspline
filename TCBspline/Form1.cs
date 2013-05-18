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
        static Bitmap graphBmp;
        List<MyPointF> points = new List<MyPointF>();
        bool wasChanged = false;
        MyPointF selected;

        public Form1()
        {
            InitializeComponent();
            pictureBox.Paint += pictureBox_Paint;
            pictureBox.MouseClick += pictureBox_MouseClick;
            tensionBar.ValueChanged += bar_ValueChanged;
            continuityBar.ValueChanged += bar_ValueChanged;
            biasBar.ValueChanged += bar_ValueChanged;

            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseMove += pictureBox_MouseMove;
            pictureBox.MouseUp += pictureBox_MouseUp;

            InitPictureBox();
        }

        void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (selected != null)
            {
                selected.UnSelect();
                selected = null;
            }
            pictureBox.Invalidate();
        }

        void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (selected != null)
            {
                InitPictureBox();
                selected.UpdatePoint(new PointF(e.X, e.Y));
                Draw();
            }
        }

        void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var founded = GetPointInSurrounding(e.Location);
                if ((founded != null) && points.Contains(founded))
                {
                    founded.SetSelected(points);
                    selected = founded;
                }
                else
                {
                    InitPictureBox();
                    points.Add(new MyPointF(e.Location.X, e.Location.Y));
                    wasChanged = true;
                }

            }
        }

        private void InitPictureBox()
        {
            graphBmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = graphBmp;
            Graphics g = Graphics.FromImage(graphBmp);
            SolidBrush b = new SolidBrush(Color.White);
            g.FillRectangle(b, 0, 0, graphBmp.Width, graphBmp.Height);
        }

        void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            DrawPoints(points, e.Graphics);
            if (wasChanged)
                Draw();
            wasChanged = false;
        }

        void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) // add
            {
                var founded = GetPointInSurrounding(e.Location);
                if (founded != null)
                {
                    InitPictureBox();
                    points.Remove(founded);
                    pictureBox.Invalidate();
                    Draw();
                }
            }
        }

        private MyPointF GetPointInSurrounding(Point mousePosition)
        {
            if (points != null && points.Count > 0)
            {
                var location = new MyPointF(mousePosition);
                var founded = points.Where(p => location.DistanceFrom(p) < 5f).FirstOrDefault();
                if (founded != null && points.Contains(founded))
                {
                    return founded;
                }
            }
            return null;
        }

        void bar_ValueChanged(object sender, EventArgs e)
        {
            InitPictureBox();
            Draw();
        }

        float devider = 10f;
        private void Draw()
        {
            try
            {

                if (points != null && points.Count > 2)
                {
                    DataGraphic dg = new DataGraphic(points.ToArray(), pictureBox.Size.Width, pictureBox.Size.Height, (float)tensionBar.Value / devider, (float)continuityBar.Value / devider, (float)biasBar.Value / devider);
                    var result = dg.GetSpline();
                    Draw(result);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private void DrawPoints(List<MyPointF> points)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            DrawPoints(points, g);
            pictureBox.Image = graphBmp;
            g.Dispose();
        }
        private void DrawPoints(List<MyPointF> points, Graphics g)
        {
            foreach (var point in points)
                DrawPoint(point, g);
        }

        private void DrawPoint(MyPointF point)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            DrawPoint(point, g);
            pictureBox.Image = graphBmp;
            g.Dispose();
        }

        private void DrawPoint(MyPointF point, Graphics g)
        {
            g.FillEllipse(new SolidBrush(point.Color), new Rectangle((int)point.X, (int)point.Y, 5, 5));
        }

        internal void Draw(MyPointF[] spline)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var p1 = spline[0];
            for (int i = 0; i < spline.Length - 1; i++)
            {
                var p2 = spline[i + 1];
                g.DrawLine(new Pen(Color.Red, 2), p1.X, p1.Y, p2.X, p2.Y);

                p1 = p2;
            }
            pictureBox.Image = graphBmp;
            g.Dispose();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            InitPictureBox();
            points.Clear();
        }
    }
}

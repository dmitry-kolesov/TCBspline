using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCBspline
{
    internal class Drawer
    {
        //private static Drawer instance;
        //internal static Drawer Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance = new Drawer();
        //        return instance;
        //    }
        //}

        PictureBox pictureBox;
        Bitmap graphBmp;
        
        internal Drawer(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
        }

        internal void InitPictureBox()
        {
            graphBmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = graphBmp;
            Graphics g = Graphics.FromImage(graphBmp);
            SolidBrush b = new SolidBrush(Color.White);
            g.FillRectangle(b, 0, 0, graphBmp.Width, graphBmp.Height);
        }

        float devider = 10f;
        internal void Draw(List<MyPointF> points, float tensionBarValue, float continuityBarValue, float biasBarValue)
        {
            try
            {
                if (points != null && points.Count > 2)
                {
                    DataGraphic dg = new DataGraphic(points.ToArray(), pictureBox.Size.Width, pictureBox.Size.Height, tensionBarValue / devider, continuityBarValue / devider, biasBarValue / devider);
                    var result = dg.GetSpline();
                    Draw(result);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        internal void DrawPoints(List<MyPointF> points)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            DrawPoints(points, g);
            pictureBox.Image = graphBmp;
            g.Dispose();
        }

        internal void DrawPoints(List<MyPointF> points, Graphics g)//
        {
            foreach (var point in points)
                DrawPoint(point, g);
        }

        internal void DrawPoint(MyPointF point)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            DrawPoint(point, g);
            pictureBox.Image = graphBmp;
            g.Dispose();
        }

        internal void DrawPoint(MyPointF point, Graphics g)
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

    }
}

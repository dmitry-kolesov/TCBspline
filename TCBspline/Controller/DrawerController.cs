using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using TCBspline.Model;

namespace TCBspline.Controller
{
    internal class DrawerController
    {
        public event Action<Bitmap> OnUpdateImage;
        Bitmap graphBmp;
        int pictureBoxWidth;
        int pictureBoxHeight;

        internal DrawerController(int _pictureBoxWidth, int _pictureBoxHeight)
        {
            InitPictureBox(_pictureBoxWidth, _pictureBoxHeight);
        }

        internal void InitPictureBox(int _pictureBoxWidth, int _pictureBoxHeight)
        {
            pictureBoxWidth = _pictureBoxWidth;
            pictureBoxHeight = _pictureBoxHeight;

            graphBmp = new Bitmap(pictureBoxWidth, pictureBoxHeight);
            Graphics g = Graphics.FromImage(graphBmp);
            SolidBrush b = new SolidBrush(Color.White);
            g.FillRectangle(b, 0, 0, graphBmp.Width, graphBmp.Height);
            if (OnUpdateImage != null)
                OnUpdateImage(graphBmp);
        }

        float divider = 10f;
        internal void Draw(List<MyPointF> points, float tensionBarValue, float continuityBarValue, float biasBarValue)
        {
            try
            {
                if (points != null && points.Count > 2)
                {
                    SplineCalculator dg = new SplineCalculator(points.ToArray(), pictureBoxWidth, pictureBoxHeight, tensionBarValue / divider, continuityBarValue / divider, biasBarValue / divider);
                    var result = dg.GetSpline();
                    Draw(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void DrawPoints(List<MyPointF> points)
        {
            Graphics g = Graphics.FromImage(graphBmp);
            DrawPoints(points, g);
            UpdateImage(graphBmp);
            g.Dispose();
        }

        private void UpdateImage(Bitmap updatedImage)
        {
            if (OnUpdateImage != null)
                OnUpdateImage(updatedImage);
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
            UpdateImage(graphBmp);
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
            UpdateImage(graphBmp);
            g.Dispose();
        }

    }
}

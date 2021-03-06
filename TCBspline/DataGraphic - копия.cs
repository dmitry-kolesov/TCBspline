using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TCBspline
{
    class DataGraphic
    {
        //private PictureBox graphPB;
        private PointF[] initPoints;
        private PointF[] points;
        private float width;
        private float height;
        private float tension;
        private float continuty;
        private float bias;

        private int outXMin = -10;
        private int outXMax = 10;
        private int outYMin = -10;
        private int outYMax = 10;

        //static Bitmap graphBmp;// = new Bitmap();

        public DataGraphic(PointF[] points, int width, int height, float tension, float continuty, float bias)//, PictureBox pb)
        {
            if (points != null && points.Length > 2)
            {
                //graphPB = pb;
                //graphBmp = new Bitmap(graphPB.Width, graphPB.Height);
                //graphPB.Image = graphBmp;

                // real points convert to (-10, 10)^2 box

                this.width = width;
                this.height = height;

                initPoints = points;
                this.points = points;// Scale(points, Scale);

                this.tension = tension;
                this.continuty = continuty;
                this.bias = bias;
            }
            else
            {
                if (points.Length > 2)
                {
                    MessageBox.Show("Please select points");
                    throw new Exception("Points was not selected");
                }
            }
        }

        private PointF[] Scale(PointF[] points, ScaleDelegate scale)
        {
            PointF[] result = new PointF[points.Length];
            int i = 0;
            foreach (var point in points)
            {
                result[i] = scale(point);
                i++;
            }
            return result;
        }

        public delegate PointF ScaleDelegate(PointF point);

        private PointF BackScale(PointF point)
        {
            var result = new PointF(ScaleX(point.X, outXMin, outXMax, 0, width), ScaleX(point.Y, outYMin, outYMax, 0, height));
            return result;
        }

        private PointF Scale(PointF point)
        {
            var result = new PointF(ScaleX(point.X, 0, width, outXMin, outXMax), ScaleX(point.Y, 0, height, outYMin, outYMax));
            return result;
        }

        private static float ScaleX(float inX, float inMin, float inMax, float outMin, float outMax)
        {
            var newX = ((float)(inX - inMin) / (float)(inMax - inMin) * (float)(outMax - outMin));
            return (float)newX;
        }

        public PointF[] GetSpline(int discrete = 10)
        {
            var p = points;
            var result = new PointF[p.Length * discrete];

            var delta = 1f / discrete;
            var start = 0f;

            PointF r1, r2;
            PointF ra1 = new PointF();
            MyPointF rb = new MyPointF(ra1);
            for (int i = 1; i < p.Length - 1; i++)
            {
                //var next1 = (i + 1) % p.Length;
                var next = (i + 1) % p.Length;
                //var prev = (i - 1) % p.Length;
                r1 = CalcRAB(p[i - 1], p[i], p[i + 1], true);
                if (i == 1) ra1 = r1;
                r2 = CalcRAB(p[i - 1], p[i], p[i + 1], false);
                if (i == p.Length - 1) rb = new MyPointF(r2);
                start = 0f;//t = (p.Length - i); / (p2.T - p1.T)
                for (int j = 0; j < discrete; j++)
                {
                    result[i * discrete + j] = CalcInPoint(p[i], p[i + 1], r1, r2, start);
                    start += delta;
                }
            }

            r1 = ((new MyPointF(p[1])) - (new MyPointF(p[0]))).Point;
            r2 = ((1.5f * ((new MyPointF(p[1])) - (new MyPointF(p[0]))) - 0.5f * new MyPointF(ra1)) * (1 - tension)).Point;

            for (int j = 0; j < discrete; j++)
            {
                result[j] = CalcInPoint(p[0], p[1], r1, r2, start);
                start += delta;
            }

            var lastInd = p.Length - 1;
            var cur = new MyPointF(p[lastInd]);
            var prev = new MyPointF(p[lastInd - 1]);
            r1 = ((1.5f * (cur - prev) - 0.5f * rb) * (1 - tension)).Point;
            r2 = ((new MyPointF(p[lastInd])) - (new MyPointF(p[lastInd - 1]))).Point;

            for (int j = 0; j < discrete; j++)
            {
                result[lastInd * discrete + j] = CalcInPoint(prev.Point, cur.Point, r1, r2, start);
                start += delta;
            }

            //start = 0f;
            //for (int i = p.Length - 3; i < p.Length - 1; i++)
            //{
            //    if (i == p.Length - 3)
            //        r1 = CalcR1(p[i], p[i + 1], p[i + 2]);
            //    else
            //        r1 = (((new MyPointF(p[i])) - (new MyPointF(p[i - 1]))) * (1 - tension)).Point;
            //    if (i < p.Length - 1) 
            //        r2 = (((new MyPointF(p[i + 1])) - (new MyPointF(p[i]))) * (1 - tension)).Point;
            //    else
            //        r2= r1;
            //    for (int j = 0; j < discrete; j++)
            //    {
            //        result[i * discrete + j] = CalcInPoint(p[i], p[i + 1], r1, r2, start);
            //        start += delta;
            //    }
            //}
            var result2 = result;// Scale(result, BackScale);
            return result2;
        }

        private PointF CalcRAB(PointF p11, PointF p22, PointF p33, bool isRa)
        {
            var g1 = (new MyPointF(p22) - new MyPointF(p11)) * (1f + bias);
            var g2 = (new MyPointF(p33) - new MyPointF(p22)) * (1f - bias);
            var g3 = g2 - g1;
            MyPointF ra;
            if (isRa) ra = (1f - tension) * (g1 + 0.5f * g3 * (1f + continuty));
            else ra = (1f - tension) * (g1 + 0.5f * g3 * (1f - continuty));
            return ra.Point;
        }

        private PointF CalcR1(PointF p11, PointF p22, PointF p33)
        {
            var p1 = new MyPointF(p11);
            var p2 = new MyPointF(p22);
            var p3 = new MyPointF(p33);
            var r1 = (1 - tension) * (1 + bias) * (1 + continuty) / 2f * (p2 - p1) + (1 - tension) * (1 - bias) * (1 + continuty) / 2f * (p3 - p2);
            return r1.Point;
        }

        private PointF CalcInPoint(PointF p11, PointF p22, PointF r11, PointF r22, float t)
        {
            var tSqure = Math.Pow(t, 2);
            var tCube = Math.Pow(t, 3);
            var p1 = new MyPointF(p11);
            var p2 = new MyPointF(p22);
            var r1 = new MyPointF(r11);
            var r2 = new MyPointF(r22);
            var ft = p1 * (2 * tCube - 3 * tSqure + 1) + r1 * (tCube - 2 * tSqure + t) + p2 * (-2 * tCube + 3 * tSqure) + r2 * (tCube - tSqure);
            //var fY = p1.Y * (2*tCube - 3*tSqure + 1) + r1.Y * (tCube - 2*tSqure + t) + p2.Y * (-2*tCube + 3*tSqure) + r2.Y * (tCube - tSqure);
            return ft.Point;
        }

    }
}

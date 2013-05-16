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

            //var r1 = (((new MyPointF(p[1])) - (new MyPointF(p[0]))) * (1 - tension)).Point;
            //var r2 = r1;

            var delta = 1f / discrete;
            var start = 0f;
            //for (int j = 0; j < discrete; j++)
            //{
            //    result[j] = CalcInPoint(p[0], p[1], r1, r2, start);
            //    start += delta;
            //}
            PointF r1, r2;
            for (int i = 0; i < p.Length; i++)
            {
                var next1 = (i + 1) % p.Length;
                var next2 = (i + 2) % p.Length;
                var next3 = (i + 3) % p.Length;
                r1 = CalcR1(p[i], p[next1], p[next2]);
                r2 = CalcR1(p[next1], p[next2], p[next3]);
                start = 0f;//t = (p.Length - i); / (p2.T - p1.T)
                for (int j = 0; j < discrete; j++)
                {
                    result[i * discrete + j] = CalcInPoint(p[i], p[next1], r1, r2, start);
                    start += delta;
                }
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

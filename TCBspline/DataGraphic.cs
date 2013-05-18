using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TCBspline
{
    class DataGraphic
    {
        private MyPointF[] initPoints;
        private MyPointF[] points;
        private float width;
        private float height;
        private float tension;
        private float continuty;
        private float bias;

        private int outXMin = -10;
        private int outXMax = 10;
        private int outYMin = -10;
        private int outYMax = 10;


        public DataGraphic(MyPointF[] points, int width, int height, float tension, float continuty, float bias)//, PictureBox pb)
        {
            if (points != null && points.Length > 2)
            {

                this.width = width;
                this.height = height;

                initPoints = points;
                this.points = points;

                this.tension = tension;
                this.continuty = continuty;
                this.bias = bias;
            }
            else
            {
                if (points != null && points.Length < 2)
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

        internal MyPointF[] GetSpline(int discrete = 10)
        {
            var p = points;
            var result = new MyPointF[(p.Length - 1) * discrete + 1];

            var delta = 1f / discrete;
            var start = 0f;

            MyPointF r1 = (((p[1])) - ((p[0])));
            MyPointF ra1 = r1;// new MyPointF();
            MyPointF r2 = ((1.5f * (((p[1])) - ((p[0]))) - 0.5f * ra1) * (1 - tension));
            MyPointF rbLast = (ra1);
            for (int i = 1; i < p.Length - 1; i++)
            {
                //var next1 = (i + 1) % p.Length;
                var next = (i + 1);// % p.Length;
                //var prev = (i - 1) % p.Length;
                if (next < p.Length)
                {
                    r1 = CalcRAB(p[i - 1], p[i], p[next], true);
                    if (i == 1) ra1 = r1;
                    r2 = CalcRAB(p[i - 1], p[i], p[next], false);
                }
                if (i == p.Length - 1)
                {
                    var lastInd = p.Length - 1;
                    var cur = (p[lastInd]);
                    var prev = (p[lastInd - 1]);
                    r1 = ((1.5f * (cur - prev) - 0.5f * rbLast) * (1 - tension));
                    r2 = (cur - prev);//((new MyPointF(p[lastInd])) - (new MyPointF(p[lastInd - 1]))).Point;
                }
                if (i == p.Length - 2) rbLast = r2;
                start = 0f;//t = (p.Length - i); / (p2.T - p1.T)
                for (int j = 0; j < discrete; j++)
                {
                    result[i * discrete + j] = CalcInPoint(p[i], p[next], r1, r2, start);
                    start += delta;
                }
            }
            result[result.Length - 1] = p[p.Length - 1];

            r1 = (((p[1])) - ((p[0])));
            r2 = ((1.5f * (((p[1])) - ((p[0]))) - 0.5f * ra1) * (1 - tension));
            start = 0f;
            for (int j = 0; j < discrete; j++)
            {
                result[j] = CalcInPoint(p[0], p[1], r1, r2, start);
                start += delta;
            }
            var result2 = result;// Scale(result, BackScale);
            return result2;
        }
        
        private MyPointF CalcRAB(MyPointF p11, MyPointF p22, MyPointF p33, bool isRa)
        {
            var g1 = ((p22) - (p11)) * (1f + bias);
            var g2 = ((p33) - (p22)) * (1f - bias);
            var g3 = g2 - g1;
            MyPointF ra;
            if (isRa) ra = (1f - tension) * (g1 + 0.5f * g3 * (1f + continuty));
            else ra = (1f - tension) * (g1 + 0.5f * g3 * (1f - continuty));
            return ra;
        }

        private MyPointF CalcR1(PointF p11, PointF p22, PointF p33)
        {
            var p1 = new MyPointF(p11);
            var p2 = new MyPointF(p22);
            var p3 = new MyPointF(p33);
            var r1 = (1 - tension) * (1 + bias) * (1 + continuty) / 2f * (p2 - p1) + (1 - tension) * (1 - bias) * (1 + continuty) / 2f * (p3 - p2);
            return r1;
        }

        private MyPointF CalcInPoint(MyPointF p11, MyPointF p22, MyPointF r11, MyPointF r22, float t)
        {
            var tSqure = Math.Pow(t, 2);
            var tCube = Math.Pow(t, 3);
            var p1 = (p11);
            var p2 = (p22);
            var r1 = (r11);
            var r2 = (r22);
            var ft = p1 * (2 * tCube - 3 * tSqure + 1) + r1 * (tCube - 2 * tSqure + t) + p2 * (-2 * tCube + 3 * tSqure) + r2 * (tCube - tSqure);
            //var fY = p1.Y * (2*tCube - 3*tSqure + 1) + r1.Y * (tCube - 2*tSqure + t) + p2.Y * (-2*tCube + 3*tSqure) + r2.Y * (tCube - tSqure);
            return ft;
        }

    }
}

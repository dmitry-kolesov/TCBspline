using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCBspline
{
    class MyPointF
    {

        public MyPointF(PointF newPoint)
        {
            point = newPoint;
        }

        private PointF point;
        internal PointF Point { get { return point; } }
        internal float X { get { return point.X; } set { point.X = value; } }
        internal float Y { get { return point.Y; } set { point.Y = value; } }

        public static MyPointF operator *(float coef, MyPointF p1)
        {
            return p1 * (double)coef;
        }
        public static MyPointF operator *(MyPointF p1, float coef)
        {
            return p1 * coef;
        }
        public static MyPointF operator *(MyPointF p1, double coef)
        {
            return new MyPointF(new PointF(p1.X * (float)coef, p1.Y * (float)coef));
        }
        public static MyPointF operator +(MyPointF p1, MyPointF p2)
        {
            return new MyPointF(new PointF(p1.X + p2.X, p1.Y + p2.Y));
        }
        public static MyPointF operator -(MyPointF p1, MyPointF p2)
        {
            return new MyPointF(new PointF(p1.X - p2.X, p1.Y - p2.Y));
        }
    }
}

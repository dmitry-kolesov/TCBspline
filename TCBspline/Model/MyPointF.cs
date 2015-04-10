using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace TCBspline.Model
{
    [Serializable]
    public class MyPointF
    {
        public MyPointF()
        { }

        public MyPointF(float x, float y)
        {
            point = new PointF(x, y);
        }

        public MyPointF(Point newPoint)
        {
            point = new PointF(newPoint.X, newPoint.Y);
        }

        public MyPointF(PointF newPoint)
        {
            point = newPoint;
        }

        private PointF point;

        [XmlAttribute]
        public int Index;

        public PointF Point { get { return point; } set { point = value; } }

        internal float X { get { return point.X; } set { point.X = value; } }
        internal float Y { get { return point.Y; } set { point.Y = value; } }

        private Color color = System.Drawing.Color.Green;

        [XmlIgnore]
        public Color Color { get { return color; } set { color = value; } }

        [XmlElement("Color")]
        public int ColorAsArgb
        {
            get { return Color.ToArgb(); }
            set { Color = Color.FromArgb(value); }
        }

        private bool isSelected = false;

        public void SetSelected(List<MyPointF> points)
        {
            Index = points.IndexOf(this);
            Color = Color.Red;
            isSelected = true;
        }

        public void UnSelect()
        {
            Color = Color.Green;
            isSelected = false;
        }

        public void UpdatePoint(PointF newPoint)
        {
            point = newPoint;
        }

        public static MyPointF operator *(float coef, MyPointF p1)
        {
            return p1 * (double)coef;
        }

        public static MyPointF operator *(MyPointF p1, float coef)
        {
            return p1 * (double)coef;
        }

        public static MyPointF operator *(MyPointF p1, double coef)
        {
            return new MyPointF(p1.X * (float)coef, p1.Y * (float)coef);
        }

        public static MyPointF operator +(MyPointF p1, MyPointF p2)
        {
            return new MyPointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static MyPointF operator -(MyPointF p1, MyPointF p2)
        {
            return new MyPointF(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static double Lenght(MyPointF p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        public static double Distance(MyPointF p1, MyPointF p2)
        {
            return Lenght(p1 - p2);
        }

        public double DistanceFrom(MyPointF p2)
        {
            return Lenght(this - p2);
        }

        public double DistanceFrom(PointF p2)
        {
            return Lenght(this - (new MyPointF(p2)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCBspline
{
    class PointsLogic
    {
        List<MyPointF> points = new List<MyPointF>();
        internal List<MyPointF> Points { get { return points; } }

        bool wasChanged = false;
        MyPointF selected;

        public delegate void OnDrawHandler();
        public event OnDrawHandler OnDraw;

        public void UnSelectPoint()
        {
            if (selected != null)
            {
                selected.UnSelect();
                selected = null;
            }
        }

        public void MoveSelectedPoint(Drawer drawer, MouseEventArgs e)
        {
            if (selected != null)
            {
                selected.UpdatePoint(new PointF(e.X, e.Y));
                drawer.InitPictureBox();
                if (OnDraw != null) OnDraw();
            }
        }

        public void SelectOrAddNewPoint(Drawer drawer, MouseEventArgs e)
        {
            var founded = GetPointInSurrounding(e.Location);
            if ((founded != null) && points.Contains(founded))
            {
                founded.SetSelected(points);
                selected = founded;
            }
            else
            {
                drawer.InitPictureBox();
                points.Add(new MyPointF(e.Location.X, e.Location.Y));
                wasChanged = true;
            }
        }

        public bool PointsChanged(Drawer drawer, PaintEventArgs e)
        {
            drawer.DrawPoints(points, e.Graphics);
            if (!wasChanged)
                return true;
            if (OnDraw != null) OnDraw();
            wasChanged = false;
            return true;
        }

        public void DeletePoint(Drawer drawer, MouseEventArgs e)
        {
            var founded = GetPointInSurrounding(e.Location);
            if (founded != null)
            {
                drawer.InitPictureBox();
                points.Remove(founded);
                //pictureBox.Invalidate();
                if (OnDraw != null) OnDraw();
            }
        }

        public void ClearPoints(Drawer drawer)
        {
            drawer.InitPictureBox();
            points.Clear();
            if (OnDraw != null) OnDraw();
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
    }
}

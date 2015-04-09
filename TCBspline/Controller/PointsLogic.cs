using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using TCBspline.Model;

namespace TCBspline
{
    class PointsLogicAndDrawerController
    {
        DrawerController drawerController;

        List<MyPointF> points = new List<MyPointF>();
        internal List<MyPointF> Points { get { return points; } }

        bool wasChanged = false;
        MyPointF selected;

        public delegate void OnDrawHandler();
        public event OnDrawHandler OnDraw;

        internal PointsLogicAndDrawerController(PictureBox pbx)
        {
            drawerController = new DrawerController(pbx);
            drawerController.InitPictureBox();
        }

        public void InitPictureBox(int pbxWidth, int pbxHeight)
        {
            drawerController.InitPictureBox();
        }

        public void Draw(float tensionBarValue, float continuityBarValue, float biasBarValue)
        {
            drawerController.Draw(this.Points, tensionBarValue, continuityBarValue, biasBarValue);
        }

        public void UnSelectPoint()
        {
            if (selected != null)
            {
                selected.UnSelect();
                selected = null;
            }
        }

        public void MoveSelectedPoint(MouseEventArgs e)
        {
            if (selected != null)
            {
                selected.UpdatePoint(new PointF(e.X, e.Y));
                drawerController.InitPictureBox();
                if (OnDraw != null) OnDraw();
            }
        }

        public void SelectOrAddNewPoint(MouseEventArgs e)
        {
            var founded = GetPointInSurrounding(e.Location);
            if ((founded != null) && points.Contains(founded))
            {
                founded.SetSelected(points);
                selected = founded;
            }
            else
            {
                drawerController.InitPictureBox();
                points.Add(new MyPointF(e.Location.X, e.Location.Y));
                wasChanged = true;
            }
        }

        public bool PointsChanged(PaintEventArgs e)
        {
            drawerController.DrawPoints(points, e.Graphics);
            if (!wasChanged)
                return true;
            if (OnDraw != null) OnDraw();
            wasChanged = false;
            return true;
        }

        public void DeletePoint(MouseEventArgs e)
        {
            var founded = GetPointInSurrounding(e.Location);
            if (founded != null)
            {
                drawerController.InitPictureBox();
                points.Remove(founded);
                //pictureBox.Invalidate();
                if (OnDraw != null) OnDraw();
            }
        }

        public void ClearPoints()
        {
            drawerController.InitPictureBox();
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

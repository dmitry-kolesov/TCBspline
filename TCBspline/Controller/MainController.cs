using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using TCBspline.Model;

namespace TCBspline.Controller
{
    // points and drawer controller
    class MainController
    {
        private List<MyPointF> points = new List<MyPointF>();
        internal List<MyPointF> Points { get { return points; } }

        private List<PointAction> actionsList;

        private DrawerController drawerController;

        private bool wasChanged = false;
        private MyPointF selected;
        private int pbxWidth;
        private int pbxHeight;

        public delegate void OnDrawHandler();
        public event OnDrawHandler OnDraw;

        public event Action<Bitmap> OnUpdateImage;

        internal MainController(PictureBox pbx)
        {
            actionsList = new List<PointAction>();

            pbxWidth = pbx.Width;
            pbxHeight = pbx.Height;
            drawerController = new DrawerController(pbx.Width, pbx.Height);
            drawerController.OnUpdateImage += drawerController_OnUpdateImage;
        }
        
        public void InitPictureBox(int _pbxWidth, int _pbxHeight)
        {
            pbxWidth = _pbxWidth;
            pbxHeight = _pbxHeight;
            drawerController.InitPictureBox(pbxWidth, pbxHeight);
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

                actionsList[actionsList.Count - 1].SetNewPoint(selected);
            }
        }

        public void MoveSelectedPoint(MouseEventArgs e)
        {
            if (selected != null)
            {
                selected.UpdatePoint(new PointF(e.X, e.Y));
                drawerController.InitPictureBox(pbxWidth, pbxHeight);
                if (OnDraw != null) OnDraw();
            }
        }

        public void SelectOrAddNewPoint(MouseEventArgs e)
        {
            var foundedPoint = GetPointInSurrounding(e.Location);
            if ((foundedPoint != null) && points.Contains(foundedPoint))
            {
                foundedPoint.SetSelected(points);
                selected = foundedPoint;

                actionsList.Add(new PointAction(PointActionType.MovePoint, foundedPoint, null, points.IndexOf(foundedPoint)));
            }
            else
            {
                drawerController.InitPictureBox(pbxWidth, pbxHeight);
                var newPoint = new MyPointF(e.Location.X, e.Location.Y);
                points.Add(newPoint);
                wasChanged = true;

                actionsList.Add(new PointAction(PointActionType.AddPoint, newPoint, null, points.Count - 1));
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
                drawerController.InitPictureBox(pbxWidth, pbxHeight);

                actionsList.Add(new PointAction(PointActionType.DeletePoint, founded, null, points.IndexOf(founded)));

                points.Remove(founded);
                //pictureBox.Invalidate();
                if (OnDraw != null) OnDraw();

            }
        }

        public void ClearPoints()
        {
            drawerController.InitPictureBox(pbxWidth, pbxHeight);
            points.Clear();
            if (OnDraw != null) OnDraw();
        }

        internal void Undo()
        {
 
        }

        internal void Redo()
        { }
        
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

        private void drawerController_OnUpdateImage(Bitmap obj)
        {
            if (OnUpdateImage != null)
                OnUpdateImage(obj);
        }
    }
}

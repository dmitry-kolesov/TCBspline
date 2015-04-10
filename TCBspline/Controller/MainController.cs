using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

using TCBspline.Common;
using TCBspline.Model;

namespace TCBspline.Controller
{
    // points and drawer controller
    class MainController
    {
        private List<MyPointF> points = new List<MyPointF>();
        internal List<MyPointF> Points { get { return points; } }

        /// it is better to move to undo redo controller
        private List<PointAction> toUndoActionsList;
        private List<PointAction> toRedoActionsList;

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
            toUndoActionsList = new List<PointAction>();
            toRedoActionsList = new List<PointAction>();

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
                toUndoActionsList[toUndoActionsList.Count - 1].SetNewPoint(selected);

                selected.UnSelect();
                selected = null;
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

                toUndoActionsList.Add(new PointAction(PointActionType.MovePoint, foundedPoint, null, points.IndexOf(foundedPoint)));
                toRedoActionsList.Clear();
            }
            else
            {
                drawerController.InitPictureBox(pbxWidth, pbxHeight);
                var newPoint = new MyPointF(e.Location.X, e.Location.Y);
                points.Add(newPoint);
                wasChanged = true;

                toUndoActionsList.Add(new PointAction(PointActionType.AddPoint, newPoint, null, points.Count - 1));
                toRedoActionsList.Clear();
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

                toUndoActionsList.Add(new PointAction(PointActionType.DeletePoint, founded, null, points.IndexOf(founded)));
                toRedoActionsList.Clear();

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

        private void FullRedraw()
        {
            drawerController.InitPictureBox(pbxWidth, pbxHeight);
            wasChanged = true;
            if (OnDraw != null) OnDraw();

        }

        private void drawerController_OnUpdateImage(Bitmap obj)
        {
            if (OnUpdateImage != null)
                OnUpdateImage(obj);
        }

        #region undo / redo logic
        internal void Undo()
        {
            MoveLastUndoRedo(toUndoActionsList, toRedoActionsList, true);
        }

        internal void Redo()
        {
            MoveLastUndoRedo(toRedoActionsList, toUndoActionsList, false);
        }

        private void MoveLastUndoRedo(List<PointAction> src, List<PointAction> dest, bool isUndo)
        {
            if (src.Count > 0)
            {
                var elementToMove = src[src.Count - 1];
                dest.Add(elementToMove);
                src.Remove(elementToMove);
                ProcessAction(elementToMove, isUndo);
            }
        }

        private void ProcessAction(PointAction pointAction, bool isUndo)
        {
            drawerController.InitPictureBox(pbxWidth, pbxHeight);
            if (isUndo)
            {
                switch (pointAction.ActionType)
                {
                    case (PointActionType.AddPoint):
                        {
                            var wasFounded = points.Remove(pointAction.Old);
                        }
                        break;
                    case (PointActionType.DeletePoint):
                        {
                            if (pointAction.PointInd < points.Count)
                                points.Insert(pointAction.PointInd, pointAction.Old);
                            else
                                points.Add(pointAction.Old);
                        }
                        break;
                    case (PointActionType.MovePoint):
                        {
                            var founded = points.FirstOrDefault(o => o.Equals(pointAction.New));
                            if (founded != null)
                                founded.UpdatePoint(pointAction.OldCoord);
                        }
                        break;
                }
            }
            else
            {
                switch (pointAction.ActionType)
                {
                    case (PointActionType.AddPoint):
                        {
                            points.Add(pointAction.Old);
                        }
                        break;
                    case (PointActionType.DeletePoint):
                        {
                            points.Remove(pointAction.Old);
                        }
                        break;
                    case (PointActionType.MovePoint):
                        {
                            var founded = points.FirstOrDefault(o => o.Equals(pointAction.Old));
                            if (founded != null)
                                founded.UpdatePoint(pointAction.NewCoord);
                        }
                        break;
                }
            }
            wasChanged = true;
            if (OnDraw != null) OnDraw();
        }
        #endregion

        #region Xml serialization / deserialization

        internal void Serialize(string path)
        {
            var xmlHelper = new XmlHelper();
            xmlHelper.Serialize(path, points);
        }

        internal void Deserialize(string path)
        {
            var xmlHelper = new XmlHelper();
            points = xmlHelper.Deserialize(path);

            FullRedraw();
        }

        #endregion
    }
}

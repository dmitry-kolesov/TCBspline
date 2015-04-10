using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TCBspline.Model
{
    enum PointActionType
    {
        AddPoint,
        DeletePoint,
        MovePoint
    }

    class PointAction
    {
        public PointF OldCoord { get; private set; }
        public PointF NewCoord { get; private set; }
        public PointActionType ActionType { get; private set; }
        public MyPointF Old { get; private set; }
        public MyPointF New { get; private set; }
        public int PointInd { get; private set; }

        internal PointAction(PointActionType actionType,
                             MyPointF oldPoint,
                             MyPointF newPoint,
                             int pointInd)
        {
            // as a structure it would copy by value
            if (oldPoint != null)
                OldCoord = oldPoint.Point;
            ActionType = actionType;
            Old = oldPoint;
            New = newPoint;
            PointInd = pointInd;
        }

        internal void SetNewPoint(MyPointF newPoint)
        {
            New = newPoint;
            if (newPoint != null)
                NewCoord = newPoint.Point;
        }
    }
}

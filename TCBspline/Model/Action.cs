using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public PointActionType ActionType { get; private set; }
        public MyPointF Old { get; private set; }
        public MyPointF New { get; private set; }
        public int PointInd { get; private set; }

        internal PointAction(PointActionType actionType,
                             MyPointF oldPoint,
                             MyPointF newPoint,
                             int pointInd)
        {
            ActionType = actionType;
            Old = oldPoint;
            New = newPoint;
            PointInd = pointInd;
        }

        internal void SetNewPoint(MyPointF newPoint)
        {
            New = newPoint;
        }
    }
}

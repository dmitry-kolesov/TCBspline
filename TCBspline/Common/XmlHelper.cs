using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using TCBspline.Model;

namespace TCBspline.Common
{
    class XmlHelper
    {
        internal void Serialize(string path, List<MyPointF> points)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(PointsCollection));
                    var pointsCollection = new PointsCollection(points);
                    formatter.Serialize(fs, pointsCollection);
                }
            }
            catch (Exception ex)
            {
                // log ex;
                throw ex;
            }
        }

        internal List<MyPointF> Deserialize(string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(PointsCollection));
                    var pointsArray = (PointsCollection)formatter.Deserialize(fs);
                    var points = pointsArray.PointsArray.ToList();
                    return points;
                }
            }
            catch (Exception ex)
            {
                // log ex;
                throw ex;
            }
        }
    }
}

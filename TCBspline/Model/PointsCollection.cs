using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TCBspline.Model
{
    [Serializable()]
    [System.Xml.Serialization.XmlRoot("PointsCollection")]
    public class PointsCollection
    {
        public PointsCollection()
        { }

        public PointsCollection(List<MyPointF> list)
        {
            PointsArray = list.ToArray();
        }

        [XmlArray("Points")]
        [XmlArrayItem("MyPointF", typeof(MyPointF))]
        public MyPointF[] PointsArray { get; set; }
    }
}

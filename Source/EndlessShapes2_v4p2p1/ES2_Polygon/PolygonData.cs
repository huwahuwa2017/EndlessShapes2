using System.Collections.Generic;
using UnityEngine;

namespace EndlessShapes2.Polygon
{
    public class PolygonData
    {
        public PolygonType PolyType { get; private set; }

        public SideData[] Sides { get; private set; }

        public Vector3 NormalVector { get; private set; }

        public Vector2 UV { get; private set; }

        public PolygonData(PolygonType polygonType, int[] indexs, List<Vector3> vertices, Vector2 uv)
        {
            PolyType = polygonType;
            Sides = PolygonDataControl.GenerateSides(indexs, vertices);
            UV = uv;

            switch (polygonType)
            {
                case PolygonType.Line:
                    NormalVector = Vector3.zero;
                    break;
                default:
                    NormalVector = Vector3.Cross(Sides[0].SideVector, Sides[1].SideVector).normalized;
                    break;
            }
        }

        public PolygonData EasyClone()
        {
            return (PolygonData)MemberwiseClone();
        }
    }
}

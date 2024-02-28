using System.Collections.Generic;
using UnityEngine;

namespace EndlessShapes2.Polygon
{
    public struct SideData
    {
        public int SideIndex { get; private set; }

        public Vector3 OriginPosition { get; private set; }

        public Vector3 TargetPosition { get; private set; }

        public Vector3 SideVector { get; private set; }

        public Vector3 Midpoint { get; private set; }

        public float Length { get; private set; }

        public SideData(int sideIndex, int originVertexIndex, int targetVertexIndex, List<Vector3> vertexList)
        {
            SideIndex = sideIndex;
            OriginPosition = vertexList[originVertexIndex];
            TargetPosition = vertexList[targetVertexIndex];

            SideVector = TargetPosition - OriginPosition;
            Midpoint = (OriginPosition + TargetPosition) / 2;
            Length = SideVector.magnitude;
        }
    }
}

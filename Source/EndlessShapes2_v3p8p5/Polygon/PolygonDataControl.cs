using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EndlessShapes2.Polygon
{
    public static class PolygonDataControl
    {
        private static float[] _cv = new float[] { 0f, 0.382683f, 0.707107f, 0.923880f, 1f };

        private static Vector3[] _circleValue = new Vector3[]
        {
            new Vector3(0,  _cv[0],  _cv[4]), new Vector3(0,  _cv[1],  _cv[3]), new Vector3(0,  _cv[2],  _cv[2]), new Vector3(0,  _cv[3],  _cv[1]),
            new Vector3(0,  _cv[4], -_cv[0]), new Vector3(0,  _cv[3], -_cv[1]), new Vector3(0,  _cv[2], -_cv[2]), new Vector3(0,  _cv[1], -_cv[3]),
            new Vector3(0, -_cv[0], -_cv[4]), new Vector3(0, -_cv[1], -_cv[3]), new Vector3(0, -_cv[2], -_cv[2]), new Vector3(0, -_cv[3], -_cv[1]),
            new Vector3(0, -_cv[4],  _cv[0]), new Vector3(0, -_cv[3],  _cv[1]), new Vector3(0, -_cv[2],  _cv[2]), new Vector3(0, -_cv[1],  _cv[3])
        };

        private static bool _uvReadable = false;

        public static float AllowableError_length { get; set; } = 0.001f;

        public static bool UV_Load { get; set; } = true;

        public static void PolygonClassify(List<PolygonData> polygonDataList, List<int[][]> faceDatas, List<int[]> lineDatas, List<Vector3> vertices, List<Vector2> uvs = null)
        {
            List<int[][]> faces = new List<int[][]>(faceDatas);
            faces.Sort((a, b) => b.Length - a.Length);

            _uvReadable = UV_Load && (faces[0][0].Length >= 2) && (uvs != null);

            while (faces.Count > 0)
            {
                int[][] face = faces[0];
                int vertexCount = face.Length;

                switch (vertexCount)
                {
                    case 3:
                        TriangleClassify(polygonDataList, face, vertices, uvs);

                        break;
                    case 4:
                        if (!RectangleClassify(polygonDataList, face, vertices, uvs))
                        {
                            NgonDisassembly(faces, face);
                        }

                        break;
                    case 16:
                        if (!EllipseClassify(polygonDataList, face, vertices, uvs))
                        {
                            NgonDisassembly(faces, face);
                        }

                        break;
                    default:
                        NgonDisassembly(faces, face);

                        break;
                }

                faces.RemoveAt(0);

                if (faces.Count > 0 && vertexCount != faces[0].Length)
                {
                    faces.Sort((a, b) => b.Length - a.Length);
                }
            }

            foreach (int[] temp_0 in lineDatas)
            {
                List<int[]> lineList = new List<int[]>();

                for (int index = 1; index < temp_0.Length; ++index)
                {
                    lineList.Add(new int[] { temp_0[index - 1], temp_0[index] });
                }

                foreach (int[] temp_1 in lineList)
                {
                    LineClassify(polygonDataList, temp_1, vertices);
                }
            }
        }

        public static void TriangleClassify(List<PolygonData> polygonDataList, int[][] faceData, List<Vector3> vertices, List<Vector2> uvs)
        {
            Vector2 UV_Mid = UV_Midpoint(faceData, uvs);

            int[] V_indexs = faceData.Select(I => I[0]).ToArray();

            SideData[] sides = GenerateSides(V_indexs, vertices);
            SideData[] sidesLengthSort = sides.OrderByDescending((SideData s) => s.Length).ToArray();

            int[] next = { 1, 2, 0 };
            float variable_0 = Mathf.Abs(Vector3.Angle(sidesLengthSort[1].SideVector, sidesLengthSort[2].SideVector) - 90f);
            bool flag0 = sidesLengthSort[0].Length * Mathf.Sin(variable_0 * Mathf.Deg2Rad) < AllowableError_length;

            if (flag0)
            {
                int startSideIndex = sidesLengthSort[0].SideIndex;
                int[] newIndexs = new int[] { V_indexs[startSideIndex], V_indexs[next[startSideIndex]], V_indexs[next[next[startSideIndex]]] };

                polygonDataList.Add(new PolygonData(PolygonType.RightTriangle, newIndexs, vertices, UV_Mid));
            }
            else
            {
                bool flag1 = Mathf.Abs(sidesLengthSort[1].Length - sidesLengthSort[2].Length) < AllowableError_length;
                bool flag2 = Mathf.Abs(sidesLengthSort[0].Length - sidesLengthSort[1].Length) < AllowableError_length;

                if (flag1 || flag2)
                {
                    int startSideIndex = sidesLengthSort[flag1 ? 0 : 2].SideIndex;
                    int[] newIndexs = new int[] { V_indexs[startSideIndex], V_indexs[next[startSideIndex]], V_indexs[next[next[startSideIndex]]] };

                    polygonDataList.Add(new PolygonData(PolygonType.IsoscelesTriangle, newIndexs, vertices, UV_Mid));
                }
                else
                {
                    int startSideIndex = sidesLengthSort[0].SideIndex;
                    int[] newIndexs = new int[] { V_indexs[startSideIndex], V_indexs[next[startSideIndex]], V_indexs[next[next[startSideIndex]]] };

                    polygonDataList.Add(new PolygonData(PolygonType.OtherTriangle_F, newIndexs, vertices, UV_Mid));
                    polygonDataList.Add(new PolygonData(PolygonType.OtherTriangle_B, newIndexs, vertices, UV_Mid));
                }
            }
        }

        public static bool RectangleClassify(List<PolygonData> polygonDataList, int[][] faceData, List<Vector3> vertices, List<Vector2> uvs)
        {
            int[] V_indexs = faceData.Select(I => I[0]).ToArray();

            SideData[] sides = GenerateSides(V_indexs, vertices);
            SideData[] sidesLengthSort = sides.OrderByDescending((SideData s) => s.Length).ToArray();

            float LongestSideLength = sidesLengthSort[0].Length;

            float variable_0 = Mathf.Abs(Vector3.Angle(sides[0].SideVector, sides[1].SideVector) - 90f);
            bool flag0 = LongestSideLength * Mathf.Sin(variable_0 * Mathf.Deg2Rad) < AllowableError_length;
            if (!flag0) return false;

            float variable_1 = Mathf.Abs(Vector3.Angle(sides[1].SideVector, sides[2].SideVector) - 90f);
            bool flag1 = LongestSideLength * Mathf.Sin(variable_1 * Mathf.Deg2Rad) < AllowableError_length;
            if (!flag1) return false;

            float variable_2 = Mathf.Abs(Vector3.Angle(sides[2].SideVector, sides[3].SideVector) - 90f);
            bool flag2 = LongestSideLength * Mathf.Sin(variable_2 * Mathf.Deg2Rad) < AllowableError_length;
            if (!flag2) return false;

            float variable_3 = Mathf.Abs(Vector3.Angle(sides[3].SideVector, sides[0].SideVector) - 90f);
            bool flag3 = LongestSideLength * Mathf.Sin(variable_3 * Mathf.Deg2Rad) < AllowableError_length;
            if (!flag3) return false;

            Vector2 UV_Mid = UV_Midpoint(faceData, uvs);
            polygonDataList.Add(new PolygonData(PolygonType.Rectangle, V_indexs, vertices, UV_Mid));
            return true;
        }

        public static bool EllipseClassify(List<PolygonData> polygonDataList, int[][] faceData, List<Vector3> vertices, List<Vector2> uvs)
        {
            int[] V_indexs = faceData.Select(I => I[0]).ToArray();

            SideData[] sides = GenerateSides(V_indexs, vertices);
            SideData[] sidesLengthSort = sides.OrderByDescending((SideData s) => s.Length).ToArray();

            Vector3 center = Vector3.zero;
            Array.ForEach(sides, I => center += I.OriginPosition);
            center /= 16;

            SideData[] tempSort = sides.OrderByDescending((SideData s) => (s.OriginPosition - center).sqrMagnitude).ToArray();

            List<int> order = Enumerable.Range(0, 16).ToList();
            List<int> next = new List<int>(order);
            next.RemoveAt(0);
            next.Add(0);

            SideData mainSide_0 = tempSort[0];
            SideData mainSide_1 = sides[next[next[next[next[mainSide_0.SideIndex]]]]];

            Vector3 fv = mainSide_0.OriginPosition - center;
            Vector3 uv = mainSide_1.OriginPosition - center;
            Quaternion q = Quaternion.Inverse(Quaternion.LookRotation(fv, uv));

            float a = (mainSide_0.OriginPosition - center).magnitude;
            float b = (mainSide_1.OriginPosition - center).magnitude;
            Vector3 scale = new Vector3(0, b, a);
            Vector3[] newCircleValue = _circleValue.Select(I => Vector3.Scale(I, scale)).ToArray();



            int nextIndex = mainSide_0.SideIndex;
            bool flag_1 = true;

            for (int i = 0; i < 16; ++i)
            {
                Vector3 temp_3 = q * (sides[nextIndex].OriginPosition - center);
                Vector3 temp_4 = temp_3 - newCircleValue[i];

                bool flag_0 = Mathf.Abs(temp_4.x) < AllowableError_length && Mathf.Abs(temp_4.y) < AllowableError_length && Mathf.Abs(temp_4.z) < AllowableError_length;

                if (!flag_0)
                {
                    flag_1 = false;
                    break;
                }

                nextIndex = next[nextIndex];
            }

            if (flag_1)
            {
                List<int> newIndexs = new List<int>();
                int nextIndex_1 = mainSide_0.SideIndex;

                for (int i = 0; i < 16; ++i)
                {
                    newIndexs.Add(V_indexs[nextIndex_1]);
                    nextIndex_1 = next[nextIndex_1];
                }

                Vector2 UV_Mid = UV_Midpoint(faceData, uvs);
                polygonDataList.Add(new PolygonData(PolygonType.Ellipse, newIndexs.ToArray(), vertices, UV_Mid));
            }

            return flag_1;
        }

        public static void LineClassify(List<PolygonData> polygonDataList, int[] indexs, List<Vector3> vertices)
        {
            polygonDataList.Add(new PolygonData(PolygonType.Line, indexs, vertices, Vector2.zero));
        }

        private static Vector2 UV_Midpoint(int[][] faceData, List<Vector2> uvs)
        {
            Vector2 result = Vector2.zero;

            if (!_uvReadable) return result;

            int[] UV_indexs = faceData.Select(I => I[1]).ToArray();
            Vector2 temp_0 = Vector2.zero;

            foreach (int index in UV_indexs)
            {
                temp_0 += uvs[index];
            }

            result = temp_0 / UV_indexs.Length;

            return result;
        }

        public static SideData[] GenerateSides(int[] indexs, List<Vector3> vertices)
        {
            List<int> order = Enumerable.Range(0, indexs.Length).ToList();
            List<int> next = new List<int>(order);
            next.RemoveAt(0);
            next.Add(0);

            return order.Select(I => new SideData(I, indexs[I], indexs[next[I]], vertices)).ToArray();
        }

        public static void NgonDisassembly(List<int[][]> faces, int[][] face)
        {
            for (int index = 0; index < face.Length - 2; ++index)
            {
                faces.Add(new int[][] { face[0], face[1 + index], face[2 + index] });
            }
        }
    }
}

using BrilliantSkies.Core.Help;
using System;
using UnityEngine;

namespace EndlessShapes2.Polygon
{
    public static class MADCD_Generation
    {
        public static bool NormalReversal { get; set; } = false;

        public static float FaceThickness { get; set; } = 0.05f;

        public static float LineThickness { get; set; } = 0.05f;

        public static StructureBlockType SBType { get; set; } = StructureBlockType.Metal;

        public static Func<PolygonData, int> ColorSetting { get; set; } = null;

        public static void Generate(MimicAndDecorationCommonData madcd, PolygonData polygonData)
        {
            int colorIndex = -1;

            if (ColorSetting != null)
            {
                colorIndex = ColorSetting(polygonData);
            }

            StructureBlockGUID SBGUID = StructureBlockGUID_Memory.GetSBGUID(SBType);

            SideData[] sides = polygonData.Sides;
            Vector3 normalVector = polygonData.NormalVector * ((NormalReversal ? -FaceThickness : FaceThickness) / 2);

            Vector3 position;
            Vector3 scare;
            Vector3 angles;

            switch (polygonData.PolyType)
            {
                case PolygonType.RightTriangle:
                    position = sides[0].Midpoint - normalVector;
                    scare = new Vector3(FaceThickness, sides[2].Length, sides[1].Length);
                    angles = Quaternion.LookRotation(-sides[1].SideVector, sides[2].SideVector).eulerAngles;

                    InputMAD_Data(madcd, SBGUID.Slope, position, scare, angles, colorIndex);
                    break;
                case PolygonType.IsoscelesTriangle:
                    Vector3 variable_0 = sides[1].TargetPosition;
                    Vector3 variable_1 = (sides[0].Midpoint + variable_0) / 2;
                    Vector3 variable_2 = variable_0 - sides[0].Midpoint;

                    position = variable_1 - normalVector;
                    scare = new Vector3(sides[0].Length, FaceThickness, variable_2.magnitude);
                    angles = Quaternion.LookRotation(variable_2, normalVector).eulerAngles;

                    InputMAD_Data(madcd, SBGUID.Wedge, position, scare, angles, colorIndex);
                    break;
                case PolygonType.OtherTriangle_F:
                    Vector3 forwardVector_0 = sides[0].SideVector;
                    float scareY_0 = sides[1].Length * Mathf.Sin(Vector3.Angle(-forwardVector_0, sides[1].SideVector) * Mathf.Deg2Rad);
                    float variable_4 = sides[1].Length * Mathf.Sin(Mathf.Acos(scareY_0 / sides[1].Length));

                    position = sides[1].Midpoint - normalVector;
                    scare = new Vector3(FaceThickness, scareY_0, variable_4);
                    angles = Quaternion.LookRotation(forwardVector_0, sides[1].SideVector).eulerAngles;

                    InputMAD_Data(madcd, SBGUID.Slope, position, scare, angles, colorIndex);
                    break;
                case PolygonType.OtherTriangle_B:
                    Vector3 forwardVector_1 = sides[0].SideVector;
                    float scareY_1 = sides[1].Length * Mathf.Sin(Vector3.Angle(-forwardVector_1, sides[1].SideVector) * Mathf.Deg2Rad);
                    float variable_5 = sides[2].Length * Mathf.Sin(Mathf.Acos(scareY_1 / sides[2].Length));

                    position = sides[2].Midpoint - normalVector;
                    scare = new Vector3(FaceThickness, scareY_1, variable_5);
                    angles = Quaternion.LookRotation(-forwardVector_1, -sides[2].SideVector).eulerAngles;

                    InputMAD_Data(madcd, SBGUID.Slope, position, scare, angles, colorIndex);
                    break;
                case PolygonType.Rectangle:
                    position = (sides[0].OriginPosition + sides[2].OriginPosition) / 2 - normalVector;
                    scare = new Vector3(FaceThickness, sides[1].Length, sides[0].Length);
                    angles = Quaternion.LookRotation(sides[0].SideVector, sides[1].SideVector).eulerAngles;

                    InputMAD_Data(madcd, SBGUID.Block, position, scare, angles, colorIndex);
                    break;
                case PolygonType.Ellipse:
                    Vector3 rv = sides[8].OriginPosition - sides[0].OriginPosition;
                    Vector3 uv = sides[12].OriginPosition - sides[4].OriginPosition;
                    Vector3 fv = Vector3.Cross(rv, uv);

                    Vector3 center = Vector3.zero;
                    Array.ForEach(sides, I => center += I.OriginPosition);
                    center /= 16;

                    position = center - normalVector;
                    scare = new Vector3(rv.magnitude, uv.magnitude, FaceThickness);
                    angles = Quaternion.LookRotation(fv, uv).eulerAngles;

                    InputMAD_Data(madcd, SBGUID.Pole, position, scare, angles, colorIndex);
                    break;
                case PolygonType.Line:
                    position = (sides[0].OriginPosition + sides[0].TargetPosition) / 2;
                    scare = new Vector3(LineThickness, LineThickness, sides[0].Length);
                    angles = Quaternion.LookRotation(sides[0].SideVector, Vector3.up).eulerAngles;

                    InputMAD_Data(madcd, SBGUID.Pole, position, scare, angles, colorIndex);
                    break;
            }
        }

        private static void InputMAD_Data(MimicAndDecorationCommonData madcd, Guid guid, Vector3 position, Vector3 scare, Vector3 angles, int colorIndex)
        {
            madcd.MeshGuid = guid;
            madcd.Positioning = new Vector3(Rounding.R4(position.x), Rounding.R4(position.y), Rounding.R4(position.z));
            madcd.Scaling = new Vector3(Rounding.R4(scare.x), Rounding.R4(scare.y), Rounding.R4(scare.z));
            madcd.Orientation = new Vector3(Rounding.R4(angles.x), Rounding.R4(angles.y), Rounding.R4(angles.z));

            if (colorIndex >= 0)
            {
                madcd.ColorIndex = colorIndex;
            }
        }
    }
}

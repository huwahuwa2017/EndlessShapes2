using BrilliantSkies.Core.Timing;
using BrilliantSkies.Core.Types;
using BrilliantSkies.Ftd.Avatar.Build;
using BrilliantSkies.Ftd.Avatar.Build.UndoRedo;
using BrilliantSkies.Ftd.Constructs.Connections;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Containers;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Ui.Tips;
using EndlessShapes2.Polygon;
using EndlessShapes2.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace EndlessShapes2
{
    public class DecorationBuilder : Block
    {
        private List<PolygonData> _polygonDataList = new List<PolygonData>();

        private List<Color> _colorPalette = new List<Color>();

        private Texture2D _texture;

        public DecorationBuilderData Data { get; set; } = new DecorationBuilderData(1U);

        public List<OBJ_Mesh> Meshes { get; } = new List<OBJ_Mesh>();

        public List<Vector3> Vertices { get; } = new List<Vector3>();

        public List<Vector2> UVs { get; } = new List<Vector2>();

        public OBJ_Mesh SelectMesh { get; private set; }

        public int PolygonDataListCount => _polygonDataList.Count;



        protected override void AppendToolTip(ProTip tip)
        {
            base.AppendToolTip(tip);

            tip.Add(new ProTipSegment_Text(200, "Test"));
        }

        public override void Secondary(Transform T)
        {
            new DBUI_SettingWindows(this).ActivateGui();
        }



        public void Load()
        {
            Meshes.Clear();
            Vertices.Clear();
            UVs.Clear();

            if (!File.Exists(Data.OBJ_FilePath.Us)) return;

            string InputLine;
            StreamReader Reader = new StreamReader(Data.OBJ_FilePath.Us);

            while ((InputLine = Reader.ReadLine()) != null)
            {
                List<string> SA = InputLine.Split(' ').ToList();

                switch (SA[0])
                {
                    case "o":
                        Meshes.Insert(0, new OBJ_Mesh { Name = SA[1] });
                        break;
                    case "v":
                        Vertices.Add(new Vector3(-float.Parse(SA[1]), float.Parse(SA[2]), float.Parse(SA[3])));
                        break;
                    case "vt":
                        UVs.Add(new Vector2(float.Parse(SA[1]), float.Parse(SA[2])));
                        break;
                    case "f":
                        SA.RemoveAt(0);
                        int[][] temp = SA.Select(I0 => I0.Split('/').Select(I1 => int.Parse(I1) - 1).ToArray()).Reverse().ToArray();
                        Meshes[0].FaceDatas.Add(temp);
                        break;
                    case "l":
                        SA.RemoveAt(0);
                        Meshes[0].LineDatas.Add(SA.Select(I => int.Parse(I) - 1).ToArray());
                        break;
                }
            }
        }

        public void SetSelectMesh(OBJ_Mesh mesh)
        {
            SelectMesh = mesh;
            _polygonDataList.Clear();

            List<Vector3> newList = Enumerable.Repeat(default(Vector3), Vertices.Count).ToList();

            AllConstruct myC = GetC();
            bool isMain = myC.IsMain;

            Vector3 temp_0 = Vector3.zero;
            Quaternion temp_1 = Quaternion.identity;

            if (!isMain)
            {
                Transform myCtransform = myC.myTransform;
                Transform mainCtransform = myC.Main.myTransform;

                Quaternion temp_2 = Quaternion.Inverse(mainCtransform.rotation);
                temp_0 = temp_2 * (myCtransform.position - mainCtransform.position);
                temp_1 = Quaternion.Inverse(temp_2 * myCtransform.rotation);
            }

            Parallel.For(0, Vertices.Count,
                (int index) =>
                {
                    Vector3 vartex = Vertices[index];
                    vartex = Vector3.Scale(vartex, Data.Scaling.Us);
                    vartex = Quaternion.Euler(Data.Orientation.Us) * vartex;

                    if (Data.LocalOriginProjection)
                    {
                        vartex += Data.Positioning.Us;

                        if (!isMain)
                        {
                            vartex = temp_1 * (vartex - temp_0);
                        }
                    }
                    else
                    {
                        vartex += Data.Positioning.Us + LocalPosition;
                    }

                    newList[index] = vartex;
                });

            PolygonDataControl.PolygonClassify(_polygonDataList, mesh.FaceDatas, mesh.LineDatas, newList, UVs);
        }



        private AllConstruct _myAllConstruct;
        private AllConstructDecorations _acd;
        private ItemDefinition _itemDef;
        private bool _itemDefFound;
        private List<PolygonData> _buildMemory;
        private float _tcf = 0;

        public void Start()
        {
            _myAllConstruct = GetC();
            if (_myAllConstruct == null) return;

            _acd = _myAllConstruct.Decorations as AllConstructDecorations;
            if (_acd == null || _acd.DecorationCount + _polygonDataList.Count >= AllConstructDecorations._limitPerPacketManager) return;

            ConnectionRules connectionRules = _myAllConstruct.Main.ConnectionRules as ConnectionRules;
            if (connectionRules == null) return;

            _itemDef = Configured.i.Get<ModificationComponentContainerItem>().Find(new Guid(Data.TP_BlockGUID.Us), out _itemDefFound);

            for (int index = 0; index < 28; ++index)
            {
                _colorPalette.Add(GetC().Main.ColorsRestricted.GetColor(index));
            }

            _texture = null;

            if (File.Exists(Data.TexturePath.Us))
            {
                _texture = new Texture2D(2, 2);
                _texture.LoadImage(File.ReadAllBytes(Data.TexturePath.Us));
                _texture.Compress(false);
            }

            connectionRules.Data.MasterSwitch.Us = false;
            connectionRules.Data.RequestSwitch.Us = false;

            MADCD_Generation.NormalReversal = false;
            MADCD_Generation.FaceThickness = Data.FaceThickness.Us;
            MADCD_Generation.LineThickness = Data.LineThickness.Us;
            MADCD_Generation.SBType = Data.SBType.Us;
            MADCD_Generation.ColorSetting = ColorSetting;

            if (Data.BuildAnimation.Us)
            {
                _buildMemory = new List<PolygonData>(_polygonDataList);
                _tcf = 0;
                MainConstruct.SchedulerRestricted.RegisterForFixedUpdate(BuildAnimation);
            }
            else
            {
                foreach (PolygonData polygonData in _polygonDataList)
                {
                    Generate(polygonData);
                }
            }
        }

        private void BuildAnimation(ITimeStep t)
        {
            _tcf += t.DeltaTime * Data.BA_Speed.Us;
            int tci = Mathf.FloorToInt(_tcf);
            _tcf -= tci;

            for (int index = 0; index < tci; ++index)
            {
                if (_buildMemory.Count <= 0)
                {
                    MainConstruct.SchedulerRestricted.UnregisterForFixedUpdate(BuildAnimation);
                    return;
                }

                Generate(_buildMemory[0]);
                _buildMemory.RemoveAt(0);
            }
        }

        private void Generate(PolygonData polygonData)
        {
            MimicAndDecorationCommonData MAD_Data = new MimicAndDecorationCommonData();

            MADCD_Generation.Generate(MAD_Data, polygonData);

            Vector3i position = LocalPosition;

            if (Data.TP_AutomaticMove.Us)
            {
                Vector3 temp_3 = Vector3Int.RoundToInt(MAD_Data.Positioning);

                if (Data.TP_NormalOffset.Us)
                {
                    Vector3 temp_2 = temp_3;
                    Vector3Int temp_4 = Vector3Int.RoundToInt(temp_2);
                    temp_3 = temp_2 - polygonData.NormalVector * Data.TP_DistanceToShift.Us;

                    if (temp_4.x == 0 || Mathf.Sign(temp_2.x) != Mathf.Sign(temp_3.x))
                    {
                        temp_3.x = 0;
                    }

                    if (temp_4.y == 0 || Mathf.Sign(temp_2.y) != Mathf.Sign(temp_3.y))
                    {
                        temp_3.y = 0;
                    }

                    if (temp_4.z == 0 || Mathf.Sign(temp_2.z) != Mathf.Sign(temp_3.z))
                    {
                        temp_3.z = 0;
                    }
                }

                position = (Vector3i)temp_3;
            }

            MAD_Data.Positioning -= position;

            if (_itemDefFound && _myAllConstruct.AllBasics.GetBlockViaLocalPosition(position) == null)
            {
                PlaceBlockCommand placeBlockCommand = new PlaceBlockCommand(_myAllConstruct, position, Quaternion.identity, _itemDef, 0, MirrorInfo.none);
                placeBlockCommand.Apply();
            }

            Decoration decoration = new Decoration();
            decoration.Initialise(_myAllConstruct, position);
            _acd.Packets.NewPackage(decoration);
            _myAllConstruct.Chunks.RenderableAdded(decoration);
            decoration.Changed();
            Traverse.Create(_acd).Method("AddToPositionList", decoration, position).GetValue();

            MimicAndDecorationCommonData.Copy(MAD_Data, new MimicAndDecorationCommonData(decoration));
        }

        private int ColorSetting(PolygonData polygonData)
        {
            int colorIndex = Data.DefaultColorIndex.Us;

            if (_texture == null || polygonData.PolyType == PolygonType.Line) return colorIndex;

            Color pColor = _texture.GetPixelBilinear(polygonData.UV.x, polygonData.UV.y);
            float difference = 10f;

            for (int index = 0; index < _colorPalette.Count; ++index)
            {
                Color temp_0 = _colorPalette[index];
                float temp_1 = new Vector3(temp_0.r - pColor.r, temp_0.g - pColor.g, temp_0.b - pColor.b).magnitude + Math.Abs(temp_0.a - pColor.a);

                if (temp_1 < difference)
                {
                    difference = temp_1;
                    colorIndex = index;
                }
            }

            return colorIndex;
        }
    }
}

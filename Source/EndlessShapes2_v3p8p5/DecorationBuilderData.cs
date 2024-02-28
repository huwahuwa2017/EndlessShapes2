using BrilliantSkies.Core.Serialisation.Parameters.Prototypes;
using BrilliantSkies.Core.Widgets;
using EndlessShapes2.Polygon;
using UnityEngine;

namespace EndlessShapes2
{
    public class DecorationBuilderData : PrototypeSystem
    {
        public DecorationBuilderData(uint uniqueId) : base(uniqueId)
        {
        }

        [Variable(0U, "Face thickness", "The thickness of the face to be generated.")]
        public Var<float> FaceThickness { get; set; } = new Var<float>(0.05f);

        [Variable(1U, "Line thickness", "The thickness of the line to be generated.")]
        public Var<float> LineThickness { get; set; } = new Var<float>(0.05f);

        [Variable(2U, "Material", "Material of decoration to generate.")]
        public Var<StructureBlockType> SBType { get; set; } = new Var<StructureBlockType>(StructureBlockType.Metal);

        [Variable(3U, "Default color index", "If you can't get the color from the texture, or set the line to this color.")]
        public Var<int> DefaultColorIndex { get; set; } = new Var<int>(0);

        [Variable(4U, "Texture path", "Load the texture (png, jpg). This assigns the number of the color palette closest to the color in the center of the polygon.")]
        public VarString TexturePath { get; set; } = new VarString(string.Empty);

        [Variable(5U, "OBJ file path", "Load the 3D model in OBJ format.")]
        public VarString OBJ_FilePath { get; set; } = new VarString(string.Empty);


        [Variable(6U, "Positioning", "The positioning of the model")]
        public VarVector3 Positioning { get; set; } = new VarVector3(Vector3.zero);

        [Variable(7U, "Scaling", "The scaling of the model")]
        public VarVector3 Scaling { get; set; } = new VarVector3(Vector3.one);

        [Variable(8U, "Orientation", "The orientation of the model")]
        public VarVector3 Orientation { get; set; } = new VarVector3(Vector3.zero);


        [Variable(9U, "Tether Point automatic placement", "Place the Tether Point close to the Decoration.")]
        public VarBool TP_AutoTetherPoint { get; set; } = new VarBool(false);

        [Variable(10U, "Tether Point normal offset", "Shift Tether Point in the opposite direction of the polygon normal.")]
        public VarBool TP_NormalOffset { get; set; } = new VarBool(false);

        [Variable(11U, "Tether Point distance to shift", "Set the distance to shift in the opposite direction of the normal.")]
        public Var<float> TP_DistanceToShift { get; set; } = new Var<float>(0.87f);

        [Variable(12U, "Tether Point block placement", "If the block does not exist in the Tether Point, place the block.")]
        public VarBool TP_BlockPlacement { get; set; } = new VarBool(false);

        [Variable(13U, "Tether Point block GUID", "Specify the type of block to place in the GUID.")]
        public VarString TP_BlockGUID { get; set; } = new VarString("8bd20877-417f-4094-ab24-1ebae4d73f85");


        [Variable(14U, "Build animation", "Enable build animation.")]
        public VarBool BuildAnimation { get; set; } = new VarBool(false);

        [Variable(15U, "Build animation speed", "Set build speed per second.")]
        public Var<float> BA_Speed { get; set; } = new Var<float>(40f);


        [Variable(16U, "Local origin projection", "Projects a 3D model relative to the local origin.")]
        public VarBool LocalOrigin { get; set; } = new VarBool(false);
    }
}

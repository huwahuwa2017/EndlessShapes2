using AdvancedMimicUi;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Texts;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EndlessShapes2.UI
{
    public class DBUIAdvancedSettingTab : SuperScreen<DecorationBuilder>
    {
        public DBUIAdvancedSettingTab(ConsoleWindow window, DecorationBuilder focus) : base(window, focus)
        {
        }

        public override void Build()
        {
            CreateHeader("Offset", null);

            ScreenSegmentTable screenSegment_0 = CreateTableSegment(4, 5);
            screenSegment_0.SpaceAbove = 10f;
            screenSegment_0.SpaceBelow = 10f;
            screenSegment_0.SqueezeTable = false;

            screenSegment_0.AddInterpretter(StringDisplay.Quick("Add position", "Add a value for each parameter")).Justify = TextAnchor.UpperRight;
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Positioning.x), new ToolTip("Add a value to 'Left right positioning'"), (I, i) => I.Data.Positioning.x = i, M.m<DecorationBuilder>(I => "X ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Positioning.y), new ToolTip("Add a value to 'Up down positioning'"), (I, i) => I.Data.Positioning.y = i, M.m<DecorationBuilder>(I => "Y ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Positioning.z), new ToolTip("Add a value to 'Forward backward positioning'"), (I, i) => I.Data.Positioning.z = i, M.m<DecorationBuilder>(I => "Z ")));

            screenSegment_0.AddInterpretter(new Blank(6f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));

            screenSegment_0.AddInterpretter(StringDisplay.Quick("Add scale", "Add a value for each parameter")).Justify = TextAnchor.UpperRight;
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Scaling.x), new ToolTip("Add a value to 'Left right scaling'"), (I, i) => I.Data.Scaling.x = i, M.m<DecorationBuilder>(I => "X ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Scaling.y), new ToolTip("Add a value to 'Up down scaling'"), (I, i) => I.Data.Scaling.y = i, M.m<DecorationBuilder>(I => "Y ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Scaling.z), new ToolTip("Add a value to 'Forward backward scaling'"), (I, i) => I.Data.Scaling.z = i, M.m<DecorationBuilder>(I => "Z ")));

            screenSegment_0.AddInterpretter(new Blank(6f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));

            screenSegment_0.AddInterpretter(StringDisplay.Quick("Add angle", "Add a value for each parameter")).Justify = TextAnchor.UpperRight;
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Orientation.x), new ToolTip("Add a value to 'Pitch'"), (I, i) => I.Data.Orientation.x = i, M.m<DecorationBuilder>(I => "X ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Orientation.y), new ToolTip("Add a value to 'Yaw'"), (I, i) => I.Data.Orientation.y = i, M.m<DecorationBuilder>(I => "Y ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.Orientation.z), new ToolTip("Add a value to 'Roll'"), (I, i) => I.Data.Orientation.z = i, M.m<DecorationBuilder>(I => "Z ")));



            CreateHeader("Tether point", null);

            ScreenSegmentStandard screenSegment_1 = CreateStandardSegment();
            screenSegment_1.SpaceAbove = 10f;
            screenSegment_1.SpaceBelow = 10f;

            screenSegment_1.AddInterpretter(SubjectiveToggle<DecorationBuilder>.Quick(_focus, "Automatic move", null, (I, b) => { I.Data.TP_AutomaticMove.Us = b; TriggerScreenRebuild(); }, I => I.Data.TP_AutomaticMove.Us));

            if (_focus.Data.TP_AutomaticMove.Us)
            {
                screenSegment_1.AddInterpretter(SubjectiveToggle<DecorationBuilder>.Quick(_focus, "Normal offset", null, (I, b) => { I.Data.TP_NormalOffset.Us = b; TriggerScreenRebuild(); }, I => I.Data.TP_NormalOffset.Us));
            }

            if (_focus.Data.TP_NormalOffset.Us)
            {
                screenSegment_1.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.TP_DistanceToShift.Us), null, (I, i) => I.Data.TP_DistanceToShift.Us = i, M.m<DecorationBuilder>(I => "Distance to shift ")));
            }

            screenSegment_1.AddInterpretter(new Empty());

            screenSegment_1.AddInterpretter(SubjectiveToggle<DecorationBuilder>.Quick(_focus, "Block placement", null, (I, b) => { I.Data.TP_BlockPlacement.Us = b; TriggerScreenRebuild(); }, I => I.Data.TP_BlockPlacement.Us));

            if (_focus.Data.TP_BlockPlacement.Us)
            {
                screenSegment_1.AddInterpretter(TextInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.TP_BlockGUID.Us), "Block GUID", null, (I, s) => I.Data.TP_BlockGUID.Us = s));
            }

            CreateHeader("Others", null);

            ScreenSegmentStandard screenSegment_2 = CreateStandardSegment();
            screenSegment_2.SpaceAbove = 10f;
            screenSegment_2.SpaceBelow = 10f;

            screenSegment_2.AddInterpretter(SubjectiveToggle<DecorationBuilder>.Quick(_focus, "Build animation", null, (I, b) => { I.Data.BuildAnimation.Us = b; TriggerScreenRebuild(); }, I => I.Data.BuildAnimation.Us));

            if (_focus.Data.BuildAnimation.Us)
            {
                screenSegment_2.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.BA_Speed.Us), null, (I, i) => I.Data.BA_Speed.Us = i, M.m<DecorationBuilder>(I => "Build animation speed")));
            }

            screenSegment_2.AddInterpretter(new Empty());

            screenSegment_2.AddInterpretter(SubjectiveToggle<DecorationBuilder>.Quick(_focus, "Local origin projection", new ToolTip("Generates a 3D model relative to the local origin."), (I, b) => I.Data.LocalOriginProjection.Us = b, I => I.Data.LocalOriginProjection.Us));
        }
    }
}

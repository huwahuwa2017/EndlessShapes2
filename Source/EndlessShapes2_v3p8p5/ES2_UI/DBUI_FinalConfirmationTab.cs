using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using System;
using UnityEngine;

namespace EndlessShapes2.UI
{
    public class DBUI_FinalConfirmationTab : SuperScreen<DecorationBuilder>
    {
        private DBUI_FinalConfirmationWindows _mainUI;

        public DBUI_FinalConfirmationTab(ConsoleWindow window, DecorationBuilder focus, DBUI_FinalConfirmationWindows mainUI) : base(window, focus)
        {
            _mainUI = mainUI;
        }

        public override void Build()
        {
            ScreenSegmentTable screenSegment_0 = CreateTableSegment(2, 4);
            screenSegment_0.SpaceAbove = 20f;
            screenSegment_0.SpaceBelow = 20f;
            screenSegment_0.SetConditionalDisplay(() => _focus.SelectMesh != null);

            SubjectiveDisplay<DecorationBuilder> SubjectiveDisplay_DecorationBuilder(Func<DecorationBuilder, string> I) => screenSegment_0.AddInterpretter(SubjectiveDisplay<DecorationBuilder>.Quick(_focus, M.m(I)));

            SubjectiveDisplay_DecorationBuilder((I => "Name")).Justify = TextAnchor.UpperRight;
            SubjectiveDisplay_DecorationBuilder((I => I.SelectMesh.Name)).Justify = TextAnchor.UpperLeft;
            SubjectiveDisplay_DecorationBuilder((I => "Face count")).Justify = TextAnchor.UpperRight;
            SubjectiveDisplay_DecorationBuilder((I => I.SelectMesh.FaceDatas.Count.ToString())).Justify = TextAnchor.UpperLeft;
            SubjectiveDisplay_DecorationBuilder((I => "Line count")).Justify = TextAnchor.UpperRight;
            SubjectiveDisplay_DecorationBuilder((I => I.SelectMesh.LineDatas.Count.ToString())).Justify = TextAnchor.UpperLeft;
            SubjectiveDisplay_DecorationBuilder((I => "Number of decorations to generate")).Justify = TextAnchor.UpperRight;
            SubjectiveDisplay_DecorationBuilder((I => I.PolygonDataListCount.ToString())).Justify = TextAnchor.UpperLeft;

            ScreenSegmentStandard screenSegment_1 = CreateStandardSegment();
            screenSegment_1.SpaceAbove = 20f;
            screenSegment_1.SpaceBelow = 20f;

            screenSegment_1.AddInterpretter(SubjectiveButton<DecorationBuilder>.Quick(_focus, "Return to settings menu", null,
                (DecorationBuilder I) =>
                {
                    new DBUI_SettingWindows(_focus).ActivateGui(GuiActivateType.Stack); ;
                    _mainUI.DeactivateGui(GuiDeactivateType.Standard);
                }));
            screenSegment_1.AddInterpretter(new Empty());
            screenSegment_1.AddInterpretter(SubjectiveButton<DecorationBuilder>.Quick(_focus, "Build", null,
                (DecorationBuilder I) =>
                {
                    I.Start();

                    new DBUI_SettingWindows(_focus).ActivateGui(GuiActivateType.Stack); ;
                    _mainUI.DeactivateGui(GuiDeactivateType.Standard);
                }));
        }
    }
}

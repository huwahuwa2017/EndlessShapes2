using AdvancedMimicUi;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Texts;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Layouts.DropDowns;
using EndlessShapes2.Polygon;
using UnityEngine;

namespace EndlessShapes2.UI
{
    public class DBUI_BasicSettingTab : SuperScreen<DecorationBuilder>
    {
        private static DropDownMenuAlt<StructureBlockType> _SBGUID_DropDownMenuAlt = Preparation_0();

        private static DropDownMenuAlt<StructureBlockType> Preparation_0()
        {
            DropDownMenuAlt<StructureBlockType> dropDownMenuAlt = new DropDownMenuAlt<StructureBlockType>();

            dropDownMenuAlt.SetItems(new DropDownMenuAltItem<StructureBlockType>[]
            {
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Wood",
                    ObjectForAction = StructureBlockType.Wood
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Stone",
                    ObjectForAction = StructureBlockType.Stone
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Metal",
                    ObjectForAction = StructureBlockType.Metal
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Alloy",
                    ObjectForAction = StructureBlockType.Alloy
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Glass",
                    ObjectForAction = StructureBlockType.Glass
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Lead",
                    ObjectForAction = StructureBlockType.Lead
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Heavy armour",
                    ObjectForAction = StructureBlockType.HeavyArmour
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Rubber",
                    ObjectForAction = StructureBlockType.Rubber
                }
            });

            return dropDownMenuAlt;
        }



        private DBUI_SettingWindows _mainUI;

        public DBUI_BasicSettingTab(ConsoleWindow window, DecorationBuilder focus, DBUI_SettingWindows mainUI) : base(window, focus)
        {
            _mainUI = mainUI;
        }

        public override void Build()
        {
            ScreenSegmentStandard screenSegment_0 = CreateStandardSegment();
            screenSegment_0.SpaceAbove = 20f;
            screenSegment_0.SpaceBelow = 20f;

            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.FaceThickness.Us), null, (I, f) => I.Data.FaceThickness.Us = f, M.m<DecorationBuilder>(I => "FaceThickness : ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.LineThickness.Us), null, (I, f) => I.Data.LineThickness.Us = f, M.m<DecorationBuilder>(I => "LineThickness : ")));
            screenSegment_0.AddInterpretter(new DropDown<DecorationBuilder, StructureBlockType>(_focus, _SBGUID_DropDownMenuAlt, (I, s) => I.Data.SBType.Us.Equals(s), (I, s) => I.Data.SBType.Us = s));
            screenSegment_0.AddInterpretter(SubjectiveFloatClampedWithBar<DecorationBuilder>.Quick(_focus, 0f, 32f, 1f, M.m<DecorationBuilder>(I => I.Data.DefaultColorIndex.Us), "Color {0}", (I, f) => I.Data.DefaultColorIndex.Us = Mathf.RoundToInt(f), null));
            screenSegment_0.AddInterpretter(new Blank(20f));
            screenSegment_0.AddInterpretter(TextInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.TexturePath.Us), "TexturePath", null, (I, s) => I.Data.TexturePath.Us = s));
            screenSegment_0.AddInterpretter(new Blank(5f));
            screenSegment_0.AddInterpretter(TextInput<DecorationBuilder>.Quick(_focus, M.m<DecorationBuilder>(I => I.Data.OBJ_FilePath.Us), "FilePath", null, (I, s) => I.Data.OBJ_FilePath.Us = s));
            screenSegment_0.AddInterpretter(new Blank(5f));
            screenSegment_0.AddInterpretter(SubjectiveButton<DecorationBuilder>.Quick(_focus, "Load", null, I => { I.Load(); TriggerScreenRebuild(); }));
            screenSegment_0.AddInterpretter(new Blank(20f));

            foreach (OBJ_Mesh meshes in _focus.Meshes)
            {
                screenSegment_0.AddInterpretter(SubjectiveButton<DecorationBuilder>.Quick(_focus, meshes.Name, null,
                    (DecorationBuilder I) =>
                    {
                        I.SetSelectMesh(meshes);

                        new DBUI_FinalConfirmationWindows(_focus).ActivateGui(GuiActivateType.Stack); ;
                        _mainUI.DeactivateGui(GuiDeactivateType.Standard);
                    }));
            }
        }
    }
}

using BrilliantSkies.Ftd.Constructs.UI;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Consoles.Styles;
using HarmonyLib;

namespace EndlessShapes2
{
    class EndlessShapes2Patch
    {
        [HarmonyPatch(typeof(GeneralTab), "Mesh")]
        class GeneralTabPatch
        {
            static void Postfix(GeneralTab __instance)
            {
                ScreenSegmentStandard screenSegmentStandard = __instance.CreateStandardSegment(InsertPosition.OnCursor);
                screenSegmentStandard.SpaceAbove = 30f;
                screenSegmentStandard.BackgroundStyleWhereApplicable = ConsoleStyles.Instance.Styles.Segments.OptionalSegmentDarkBackgroundWithHeader.Style;
                screenSegmentStandard.NameWhereApplicable = "OBJ file creator";

                screenSegmentStandard.AddInterpretter(SubjectiveButton<ConstructInfo>.Quick(__instance._focus, "Create a OBJ file of this vehicle", null,
                    (ConstructInfo I) =>
                    {
                        OBJ_FileCreation.Start(I.Construct);
                    }));
            }
        }
    }
}

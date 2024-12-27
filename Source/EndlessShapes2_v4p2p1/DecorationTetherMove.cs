using BrilliantSkies.Core.Collections.Arrays;
using BrilliantSkies.Core.Constants;
using BrilliantSkies.Core.Timing;
using BrilliantSkies.Core.Types;
using BrilliantSkies.Ftd.Avatar.Build;
using BrilliantSkies.Ftd.Avatar.Build.UndoRedo;
using BrilliantSkies.Ftd.Avatar.Items;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Containers;
using BrilliantSkies.Modding.Types;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessShapes2
{
    public class DecorationTetherMove : CharacterItem
    {
        private static string TetherBlockGUID { get; set; } = "8bd20877-417f-4094-ab24-1ebae4d73f85";

        private static Block pointedBlock;



        public void OnEnable()
        {
            GameEvents.Four_Second_PlayerTime.RegWithEvent(new Action<ITimeStep>(UpdatePointedBlock));
        }

        public void OnDisable()
        {
            GameEvents.Four_Second_PlayerTime.UnregWithEvent(new Action<ITimeStep>(UpdatePointedBlock));
        }

        public void OnGUI()
        {
            if (Get.UserInput.AllGameControlsEnabled && cBuild.GetSingleton().IsInactive() && null != pointedBlock)
            {
                DisplayText(pointedBlock.Name);
            }
        }

        public override bool AreYouTwoHanded()
        {
            return true;
        }

        public override void LeftClick()
        {
            BlockMove(Vector3.forward);
        }

        public override void RightClick()
        {
            BlockMove(Vector3.back);
        }

        private void BlockMove(Vector3 direction)
        {
            ItemDefinition itemDef = Configured.i.Get<ModificationComponentContainerItem>().Find(new Guid(TetherBlockGUID), out bool flag_0);
            if (!flag_0) return;

            AllConstruct myAllConstruct = pointedBlock.GetC();
            if (myAllConstruct == null) return;

            AllConstructDecorations acd = myAllConstruct.Decorations as AllConstructDecorations;
            if (acd == null) return;

            Quaternion cameraRotation = CameraManager.GetSingleton().transform.rotation;
            Quaternion temp_0 = Quaternion.Inverse(myAllConstruct.myTransform.rotation) * cameraRotation;

            Vector3 temp_1 = temp_0 * direction;
            Vector3 temp_2 = new Vector3(Math.Abs(temp_1.x), Math.Abs(temp_1.y), Math.Abs(temp_1.z));

            if (temp_2.x > temp_2.y && temp_2.x > temp_2.z)
            {
                temp_1.y = 0;
                temp_1.z = 0;
            }

            if (temp_2.y > temp_2.z && temp_2.y > temp_2.x)
            {
                temp_1.z = 0;
                temp_1.x = 0;
            }

            if (temp_2.z > temp_2.x && temp_2.z > temp_2.y)
            {
                temp_1.x = 0;
                temp_1.y = 0;
            }

            Vector3i shiftPos = (Vector3i)temp_1;
            Vector3i position = pointedBlock.LocalPosition;

            ThreeDDictionary<List<Decoration>> _decorationArray = Traverse.Create(acd).Field("_decorationArray").GetValue<ThreeDDictionary<List<Decoration>>>();

            if (_decorationArray.TryRead(position.x, position.y, position.z, out List<Decoration> temp_3))
            {
                List<Decoration> decorations = new List<Decoration>(temp_3);

                Console.WriteLine("deco count" + decorations.Count.ToString());

                foreach (Decoration decoration in decorations)
                {
                    Vector3 temp_4 = decoration.Positioning.Us - shiftPos;

                    if (Mathf.Abs(temp_4.x) <= 10 && Mathf.Abs(temp_4.y) <= 10 && Mathf.Abs(temp_4.z) <= 10)
                    {
                        decoration.TetherPoint.Us += shiftPos;
                        decoration.Positioning.Us -= shiftPos;
                    }
                }
            }

            PlaceBlockCommand placeBlockCommand = new PlaceBlockCommand(myAllConstruct, position + shiftPos, Quaternion.identity, itemDef, 0, MirrorInfo.none);
            placeBlockCommand.Apply();

            RemoveBlockCommand removeBlockCommand = new RemoveBlockCommand(myAllConstruct, position, MirrorInfo.none);
            removeBlockCommand.Apply();
        }

        private static void UpdatePointedBlock(ITimeStep DeltaTime)
        {
            pointedBlock = GetPointedBlock();
        }
    }
}

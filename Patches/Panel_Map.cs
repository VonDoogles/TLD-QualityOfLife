using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Map ), "ShouldCenterOnPlayer" )]
    internal class Patch_Panel_Map_ShouldCenterOnPlayer
    {
        static bool Prefix( ref bool __result )
        {
            if ( Settings.Instance.MapShowPlayerIcon )
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch( typeof( Panel_Map ), "Update" )]
    internal class Patch_Panel_Map_Update
    {
        static int InputFrame = 0;

        static void Postfix( Panel_Map __instance )
        {
            int Frame = Time.frameCount;
            if ( Settings.Instance.UIExtraControls && InputFrame != Frame )
            {
                if ( Input.GetKeyDown( KeyCode.A ) )
                {
                    InputFrame = Frame;
                    __instance.OnPrevRegion();
                }

                if ( Input.GetKeyDown( KeyCode.D ) )
                {
                    InputFrame = Frame;
                    __instance.OnNextRegion();
                }

                if ( Input.GetKeyDown( Settings.Instance.EquipKey ) || Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    InputFrame = Frame;
                    __instance.ToggleWorldMap();
                }
            }
        }
    }

}

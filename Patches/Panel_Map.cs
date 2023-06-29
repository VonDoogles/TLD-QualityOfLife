using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Map ), "Update" )]
    internal class Patch_Panel_Map_Update
    {
        static int InputFrame = 0;

        static void Postfix( Panel_Map __instance )
        {
            int Frame = Time.frameCount;
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls && InputFrame != Frame )
            {
                if ( ModInput.GetKeyDown( __instance, KeyCode.A ) )
                {
                    InputFrame = Frame;
                    __instance.OnPrevRegion();
                }

                if ( ModInput.GetKeyDown( __instance, KeyCode.D ) )
                {
                    InputFrame = Frame;
                    __instance.OnNextRegion();
                }

                if ( ModInput.GetKeyDown( __instance, KeyCode.Tab ) )
                {
                    InputFrame = Frame;
                    __instance.ToggleWorldMap();
                }
            }
        }
    }

}

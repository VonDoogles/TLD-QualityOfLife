using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_BreakDown ), "Update" )]
    internal class Patch_Panel_BreakDown_Update
    {
        static void Postfix( Panel_BreakDown __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBreakDown();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 || InputManager.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.OnPrevTool();
                }
                else if ( Scroll < 0 || InputManager.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.OnNextTool();
                }
            }
        }
    }

}

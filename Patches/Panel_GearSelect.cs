using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_GearSelect ), "Update" )]
    internal class Patch_Panel_GearSelect_Update
    {
        static void Postfix( Panel_GearSelect __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.SelectGear();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 || Input.GetKeyDown( KeyCode.A ) )
                {
                    __instance.PreviousGear();
                }
                else if ( Scroll < 0 || Input.GetKeyDown( KeyCode.D ) )
                {
                    __instance.NextGear();
                }
            }
        }
    }
}

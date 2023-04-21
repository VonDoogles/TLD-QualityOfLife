using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_SelectRegion_Map ), "UpdateHoveredItem" )]
    internal class Patch_Panel_SelectRegion_Map_Update
    {
        static void Postfix( Panel_SelectRegion_Map __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) || Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnSelectRegionContinue();
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_SelectRegion_Map ), "UpdateDisplayedRegions" )]
    internal class Patch_Panel_SelectRegion_Map_UpdateAnimation
    {
        static void Postfix( Panel_SelectRegion_Map __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) || Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnSelectRegionContinue();
                }
            }
        }
    }

}

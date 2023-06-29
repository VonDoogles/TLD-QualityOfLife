using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_SelectRegion_Map ), "UpdateHoveredItem" )]
    internal class Patch_Panel_SelectRegion_Map_Update
    {
        static void Postfix( Panel_SelectRegion_Map __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
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
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnSelectRegionContinue();
                }
            }
        }
    }

}

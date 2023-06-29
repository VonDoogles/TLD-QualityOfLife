using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_IceFishing ), "Update" )]
    internal class Patch_Panel_IceFishing_Update
    {
        static void Postfix( Panel_IceFishing __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
				if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
				{
					__instance.OnFish();
				}

				float Scroll = InputManager.GetScroll( __instance );
				if ( Scroll < 0 || ModInput.GetKeyDown( __instance, KeyCode.A ) )
				{
					__instance.OnDecreaseHours();
				}
				else if ( Scroll > 0 || ModInput.GetKeyDown( __instance, KeyCode.D ) )
				{
					__instance.OnIncreaseHours();
				}
            }
        }
    }
}

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
            if ( Settings.Instance.UIExtraControls )
            {
				if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
				{
					__instance.OnFish();
				}

				float Scroll = InputManager.GetScroll( __instance );
				if ( Scroll < 0 || InputManager.GetKeyDown( __instance, KeyCode.A ) )
				{
					__instance.OnDecreaseHours();
				}
				else if ( Scroll > 0 || InputManager.GetKeyDown( __instance, KeyCode.D ) )
				{
					__instance.OnIncreaseHours();
				}
            }
        }
    }
}

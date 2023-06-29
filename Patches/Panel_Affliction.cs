using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Affliction ), "Update" )]
    internal class Patch_Panel_Affliction_Update
    {
        static void Postfix( Panel_Affliction __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.TreatWound();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 || ModInput.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.PreviousAffliction();
                }
                else if ( Scroll < 0 || ModInput.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.NextAffliction();
                }
            }
        }
    }
}

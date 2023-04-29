using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Badges ), "Update" )]
    internal class Patch_Panel_Badges_Update
    {
        static void Postfix( Panel_Badges __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, KeyCode.Tab ) )
                {
                    if ( __instance.m_FeatsObject.activeSelf )
                    {
                        __instance.OnChallenges();
                    }
                    else
                    {
                        __instance.OnFeats();
                    }
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 || InputManager.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.PrevTool();
                }
                else if ( Scroll < 0 || InputManager.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.NextTool();
                }
            }
        }
    }
}

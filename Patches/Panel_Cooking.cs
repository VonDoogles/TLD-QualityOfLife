using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Cooking ), "Update" )]
    internal class Patch_Panel_Cooking_Update
    {
        static void Postfix( Panel_Cooking __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnDoAction();
                }

                if ( __instance.m_CookingFilter == Panel_Cooking.CookingFilter.WaterOnly )
                {
                    float Scroll = InputManager.GetScroll( __instance );
                    if ( Scroll < 0 )
                    {
                        __instance.OnWaterDown();
                    }
                    else if ( Scroll > 0 )
                    {
                        __instance.OnWaterUp();
                    }
                }
            }
        }
    }
}

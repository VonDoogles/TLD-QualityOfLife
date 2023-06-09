﻿using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_GearSelect ), "Update" )]
    internal class Patch_Panel_GearSelect_Update
    {
        static void Postfix( Panel_GearSelect __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.SelectGear();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 || ModInput.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.PreviousGear();
                }
                else if ( Scroll < 0 || ModInput.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.NextGear();
                }
            }
        }
    }
}

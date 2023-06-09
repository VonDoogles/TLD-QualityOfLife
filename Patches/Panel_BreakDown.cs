﻿using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_BreakDown ), "Update" )]
    internal class Patch_Panel_BreakDown_Update
    {
        static void Postfix( Panel_BreakDown __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls && !__instance.IsBreakingDown() )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBreakDown();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 || ModInput.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.OnPrevTool();
                }
                else if ( Scroll < 0 || ModInput.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.OnNextTool();
                }
            }
        }
    }

}

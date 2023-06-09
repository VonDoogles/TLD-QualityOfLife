﻿using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( BasicMenu ), "ManualUpdate" )]
    internal class Patch_BasicMenu_ManualUpdate
    {
        static void Postfix( BasicMenu __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance.m_OwnerContext, Settings.Instance.InteractKey ) )
                {
                    __instance.OnItemDoubleClicked( __instance.m_MenuSelectedButtonIndex );
                }
            }
        }
    }

}

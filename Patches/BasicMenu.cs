using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( BasicMenu ), "ManualUpdate" )]
    internal class Patch_BasicMenu_ManualUpdate
    {
        static void Postfix( BasicMenu __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) || Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnItemDoubleClicked( __instance.m_MenuSelectedButtonIndex );
                }
            }
        }
    }

}

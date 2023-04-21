using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_SnowShelterInteract ), "Update" )]
    internal class Patch_Panel_SnowShelterInteract_Update
    {
        static void Postfix( Panel_SnowShelterInteract __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    int Index = __instance.m_SelectedButtonIndex;
                    if ( Index >= 0 && Index < __instance.m_ButtonDelegates.Count )
                    {
                        __instance.m_ButtonDelegates[ Index ].Invoke();
                    }
                }
            }
        }
    }
}

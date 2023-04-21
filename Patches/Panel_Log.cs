using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Log ), "Update" )]
    internal class Patch_Panel_Log_Update
    {
        static void Postfix( Panel_Log __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    if ( __instance.m_CollectibleSectionObject.active )
                    {
                        __instance.OnCollectionsSubScreen();
                    }
                }
            }
        }
    }
}

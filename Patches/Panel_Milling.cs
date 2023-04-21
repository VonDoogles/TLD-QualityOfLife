using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Milling ), "Update" )]
    internal class Patch_Panel_Milling_Update
    {
        static void Postfix( Panel_Milling __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnRepair();
                }
            }
        }
    }
}

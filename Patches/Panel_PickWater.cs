using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_PickWater ), "Update" )]
    internal class Patch_Panel_PickWater_Update
    {
        static void Postfix( Panel_PickWater __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnExecute();
                }

                if ( Input.GetKeyDown( Settings.Instance.EquipKey ) )
                {
                    __instance.OnExecuteAll();
                }
            }
        }
    }
}

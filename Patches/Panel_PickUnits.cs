using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife.Patches
{
    [HarmonyPatch( typeof( Panel_PickUnits ), "Update" )]
    internal class Patch_Panel_PickUnits_Update
    {
        static void Postfix( Panel_PickUnits __instance )
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

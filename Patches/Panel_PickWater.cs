using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_PickWater ), "Update" )]
    internal class Patch_Panel_PickWater_Update
    {
        static void Postfix( Panel_PickWater __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnExecute();
                }

                if ( InputManager.GetKeyDown( __instance, Settings.Instance.EquipKey ) )
                {
                    __instance.OnExecuteAll();
                }
            }
        }
    }
}

using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Inventory ), "Update" )]
    internal class Patch_Panel_Inventory_Update
    {
        static void Postfix( Panel_Inventory __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.DropKey ) )
                {
                    __instance.OnDrop();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.EquipKey ) )
                {
                    __instance.OnEquip();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnExamine();
                }
            }
        }
    }

}

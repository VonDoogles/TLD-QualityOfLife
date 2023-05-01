using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Container ), "Update" )]
    internal class Patch_Panel_Container_Update
    {
        static void Postfix( Panel_Container __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.EquipKey ) || InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnMoveItem();
                }

                if ( InputManager.GetKeyDown( __instance, KeyCode.Tab ) )
                {
                    if ( __instance.m_SelectedTable == SelectedTableEnum.ContainerTable )
                    {
                        __instance.m_SelectedTable = SelectedTableEnum.InventoryTable;
                    }
                    else
                    {
                        __instance.m_SelectedTable = SelectedTableEnum.ContainerTable;
                    }

                    __instance.RefreshTables();
                }
            }
        }
    }
}

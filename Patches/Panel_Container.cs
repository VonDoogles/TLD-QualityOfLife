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
                if ( Input.GetKeyDown( Settings.Instance.EquipKey ) || Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnMoveItem();
                }

                if ( Input.GetKeyDown( KeyCode.Tab ) )
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

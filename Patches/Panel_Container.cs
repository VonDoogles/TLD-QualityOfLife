using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( Panel_Container ), "ItemPassesFilter" )]
	internal class Patch_Panel_Container_ItemPassesFilter
    {
        static bool Prefix( Panel_Container __instance, GearItem pi, string filterName, ref bool __result )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				if ( pi != null && pi.GetComponent<Container>() != null )
				{
					__result = false;
					return false;
				}
			}
            return true;
		}
	}

    [HarmonyPatch( typeof( Panel_Container ), "Update" )]
    internal class Patch_Panel_Container_Update
    {
        static void Postfix( Panel_Container __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.EquipKey ) || ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnMoveItem();
                }

                if ( ModInput.GetKeyDown( __instance, KeyCode.Tab ) )
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

				ContainerUI UIComp = __instance.m_ContainerUIComponent;
				if ( UIComp != null )
				{
                    Vector3 MouseLoc = Input.mousePosition;

					UIWidget ContainerWidget = UIComp.m_ContainerGridBackground.GetComponent<UIWidget>();
					UIWidget InventoryWidget = UIComp.m_InventoryGridBackground.GetComponent<UIWidget>();

					if ( ContainerWidget != null && WidgetUtils.UIRectContains( ContainerWidget, MouseLoc ) )
					{
						if ( __instance.m_SelectedTable != SelectedTableEnum.ContainerTable )
						{
							__instance.m_SelectedTable = SelectedTableEnum.ContainerTable;
						}
					}
					else if ( InventoryWidget != null && WidgetUtils.UIRectContains( InventoryWidget, MouseLoc ) )
					{
						if ( __instance.m_SelectedTable != SelectedTableEnum.InventoryTable )
						{
							__instance.m_SelectedTable = SelectedTableEnum.InventoryTable;
						}
					}
				}
            }
        }
    }
}

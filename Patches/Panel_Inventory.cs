using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.BigCarry;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Inventory ), "OnEquip" )]
	internal class Patch_Panel_Inventory_OnEquip
	{
		static bool Prefix( Panel_Inventory __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				GearItem SelectedItem = __instance.GetCurrentlySelectedGearItem();
				if ( SelectedItem != null && SelectedItem.m_Travois != null )
				{
					TravoisBigCarryItem? BigCarryItem = SelectedItem.m_Travois.BigCarryItem;
					if ( BigCarryItem == null )
					{
						GameObject Prefab = SelectedItem.m_Travois.m_TravoisReference.GetOrLoadAsset();
						BigCarryItem = Prefab?.GetComponent<TravoisBigCarryItem>();
					}

					Container? GearContainer = GearHelper.FindOrCreateGearContainer( SelectedItem, BigCarryItem?.m_Container );
					if ( GearContainer != null )
					{
						Panel_Container PanelContainer = InterfaceManager.GetPanel<Panel_Container>();
						if ( PanelContainer != null )
						{
							PanelContainer.SetContainer( GearContainer, SelectedItem.GetDisplayNameWithoutConditionForInventoryInterfaces() );
							PanelContainer.Enable( true, true, (Il2CppSystem.Action)OnContainerPanelClosed );
							PanelContainer.DeselectAllItems();
							__instance.Enable( false );
						}
						return false;
					}
				}
			}
			return true;
		}

        static void OnContainerPanelClosed()
        {
            InterfaceManager.TrySetPanelEnabled<Panel_Inventory>( true );
        }
	}

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

using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Inventory_Examine ), "HarvestSuccessful" )]
    internal class Patch_Panel_Inventory_Examine_HarvestSuccessful
    {
        static bool Prefix( Panel_Inventory_Examine __instance )
		{
			TravoisUtil.NotifyHarvested( __instance.m_GearItem );
			return true;
		}
    }

	[HarmonyPatch( typeof( Panel_Inventory_Examine ), "OnRefuel" )]
	internal class Patch_Panel_Inventory_Examine_OnRefuel
	{
		static bool Prefix( Panel_Inventory_Examine __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.FuelSelectSource )
			{
				RefuelPanel? Refuel = __instance.m_RefuelPanel.GetComponent<RefuelPanel>();
				if ( Refuel != null )
				{
					Refuel.OnRefuel();
					return false;
				}
			}
			return true;
		}
	}

	[HarmonyPatch( typeof( Panel_Inventory_Examine ), "RefreshMainWindow" )]
	internal class Patch_Panel_Inventory_Examine_RefreshMainWindow
	{
        static bool Prefix( Panel_Inventory_Examine __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.FuelSelectSource )
			{
				if ( IsKerosene( __instance.m_GearItem ) )
				{
					__instance.RefreshHarvest();
					__instance.RefreshCleanPanel();
					__instance.RefreshRepairPanel();
					__instance.RefreshSharpenPanel();
					__instance.RefreshRefuelPanel();
					__instance.RefreshReadPanel();
					__instance.RefreshReadPanel();
					__instance.RefreshButton();
					__instance.UpdateWeightAndConditionLabels();

					__instance.m_Button_Refuel.transform.localPosition = Vector3.zero;
					__instance.OnSelectRefuelButton();
					WidgetUtils.SetActive( __instance.m_Button_Refuel, true );
					WidgetUtils.SetActive( __instance.m_Button_Unload, false );
					WidgetUtils.SetActive( __instance.m_RequiresFuelMessage, false );
					WidgetUtils.SetLabelText( __instance.m_Item_Label, __instance.m_GearItem.GetBasicDisplayNameForInventoryInterfaces() );
					return false;
				}
			}
			return true;
		}

		static bool IsKerosene( GearItem Gear )
		{
			if ( Gear != null && Gear.m_LiquidItem != null && Gear.m_LiquidItem.LiquidType == LiquidType.GetKerosene() )
			{
				return true;
			}
			return false;
		}
	}

	[HarmonyPatch( typeof( Panel_Inventory_Examine ), "RefreshRefuelPanel" )]
	internal class Patch_Panel_Inventory_Examine_RefreshRefuelPanel
	{
        static bool Prefix( Panel_Inventory_Examine __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.FuelSelectSource )
			{
				RefuelPanel? Refuel = __instance.m_RefuelPanel.GetComponent<RefuelPanel>();
				if ( Refuel == null )
				{
					Refuel = __instance.m_RefuelPanel.AddComponent<RefuelPanel>();
				}
			}
			return true;
		}
	}

    [HarmonyPatch( typeof( Panel_Inventory_Examine ), "Update" )]
    internal class Patch_Panel_Inventory_Examine_Update
    {
        static Color ColorParialRepair = new Color( 0.904f, 0.698f, 0.306f, 1.0f );

        static void Postfix( Panel_Inventory_Examine __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( __instance.IsRepairing() )
                {
                    if ( ModInput.GetKeyDown( __instance, KeyCode.Escape ) )
                    {
                        __instance.OnProgressBarCancel();
                    }
                }

				RefuelPanel? Refuel = __instance.m_RefuelPanel.GetComponent<RefuelPanel>();
				bool bIsRefueling = Refuel != null ? Refuel.IsRefueling : false;

                bool bIsDoingAction = bIsRefueling || __instance.IsCleaning() || __instance.IsHarvesting() || __instance.IsReading() || __instance.IsRepairing() || __instance.IsSharpening();
                if ( !bIsDoingAction )
                {
                    if ( __instance.m_ReadPanel.active )
                    {
                        float Scroll = InputManager.GetScroll( __instance );
                        if ( Scroll < 0 || ModInput.GetKeyDown( __instance, KeyCode.A ) )
                        {
                            __instance.OnReadHoursDecrease();
                        }
                        else if ( Scroll > 0 || ModInput.GetKeyDown( __instance, KeyCode.D ) )
                        {
                            __instance.OnReadHoursIncrease();
                        }
                        else if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                        {
                            __instance.OnRead();
                        }
                    }
                    else if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                    {
                        if ( __instance.GetActionToolSelect().active )
                        {
                            __instance.OnSelectActionTool();
                        }
                        else
                        {
                            __instance.m_ButtonDelegates[ __instance.m_SelectedButtonIndex ].Invoke();
                        }
                    }
                }
            }

            if ( __instance.m_RepairPanel.active )
            {
                if ( Settings.Instance.EnableMod && Settings.Instance.RepairColoredAmount && __instance.m_GearItem != null && __instance.m_GearItem.m_Repairable != null )
                {
                    float FullAmount = __instance.m_GearItem.m_Repairable.m_ConditionIncrease;
                    float MissingAmount = 100 - __instance.m_GearItem.GetRoundedCondition();
                    float RepairAmount = Math.Min( FullAmount, MissingAmount );

                    __instance.m_Repair_AmountLabel.color = ( FullAmount != RepairAmount ) ? ColorParialRepair : __instance.m_RepairLabelColorNormal;
					__instance.m_Repair_AmountLabel.text = $"{RepairAmount}% ({FullAmount})";
                }
                else if ( __instance.m_Repair_AmountLabel.color != __instance.m_RepairLabelColorNormal )
                {
                    __instance.m_Repair_AmountLabel.color = __instance.m_RepairLabelColorNormal;
                }
            }
        }
    }

}

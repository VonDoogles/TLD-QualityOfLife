using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Inventory_Examine ), "Update" )]
    internal class Patch_Panel_Inventory_Examine_Update
    {
        static Color ColorParialRepair = new Color( 0.904f, 0.698f, 0.306f, 1.0f );

        static void Postfix( Panel_Inventory_Examine __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( __instance.IsRepairing() )
                {
                    if ( InputManager.GetKeyDown( __instance, KeyCode.Escape ) )
                    {
                        __instance.OnProgressBarCancel();
                    }
                }

                bool bIsDoingAction = __instance.IsCleaning() || __instance.IsHarvesting() || __instance.IsReading() || __instance.IsRepairing() || __instance.IsSharpening();
                if ( !bIsDoingAction )
                {
                    if ( __instance.m_ReadPanel.active )
                    {
                        float Scroll = InputManager.GetScroll( __instance );
                        if ( Scroll < 0 || InputManager.GetKeyDown( __instance, KeyCode.A ) )
                        {
                            __instance.OnReadHoursDecrease();
                        }
                        else if ( Scroll > 0 || InputManager.GetKeyDown( __instance, KeyCode.D ) )
                        {
                            __instance.OnReadHoursIncrease();
                        }
                        else if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                        {
                            __instance.OnRead();
                        }
                    }
                    else if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                    {
                        if ( __instance.m_ActionToolSelect.active )
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
                if ( Settings.Instance.RepairColoredAmount && __instance.m_GearItem != null && __instance.m_GearItem.m_Repairable != null )
                {
                    float FullAmount = __instance.m_GearItem.m_Repairable.m_ConditionIncrease;
                    float RepairAmount = __instance.GetConditionIncreaseFromRepair( __instance.m_GearItem );

                    __instance.m_Repair_AmountLabel.color = ( FullAmount != RepairAmount ) ? ColorParialRepair : __instance.m_RepairLabelColorNormal;
                }
                else if ( __instance.m_Repair_AmountLabel.color != __instance.m_RepairLabelColorNormal )
                {
                    __instance.m_Repair_AmountLabel.color = __instance.m_RepairLabelColorNormal;
                }
            }
        }
    }

}

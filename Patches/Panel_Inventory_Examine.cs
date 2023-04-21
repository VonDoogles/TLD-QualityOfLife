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
                    if ( Input.GetKeyDown( KeyCode.Escape ) )
                    {
                        __instance.OnProgressBarCancel();
                    }
                }

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
                    else if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
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

            if ( __instance.m_RepairPanel.active && Settings.Instance.RepairColoredAmount )
            {
                if ( __instance.m_GearItem != null && __instance.m_GearItem.m_Repairable != null )
                {
                    int FullAmount = (int)__instance.m_GearItem.m_Repairable.m_ConditionIncrease;
                    string FullAmountStr = string.Format( "{0}%", FullAmount );

                    bool bDifferentAmounts = FullAmountStr != __instance.m_Repair_AmountLabel.text;
                    if ( bDifferentAmounts )
                    {
                        __instance.m_Repair_AmountLabel.color = ColorParialRepair;
                    }
                    else
                    {
                        __instance.m_Repair_AmountLabel.color = __instance.m_RepairLabelColorNormal;
                    }
                }
            }
        }
    }

}

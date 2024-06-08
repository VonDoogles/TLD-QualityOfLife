using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( Panel_Clothing ), "Enable" )]
	internal class Patch_Panel_Clothing_Enable
    {
        static void Postfix( Panel_Clothing __instance )
		{
			if ( Settings.Instance.EnableMod )
			{
				foreach ( ClothingSlot Slot in __instance.m_ClothingSlots )
				{
                    UIProgressBar? ConditionFill = Slot.transform.FindChild( "ConditionFill" )?.GetComponent<UIProgressBar>();
                    if ( ConditionFill == null && Settings.Instance.ClothingConditionBars )
                    {
                        ConditionFill = WidgetUtils.MakeProgressBar( "ConditionFill", Slot.transform, 60 );
                    }

                    if ( ConditionFill != null )
                    {
                        ConditionFill.transform.localPosition = new Vector3( 8, -30, 0 );
						ConditionFill.mFG.transform.localPosition = new Vector3( -38, 0, 0 );

						if ( Slot.m_GearItem != null && Settings.Instance.ClothingConditionBars )
						{
							Color FillColor = Slot.m_GearItem.GetColorBasedOnCondition();
							FillColor.a = 0.65f;
							ConditionFill.mFG.color = FillColor;
							ConditionFill.value = Slot.m_GearItem.GetNormalizedCondition();
							WidgetUtils.SetActive( ConditionFill, true );
						}
						else
						{
							WidgetUtils.SetActive( ConditionFill, false );
						}
                    }
				}
			}
		}
	}
	
    [HarmonyPatch( typeof( Panel_Clothing ), "Update" )]
    internal class Patch_Panel_Clothing_Update
    {
        static void Postfix( Panel_Clothing __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.DropKey ) )
                {
                    __instance.OnDropItem();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.EquipKey ) )
                {
                    __instance.OnUseClothingItem();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnActionsButton();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll < 0 )
                {
                    __instance.NextTool();
                }
                else if ( Scroll > 0 )
                {
                    __instance.PrevTool();
                }
            }
        }
    }
}

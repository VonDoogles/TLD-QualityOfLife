using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( Panel_IceFishing ), "Enable", new Type[] { typeof( bool ) } )]
	internal class Patch_Panel_IceFishing_Enable
	{
		internal static string? BaitName;
		internal static string? LureName;
		internal static Panel_IceFishing.IceFishingAction? LastAction;

		internal static Vector3 ConditionOffset = new Vector3( -236.7f, -110.0f, 0.0f );


        static void Postfix( Panel_IceFishing __instance, bool enable )
		{
            if ( Settings.Instance.EnableMod )
			{
				if ( Settings.Instance.FishingShowTipupCondition )
				{
					Transform? Parent = __instance.m_TipupFishingParent?.transform;
					if ( Parent != null )
					{
						Transform? Condition = Parent.FindChild( "Condition" );

						if ( enable && Condition == null )
						{
							Panel_Inventory InvPanel = InterfaceManager.GetPanel<Panel_Inventory>();
							InventoryGridItem? ItemPrefab = InvPanel?.InventoryItemPrefab?.GetComponent<InventoryGridItem>();

							if ( ItemPrefab != null )
							{
								Condition = GameObject.Instantiate( ItemPrefab.m_ConditionLabel, Parent )?.transform;
								if ( Condition != null )
								{
									Condition.gameObject.name = "Condition";
									Condition.localPosition = ConditionOffset;
									Condition.localScale = Vector3.one;
								}
							}
						}

						WidgetUtils.SetActive( Condition, enable );

						UILabel? ConditionLabel = Condition?.GetComponent<UILabel>();
						if ( ConditionLabel != null && __instance.m_BestTipup != null )
						{
							int ConditionValue = __instance.m_BestTipup.GetRoundedCondition();
							ConditionLabel.color = __instance.m_BestTipup.GetColorBasedOnCondition();
							ConditionLabel.text = $"{ConditionValue}%";
							ConditionLabel.fontSize = 14;
						}
					}
				}

				if ( Settings.Instance.FishingRememberSelection && enable )
				{
					__instance.m_BaitSelection.m_SelectedIndex = Math.Max( 0, __instance.m_BaitSelection.m_Items.FindIndex( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == BaitName ) ) );
					__instance.m_BaitSelection.UpdateSelection();

					__instance.m_LureSelection.m_SelectedIndex = Math.Max( 0, __instance.m_LureSelection.m_Items.FindIndex( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == LureName ) ) );
					__instance.m_LureSelection.UpdateSelection();

					if ( LastAction == Panel_IceFishing.IceFishingAction.TipupFishing )
					{
						__instance.OnShowPlaceTipup();
					}
					else
					{
						__instance.OnShowActiveFishing();
					}
				}
			}
		}
	}

    [HarmonyPatch( typeof( Panel_IceFishing ), "OnFish" )]
	internal class Patch_Panel_IceFishing_OnFish
	{
        static void Postfix( Panel_IceFishing __instance )
		{
			Patch_Panel_IceFishing_Enable.BaitName = __instance.m_BaitSelection.SelectedItem?.name;
			Patch_Panel_IceFishing_Enable.LureName = __instance.m_LureSelection.SelectedItem?.name;
			Patch_Panel_IceFishing_Enable.LastAction = __instance.m_Action;
		}
	}

    [HarmonyPatch( typeof( Panel_IceFishing ), "Update" )]
    internal class Patch_Panel_IceFishing_Update
    {
        static void Postfix( Panel_IceFishing __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
				if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
				{
					__instance.OnFish();
				}
				else if ( ModInput.GetKeyDown( __instance, KeyCode.Tab ) )
				{
					if ( __instance.m_Action == Panel_IceFishing.IceFishingAction.ActiveFishing )
					{
						__instance.OnShowPlaceTipup();
					}
					else
					{
						__instance.OnShowActiveFishing();
					}
				}
				else if ( ModInput.GetKeyDown( __instance, KeyCode.W ) ||
						  ModInput.GetKeyDown( __instance, KeyCode.UpArrow ) )
				{
					__instance.m_ActiveRow = __instance.m_LureSelection;
				}
				else if ( ModInput.GetKeyDown( __instance, KeyCode.S ) ||
						  ModInput.GetKeyDown( __instance, KeyCode.DownArrow ) )
				{
					__instance.m_ActiveRow = __instance.m_BaitSelection;
				}
				else if ( ModInput.GetKeyDown( __instance, KeyCode.A ) ||
						  ModInput.GetKeyDown( __instance, KeyCode.LeftArrow ) )
				{
					__instance.m_ActiveRow.DecreaseSelection();
				}
				else if ( ModInput.GetKeyDown( __instance, KeyCode.D ) ||
						  ModInput.GetKeyDown( __instance, KeyCode.RightArrow ) )
				{
					__instance.m_ActiveRow.IncreaseSelection();
				}

				float Scroll = InputManager.GetScroll( __instance );
				if ( Scroll < 0 )
				{
					__instance.OnDecreaseHours();
				}
				else if ( Scroll > 0 )
				{
					__instance.OnIncreaseHours();
				}

				if ( Settings.Instance.EnableMod && Settings.Instance.HighlightSelection )
				{
					WidgetUtils.SetActive( __instance.m_BaitSelection.m_SelectedBackground, __instance.m_ActiveRow == __instance.m_BaitSelection );
					WidgetUtils.SetActive( __instance.m_LureSelection.m_SelectedBackground, __instance.m_ActiveRow == __instance.m_LureSelection );
				}
            }
        }
    }
}

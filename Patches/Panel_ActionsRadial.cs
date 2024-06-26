﻿using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppTLD.Gear;
using UnityEngine;

namespace QualityOfLife
{
	static public class Panel_ActionsRadial_Data
	{
		static public List<string> m_NavigationRadialOrderDefault = new();
		static public List<string> m_NavigationRadialOrderTravois = new();
	}

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "Initialize" )]
    internal class Patch_Panel_ActionsRadial_Initialize
    {
        static void Postfix( Panel_ActionsRadial __instance )
        {
            if ( Settings.Instance.EnableMod && __instance.m_SegmentLabel != null )
            {
                Vector3 Offset = new Vector3( 0, 0, 0 );
                Offset.x = __instance.m_SegmentLabel.width * 0.5f + 16;

                UISprite? PrevItem = WidgetUtils.MakeSprite( "PrevItem", __instance.m_SegmentLabel.transform, -Offset, "arrow_nav3" );
                UISprite? NextItem = WidgetUtils.MakeSprite( "NextItem", __instance.m_SegmentLabel.transform,  Offset, "arrow_nav3" );

				Transform LineBreak = __instance.m_GearStatsObject.transform.FindChild( "Linebreak" );
                UISprite? LineBreakSprite = LineBreak.GetComponent<UISprite>();
				UISprite? Thumb = WidgetUtils.MakeSprite( "Thumb", LineBreak, Vector3.zero, "scrollbar_thumb" );

                if ( Thumb != null )
                {
                    Thumb.depth = LineBreakSprite.depth + 1;
                    Thumb.transform.localScale = Vector3.one;
                    Thumb.SetDimensions( 4, 8 );
                }

                if ( PrevItem != null )
                {
                    PrevItem.transform.localScale = new Vector3( -1, 1, 1 );
                    PrevItem.SetDimensions( 32, 32 );
                }

                if ( NextItem != null )
                {
                    NextItem.transform.localScale = Vector3.one;
                    NextItem.SetDimensions( 32, 32 );
                }
            }

            Panel_ActionsRadial_Data.m_NavigationRadialOrderDefault = new( __instance.m_NavigationRadialOrder );

            Panel_ActionsRadial_Data.m_NavigationRadialOrderTravois = new( __instance.m_NavigationRadialOrder )
            {
                "GEAR_Travois"
            };
        }
    }

	[HarmonyPatch( typeof( Panel_ActionsRadial ), "Enable", new Type[] { typeof( bool ), typeof( bool ) } )]
	internal class Patch_Panel_ActionsRadial_Enable
    {
		static bool Prefix( Panel_ActionsRadial __instance, bool enable, bool doHoverDisable )
		{
			if ( enable )
			{
				if ( Settings.Instance.EnableMod && Settings.Instance.TravoisShowInRadial )
				{
					__instance.m_NavigationRadialOrder = Panel_ActionsRadial_Data.m_NavigationRadialOrderTravois.ToArray();
				}
				else
				{
					__instance.m_NavigationRadialOrder = Panel_ActionsRadial_Data.m_NavigationRadialOrderDefault.ToArray();
				}
			}
			return true;
		}
	}

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "GetDrinkItemsInInventory" )]
    internal class Patch_Panel_ActionsRadial_GetDrinkItemsInInventory
    {
        static void Postfix( Panel_ActionsRadial __instance, ref Il2CppSystem.Collections.Generic.List<GearItem> __result )
        {
			if ( Settings.Instance.EnableMod && Settings.Instance.RadialShowInsulatedFlaskContents )
			{
				Inventory Inv = GameManager.GetInventoryComponent();
				if ( Inv != null )
				{
					foreach ( GearItemObject GearObj in Inv.m_Items )
					{
						if ( GearObj != null && GearObj.m_GearItem != null && GearObj.m_GearItem.m_InsulatedFlask != null )
						{
							foreach ( GearItemObject ContentObj in GearObj.m_GearItem.m_InsulatedFlask.m_Items )
							{
								if ( ContentObj != null && ContentObj.m_GearItem != null && ContentObj.m_GearItem.m_FoodItem != null && ContentObj.m_GearItem.m_FoodItem.m_IsDrink )
								{
									__result.Add( ContentObj.m_GearItem );
								}
							}
						}
					}
				}
			}

            if ( Settings.Instance.EnableMod && Settings.Instance.RadialCombineItems )
            {
                GearHelper.GroupItemsByType( __result );
            }
        }
    }

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "GetFirstAidItemsInInventory" )]
    internal class Patch_Panel_ActionsRadial_GetFirstAidItemsInInventory
    {
        static void Postfix( Panel_ActionsRadial __instance, ref Il2CppSystem.Collections.Generic.List<GearItem> __result )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.RadialCombineItems )
            {
                GearHelper.GroupItemsByType( __result );
            }
        }
    }

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "GetFoodItemsInInventory" )]
    internal class Patch_Panel_ActionsRadial_GetFoodItemsInInventory
    {
        static void Postfix( Panel_ActionsRadial __instance, ref Il2CppSystem.Collections.Generic.List<GearItem> __result )
        {
            if ( Settings.Instance.EnableMod && ( Settings.Instance.RadialShowRuinedFood || Settings.Instance.RadialShowInsulatedFlaskContents ) )
            {
                Inventory Inv = GameManager.GetInventoryComponent();
                if ( Inv != null )
                {
                    foreach ( GearItemObject GearObj in Inv.m_Items )
                    {
                        if ( GearObj != null && GearObj.m_GearItem != null )
                        {
							if ( Settings.Instance.RadialShowRuinedFood && GearObj.m_GearItem.m_FoodItem != null )
							{
								if ( GearObj.m_GearItem.CurrentHP <= 0.0f && !__result.Contains( GearObj.m_GearItem ) )
								{
									__result.Add( GearObj.m_GearItem );
								}
							}

							if ( Settings.Instance.RadialShowInsulatedFlaskContents && GearObj.m_GearItem.m_InsulatedFlask != null )
							{
								foreach ( GearItemObject ContentObj in GearObj.m_GearItem.m_InsulatedFlask.m_Items )
								{
									if ( ContentObj != null && ContentObj.m_GearItem != null && ContentObj.m_GearItem.m_FoodItem != null && !ContentObj.m_GearItem.m_FoodItem.m_IsDrink )
									{
										__result.Add( ContentObj.m_GearItem );
									}
								}
							}
                        }
                    }
                }
            }

            if ( Settings.Instance.EnableMod && Settings.Instance.RadialCombineItems )
            {
                GearHelper.GroupItemsByType( __result );
            }
        }
    }

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "GetGearItemsForRadial" )]
    internal class Patch_Panel_ActionsRadial_GetGearItemsForRadial
    {
        static void Postfix( Panel_ActionsRadial __instance, Il2CppStringArray itemOrder, ref int items, ref Il2CppSystem.Collections.Generic.List<GearItem> __result )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.TorchUseLowest )
            {
                for ( int Index = 0; Index < __result.Count; ++Index )
                {
                    GearItem Item = __result[ Index ];
                    if ( Item != null && ( Item.m_TorchItem != null || Item.m_BowItem != null ) )
                    {
                        Inventory Inv = GameManager.GetInventoryComponent();
                        if ( Inv != null )
                        {
                            GearItem NewItem = Inv.GetLowestConditionGearThatMatchesName( Item.name );
                            if ( NewItem != null )
                            {
                                __result[ Index ] = NewItem;
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "ShowPlaceItemRadial" )]
    internal class Patch_Patch_ActionsRadial_ShowPlaceItemRadial
    {
        static void Postfix( Panel_ActionsRadial __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.RadialCombineItems )
            {
                for ( int Index = 0; Index < __instance.m_RadialArms.Count; ++Index )
                {
                    RadialMenuArm Arm = __instance.m_RadialArms[ Index ];
                    if ( Arm != null && Arm.name == "RadialArmS" && Arm.GetGearItem() == null )
                    {
                        GearItem Skillet = GameManager.GetInventoryComponent().GetLowestConditionGearThatMatchesName( "GEAR_Skillet" );
                        if ( Skillet != null )
                        {
                            IntPtr MethodPtr = IL2CPP.GetIl2CppMethod( Il2CppClassPointerStore<Panel_ActionsRadial>.NativeClassPtr, false, "UseItem", "void", new string[ 0 ] );
                            Arm.SetRadialInfoGear( new Panel_ActionsRadial.RadialGearDelegate( __instance, MethodPtr ), Skillet );
                            Arm.SetEnabled( true );
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "Update" )]
    internal class Patch_Panel_ActionsRadial_Update
    {
        static void Postfix( Panel_ActionsRadial __instance )
        {
            Transform PrevItem = __instance.m_SegmentLabel.transform.FindChild( "PrevItem" );
            Transform NextItem = __instance.m_SegmentLabel.transform.FindChild( "NextItem" );
            UISprite? LineBreak = __instance.m_GearStatsObject.transform.FindChild( "Linebreak" ).GetComponent<UISprite>();
            UISprite? Thumb = LineBreak?.transform.FindChild( "Thumb" ).GetComponent<UISprite>();

            bool bShowItemIcons = false;

            if ( Settings.Instance.EnableMod && Settings.Instance.RadialCombineItems )
            {
                RadialMenuArm? HoveredArm = __instance.m_RadialArms.FirstOrDefault( Arm => Arm.IsHoveredOver() );
                if ( HoveredArm != null )
                {
                    GearItem Item = HoveredArm.GetGearItem();
                    if ( Item != null )
                    {
                        Il2CppSystem.Collections.Generic.List<GearItem> ItemList = new();

                        if ( Item.name == "GEAR_CookingPot" || Item.name == "GEAR_Skillet" )
                        {
                            foreach ( GearItemObject GearObj in GameManager.GetInventoryComponent().m_Items )
                            {
                                if ( GearObj != null && GearObj.m_GearItem != null && ( GearObj.m_GearItemName == "GEAR_CookingPot" || GearObj.m_GearItemName == "GEAR_Skillet" ) )
                                {
                                    ItemList.Add( GearObj.m_GearItem );
                                }
                            }
                        }
                        else if ( Item.name == "GEAR_BedRoll" || Item.name == "GEAR_BedRoll_Down" || Item.name == "GEAR_BearSkinBedRoll" )
                        {
                            foreach ( GearItemObject GearObj in GameManager.GetInventoryComponent().m_Items )
                            {
                                if ( GearObj != null && GearObj.m_GearItem != null && GearObj.m_GearItem.m_Bed != null )
                                {
                                    ItemList.Add( GearObj.m_GearItem );
                                }
                            }
                        }
                        else
                        {
                            GameManager.GetInventoryComponent().GetItems( Item.name, ItemList );

							if ( Settings.Instance.RadialShowInsulatedFlaskContents )
							{
								foreach ( GearItemObject GearObj in GameManager.GetInventoryComponent().m_Items )
								{
									if ( GearObj != null && GearObj.m_GearItem != null && GearObj.m_GearItem.m_InsulatedFlask != null )
									{
										foreach ( GearItemObject ContentObj in GearObj.m_GearItem.m_InsulatedFlask.m_Items )
										{
											if ( ContentObj != null && ContentObj.m_GearItem != null && ContentObj.m_GearItem.name == Item.name )
											{
												ItemList.Add( ContentObj.m_GearItem );
											}
										}
									}
								}
							}
                        }

                        GearHelper.Sort( ItemList, GearHelper.CompareGearItemByHeatAndHP );

                        bool bMultipleItems = ItemList.Count > 1;
                        if ( bMultipleItems )
                        {
                            int Index = ItemList.IndexOf( Item );
                            float Scroll = InputManager.GetScroll( __instance );

                            if ( Scroll > 0 )
                            {
                                Index = Math.Max( 0, Index - 1 );
                            }
                            else if ( Scroll < 0 )
                            {
                                Index = Math.Min( Index + 1, ItemList.Count - 1 );
                            }
							else
							{
								Index = Math.Clamp( Index, 0, ItemList.Count - 1 );
							}

                            GearItem NewItem = ItemList[ Index ];
                            if ( NewItem != Item )
                            {
                                HoveredArm.m_GearItem = NewItem;
                                HoveredArm.SetRadialInfoGear( HoveredArm.m_GearCallback, NewItem );
                            }

                            if ( LineBreak != null && Thumb != null )
                            {
                                float StepSize = LineBreak.height / (float)( ItemList.Count - 1 );
                                float ThumbX = StepSize * Index - LineBreak.height * 0.5f;
                                Thumb.transform.localPosition = new Vector3( 0, -ThumbX, 0 );
                                WidgetUtils.SetActive( Thumb, true );
                            }
                        }

                        bShowItemIcons = bMultipleItems;
                    }
                }
            }

            if ( bShowItemIcons )
            {
                if ( __instance.m_SegmentLabel != null )
                {
                    Vector3 Offset = new Vector3( 0, 0, 0 );
                    Offset.x = __instance.m_SegmentLabel.width * 0.5f + 16;

                    if ( PrevItem != null )
                    {
                        PrevItem.localPosition = -Offset;
                    }

                    if ( NextItem != null )
                    {
                        NextItem.localPosition = Offset;
                    }
                }
            }

            WidgetUtils.SetActive( PrevItem, bShowItemIcons );
            WidgetUtils.SetActive( NextItem, bShowItemIcons );
            WidgetUtils.SetActive( Thumb, bShowItemIcons );
        }
    }

	[HarmonyPatch( typeof( Panel_ActionsRadial ), "UseItem" )]
	 internal class Patch_Panel_ActionsRadial_UseItem
	 {
		 static bool Prefix( Panel_ActionsRadial __instance, GearItem gi )
		 {
			 if ( Settings.Instance.EnableMod && Settings.Instance.TravoisShowInRadial )
			 {
				 if ( gi != null && gi.m_Travois != null )
				 {
					 TravoisUtil.DropTravois( gi );
				 }
			 }
			 return true;
		 }
	}

}

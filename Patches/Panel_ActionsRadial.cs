using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppTLD.Gear;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "Initialize" )]
    internal class Patch_Panel_ActionsRadial_Initialize
    {
        static void Postfix( Panel_ActionsRadial __instance )
        {
            if ( __instance.m_SegmentLabel != null )
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
        }
    }

    [HarmonyPatch( typeof( Panel_ActionsRadial ), "GetDrinkItemsInInventory" )]
    internal class Patch_Panel_ActionsRadial_GetDrinkItemsInInventory
    {
        static void Postfix( Panel_ActionsRadial __instance, ref Il2CppSystem.Collections.Generic.List<GearItem> __result )
        {
            if ( Settings.Instance.RadialCombineItems )
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
            if ( Settings.Instance.RadialCombineItems )
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
            if ( Settings.Instance.RadialShowRuinedFood )
            {
                Inventory Inv = GameManager.GetInventoryComponent();
                if ( Inv != null )
                {
                    foreach ( GearItemObject GearObj in Inv.m_Items )
                    {
                        if ( GearObj != null && GearObj.m_GearItem != null && GearObj.m_GearItem.m_FoodItem != null )
                        {
                            if ( GearObj.m_GearItem.CurrentHP <= 0.0f && !__result.Contains( GearObj.m_GearItem ) )
                            {
                                __result.Add( GearObj.m_GearItem );
                            }
                        }
                    }
                }
            }

            if ( Settings.Instance.RadialCombineItems )
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
            if ( Settings.Instance.TorchUseLowest )
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

            if ( Settings.Instance.RadialCombineItems )
            {
                RadialMenuArm? HoveredArm = __instance.m_RadialArms.FirstOrDefault( Arm => Arm.IsHoveredOver() );
                if ( HoveredArm != null )
                {
                    GearItem Item = HoveredArm.GetGearItem();
                    if ( Item != null )
                    {
                        Il2CppSystem.Collections.Generic.List<GearItem> ItemList = new();
                        GameManager.GetInventoryComponent().GetItems( Item.name, ItemList );
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

                            GearItem NewItem = ItemList[ Index ];
                            if ( NewItem != Item )
                            {
                                HoveredArm.m_GearItem = NewItem;
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

}

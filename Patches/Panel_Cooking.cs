using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Cooking;
using Il2CppTLD.Gear;
using UnityEngine;

namespace QualityOfLife
{
#if false
    [HarmonyPatch( typeof( Panel_Cooking ), "RefreshFoodList" )]
    internal class Patch_Panel_Cooking_RefreshFoodList
    {
        static void Postfix( Panel_Cooking __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.FoodCookFilterReheat )
            {
                Inventory Inv = GameManager.GetInventoryComponent();
                if ( Inv != null )
                {
                    __instance.m_FoodList.Clear();

                    foreach ( GearItemObject GearObj in Inv.m_Items )
                    {
                        GearItem? Item = GearObj != null ? GearObj.m_GearItem : null;
                        if ( Item != null && Item.m_Cookable != null )
                        {
                            if ( ( Item.m_Cookable.m_CookableType == Cookable.CookableType.Meat ) ||
                                 ( Item.m_Cookable.m_CookableType == Cookable.CookableType.Grub ) ||
                                 ( Item.m_Cookable.m_CookableType == Cookable.CookableType.Liquid && Item.m_Cookable.m_PotableWaterRequiredLiters > 0.0f ) )
                            {
                                if ( __instance.m_CookingPotInteractedWith == null || __instance.m_CookingPotInteractedWith.CanCookItem( Item ) )
                                {
                                    __instance.m_FoodList.Add( Item );
                                }
                            }
                        }
                    }

                    switch ( __instance.m_SortName )
                    {
                        case "GAMEPLAY_SortCondition":  GearHelper.Sort( __instance.m_FoodList, GearHelper.CompareGearItemByCondition );    break;
                        case "GAMEPLAY_SortAlphabetic": GearHelper.Sort( __instance.m_FoodList, GearHelper.CompareGearItemByDisplayName );  break;
                        case "GAMEPLAY_SortWeight":     GearHelper.Sort( __instance.m_FoodList, GearHelper.CompareGearItemByWeight );       break;
                        default:        break;
                    }

                    if ( __instance.m_SortFlipDictionary[ __instance.m_SortName ] )
                    {
                        __instance.m_FoodList.Reverse();
                    }

                    for ( int Index = 0; Index < __instance.m_FoodScrollList.m_ScrollObjects.Count; ++Index )
                    {
                        GameObject GameObj = __instance.m_FoodScrollList.m_ScrollObjects[ Index ];
                        if ( GameObj != null )
                        {
                            CookingItemListEntry Entry = GameObj.GetComponentInChildren<CookingItemListEntry>();
                            if ( Entry != null )
                            {
                                if ( Index < __instance.m_FoodList.Count )
                                {
                                    Entry.SetGearItem( __instance.m_FoodList[ Index ] );
                                }
                                else
                                {
                                    WidgetUtils.SetActive( GameObj, false );
                                }
                            }
                        }
                    }
                }
            }
        }
    }
#endif
}

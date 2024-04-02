using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Cooking;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Cooking ), "RefreshFoodList" )]
    internal class Patch_Panel_Cooking_RefreshFoodList
    {
        static void Postfix( Panel_Cooking __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.FoodCookFilterReheat )
            {
                List<CookableItem> NewList = new List<CookableItem>();

                foreach ( CookableItem Item in __instance.m_FoodList )
                {
                    if ( !ShouldFilterCookableItem( Item ) )
                    {
                        NewList.Add( Item );
                    }
                }

                __instance.m_FoodList.Clear();

                foreach ( CookableItem Item in NewList )
                {
                    __instance.m_FoodList.Add( Item );
                }

                for ( int Index = 0; Index < __instance.m_FoodScrollList.m_ScrollObjects.Count; ++Index )
                {
                    GameObject GameObj = __instance.m_FoodScrollList.m_ScrollObjects[ Index ];
                    if ( GameObj != null )
                    {
                        CookableListItem ListItem = GameObj.GetComponentInChildren<CookableListItem>();
                        if ( ListItem != null )
                        {
                            if ( Index < __instance.m_FoodList.Count )
                            {
                                ListItem.SetCookable( __instance.m_FoodList[ Index ], __instance.m_CookingPotInteractedWith );
                            }
                        }
                    }
                }

                for ( int Index = __instance.m_FoodScrollList.m_ScrollObjects.Count - 1; Index >= __instance.m_FoodList.Count; --Index )
                {
                    GameObject GameObj = __instance.m_FoodScrollList.m_ScrollObjects[ Index ];
                    if ( GameObj != null )
                    {
                        __instance.m_FoodScrollList.OnReleaseChild( GameObj );
                    }
                }

                __instance.m_FoodScrollSlider.numberOfSteps = __instance.m_FoodList.Count - 6;
                WidgetUtils.SetActive( __instance.m_FoodScrollSlider.transform.parent, __instance.m_FoodList.Count > 7 );
            }
        }

        static bool ShouldFilterCookableItem( CookableItem Item )
        {
            if ( Item != null && Item.m_GearItem != null )
            {
                Cookable Cook = Item.m_GearItem.m_Cookable;
                if ( Cook != null && Cook.m_CookableType == Cookable.CookableType.Liquid && Cook.m_PotableWaterRequiredLiters <= 0.0f )
                {
                    return true;
                }
            }
            return false;
        }
    }

    [HarmonyPatch( typeof( Panel_Cooking ), "Update" )]
    internal class Patch_Panel_Cooking_Update
    {
        static void Postfix( Panel_Cooking __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnDoAction();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.DropKey ) )
                {
                    __instance.OnDoActionSecondary();
                }
            }
        }
    }

}

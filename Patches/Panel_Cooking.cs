using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Cooking;
using Il2CppTLD.IntBackedUnit;
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

                __instance.m_ScrollBehaviour?.RebuildItems( __instance.m_FoodList.Count );
            }
        }

        static bool ShouldFilterCookableItem( CookableItem Item )
        {
            if ( Item != null && Item.m_GearItem != null )
            {
                Cookable Cook = Item.m_GearItem.m_Cookable;
                if ( Cook != null && Cook.m_CookableType == Cookable.CookableType.Liquid && Cook.m_PotableWaterRequired <= ItemLiquidVolume.Zero )
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

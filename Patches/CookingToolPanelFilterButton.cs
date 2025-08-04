using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Cooking;
using Il2CppTLD.IntBackedUnit;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( CookingToolPanelFilterButton ), "PassesFilter" )]
    internal class Patch_CookingToolPanelFilterButton
    {
        static void Postfix( CookingToolPanelFilterButton __instance, CookableItem cookableItem, ref bool __result )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.FoodCookFilterReheat )
            {
                if ( ShouldFilterCookableItem( cookableItem ) )
                {
                    __result = false;
                }
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
}

using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace QualityOfLife
{

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
                    if ( Item != null && Item.m_TorchItem != null )
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

}

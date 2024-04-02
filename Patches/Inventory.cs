using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Inventory ), "Deserialize" )]
	internal class Patch_Inventory_Deserialize
	{
		static void Postfix( Inventory __instance, string text )
		{
			if ( Settings.Instance.EnableMod )
			{
				Il2CppSystem.Collections.Generic.List<GearItem> CatTailItems = new();
				__instance.GearInInventory( "GEAR_CattailStalk", CatTailItems );

				foreach ( GearItem Item in CatTailItems )
				{
					if ( Item != null )
					{
						CatTailHelper.TryUpdateCalories( Item.m_FoodItem );
					}
				}
			}
		}
	}
}

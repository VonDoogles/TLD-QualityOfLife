using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Harvestable ), "Harvest" )]
	internal class Patch_Harvestable_Harvest
	{
		static bool Prefix( Harvestable __instance )
		{
			if ( Settings.Instance.EnableMod && __instance.m_GearPrefab != null && __instance.m_GearPrefab.name == "GEAR_CattailStalk" )
			{
				CatTailHelper.TryUpdateCalories( __instance.m_GearPrefab.m_FoodItem );

				if ( !Settings.Instance.CatTailHarvestTinder )
				{
					__instance.m_SecondGearPrefab = null;
				}

				if ( !Settings.Instance.CatTailHarvestStalk )
				{
					__instance.m_GearPrefab = __instance.m_SecondGearPrefab;
					__instance.m_SecondGearPrefab = null;
				}
			}
			return true;
		}
	}
}

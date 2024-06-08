using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( ItemDescriptionPage ), "CanExamine" )]
	internal class Patch_ItemDescriptionPage_CanExamine
	{
		static void Postfix( ItemDescriptionPage __instance, GearItem gi, ref bool __result )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.FuelSelectSource )
			{
				if ( gi != null && gi.m_LiquidItem != null && gi.m_LiquidItem.LiquidType == LiquidType.GetKerosene() )
				{
					__result = true;
				}
			}
		}
	}

    [HarmonyPatch( typeof( ItemDescriptionPage ), "GetEquipButtonLocalizationId" )]
    internal class Patch_ItemDescriptionPage_GetEquipButtonLocalizationId
    {
        static void Postfix( ItemDescriptionPage __instance, GearItem gi, ref string __result )
        {
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				if ( gi != null && gi.m_Travois != null )
				{
					__result = "Transfer";
				}
			}
        }
    }
}


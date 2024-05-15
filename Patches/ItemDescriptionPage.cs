using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
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

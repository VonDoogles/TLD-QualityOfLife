using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( WaterSource ), "PerformInteraction" )]
    internal class Patch_WaterSource_PerformInteraction
    {
		static bool Prefix( WaterSource __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.ToiletNonPotable )
			{
				bool bToilet = __instance.gameObject.name.Contains( "toilet", StringComparison.OrdinalIgnoreCase );
				if ( bToilet )
				{
					__instance.m_CurrentLiquidQuality = LiquidQuality.NonPotable;
				}
			}
			return true;
		}
    }
}

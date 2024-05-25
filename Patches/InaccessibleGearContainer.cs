using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( InaccessibleGearContainer ), "AddGearToContainer" )]
    internal class Patch_InaccessibleGearContainer_AddGearToContainer
    {
		static bool Prefix( InaccessibleGearContainer __instance, GearItem gi )
		{
			if ( TravoisUtil.AnyTravoisContains( gi ) )
			{
				return false;
			}
			return true;
		}
    }
}

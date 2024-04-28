using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife.Patches
{
	[HarmonyPatch( typeof( BodyHarvest ), "TryGetBodyHarvestTimelineData" )]
	internal class Patch_BodyHarvest_TryGetBodyHarvestTimelineData
	{
		static void Postfix( BodyHarvest __instance, Vector3 playerPosition, ref BodyHarvest.BodyHarvestTimelineData data, ref bool __result )
		{
			if ( Settings.Instance.EnableMod && !Settings.Instance.AnimPlayHarvest )
			{
                // Disable CS8625: VS doesn't think type BodyHarvestTimelineData can be set to null, but Il2Cpp doesn't care.
				#pragma warning disable CS8625
                data = null;
				#pragma warning restore CS8625

                __result = false;
			}
		}
    }
}

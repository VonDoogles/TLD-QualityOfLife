using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( SaveGameSystem ), "SaveGlobalData" )]
    internal class Patch_SaveGameSystem_SaveGlobalData
    {
		static bool Prefix( SaveGameSystem __instance, SlotData slot )
		{
			TravoisUtil.MoveItemsToInventory();
			return true;
		}
    }

	[HarmonyPatch( typeof( SaveGameSystem ), "SaveGame" )]
	internal class Patch_SaveGameSystem_SaveGame
	{
		static void Postfix( SaveGameSystem __instance, string name, string sceneSaveName )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				TravoisUtil.MoveItemsToTravois();
			}
		}
	}

	[HarmonyPatch( typeof( SaveGameSystem ), "RestoreGame" )]
	internal class Patch_SaveGameSystem_RestoreGame
	{
		static void Postfix( SaveGameSystem __instance, string name )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				TravoisUtil.MoveItemsToTravois();
			}
		}
	}
}

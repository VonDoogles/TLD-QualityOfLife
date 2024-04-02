using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( GearMessage ), "AddMessageToQueue" )]
    internal class Patch_GearMessage_AddMessageToQueue
    {
        static bool Prefix( GearMessage __instance, Panel_HUD hud, GearMessage.GearMessageInfo newGearMessage, bool highPriority )
		{
			if ( Settings.Instance.EnableMod && newGearMessage.m_HeaderText == "Harvested" )
			{
				if ( !Settings.Instance.CatTailHarvestStalk && newGearMessage.m_GearPrefabName == "GEAR_CattailStalk" )
				{
					return false;
				}

				if ( !Settings.Instance.CatTailHarvestTinder && newGearMessage.m_GearPrefabName == "GEAR_CattailTinder" )
				{
					return false;
				}
			}
			return true;
		}
    }
}

using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
#if DEBUG
    [HarmonyPatch( typeof( GearMessage ), "AddMessageToQueue" )]
    internal class Patch_GearMessage_AddMessageToQueue
    {
        static void Postfix( GearMessage __instance, Panel_HUD hud, GearMessage.GearMessageInfo newGearMessage, bool highPriority )
        {
            return;
        }
    }
#endif
}

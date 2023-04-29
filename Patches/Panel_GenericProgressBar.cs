using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
#if DEBUG
    [HarmonyPatch( typeof( Panel_GenericProgressBar ), "Launch", new Type[] { typeof(string), typeof(float), typeof(float), typeof(float), typeof(bool), typeof(OnExitDelegate) } )]
    internal class Patch_Panel_GenericProgressBar_Launch
    {
        static bool blah = true;

        static bool Prefix( Panel_GenericProgressBar __instance, string name, float seconds, float minutes, float randomFailureThreshold, bool skipRestoreInHands, OnExitDelegate del )
        {
            return blah;
        }

        static void Postfix( Panel_GenericProgressBar __instance )
        {
            return;
        }
    }
#endif
}

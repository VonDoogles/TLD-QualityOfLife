using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( PlayerStruggle ), "Update" )]
    internal class Patch_PlayerStruggle_Update
    {
        static void Postfix( PlayerStruggle __instance )
        {
            if ( Settings.Instance.SeparateInteract && __instance.InStruggleWIthWolf() )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.DoTap();
                }
            }
        }
    }
}

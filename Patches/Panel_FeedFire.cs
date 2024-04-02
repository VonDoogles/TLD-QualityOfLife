using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_FeedFire ), "Update" )]
    internal class Patch_Panel_FeedFire_Update
    {
        static void Postfix( Panel_FeedFire __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnFeedFire();
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_FeedFire ), "OnTakeTorch" )]
    internal class Patch_Panel_FeedFire_OnTakeTorch
    {
        static bool Prefix( Panel_FeedFire __instance )
        {
            if ( Settings.Instance.EnableMod && !Settings.Instance.FireAllowTakeTortch )
            {
                GameAudioManager.PlayGUIError();
                return false;
            }
            return true;
        }
    }
}

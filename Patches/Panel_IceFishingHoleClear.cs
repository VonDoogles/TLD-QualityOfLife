using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_IceFishingHoleClear ), "Update")]
    internal class Patch_Panel_IceFishingHoleClear_Update
    {
        static void Postfix( Panel_IceFishingHoleClear __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBreakIce();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 )
                {
                    __instance.PrevTool();
                }
                else if ( Scroll < 0 )
                {
                    __instance.NextTool();
                }
            }
        }
    }
}

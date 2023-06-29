using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_CookWater ), "Update" )]
    internal class Patch_Panel_CookWater_Update
    {
        static void Postfix( Panel_CookWater __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnDoAction();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll < 0 )
                {
                    __instance.OnWaterDown();
                }
                else if ( Scroll > 0 )
                {
                    __instance.OnWaterUp();
                }
            }
        }
    }
}

using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Cooking ), "Update" )]
    internal class Patch_Panel_Cooking_Update
    {
        static void Postfix( Panel_Cooking __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnDoAction();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.DropKey ) )
                {
                    __instance.OnDoActionSecondary();
                }
            }
        }
    }

}

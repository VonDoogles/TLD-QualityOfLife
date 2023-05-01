using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_SnowShelterBuild ), "Update" )]
    internal class Patch_Panel_SnowShelterBuild_Update
    {
        static void Postfix( Panel_SnowShelterBuild __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBuild();
                }
            }
        }
    }
}

using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_SnowShelterBuild ), "Update" )]
    internal class Patch_Panel_SnowShelterBuild_Update
    {
        static void Postfix( Panel_SnowShelterBuild __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls && !__instance.IsBuilding() )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBuild();
                }
            }
        }
    }
}

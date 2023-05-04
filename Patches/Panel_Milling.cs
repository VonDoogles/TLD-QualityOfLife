using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Milling ), "Update" )]
    internal class Patch_Panel_Milling_Update
    {
        static void Postfix( Panel_Milling __instance )
        {
            if ( Settings.Instance.UIExtraControls && !__instance.m_MillingInProgress )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnRepair();
                }
            }
        }
    }
}

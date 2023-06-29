using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_SnowShelterInteract ), "Update" )]
    internal class Patch_Panel_SnowShelterInteract_Update
    {
        static void Postfix( Panel_SnowShelterInteract __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    int Index = __instance.m_SelectedButtonIndex;
                    if ( Index >= 0 && Index < __instance.m_ButtonDelegates.Count )
                    {
                        __instance.m_ButtonDelegates[ Index ].Invoke();
                    }
                }
            }
        }
    }
}

using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_MainMenu ), "Update" )]
    internal class Patch_Panel_MainMenu_Update
    {
        static void Postfix( Panel_MainMenu __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( __instance.m_SelectFeatWindow != null && __instance.m_SelectFeatWindow.active )
                {
                    if ( InputManager.GetKeyDown( __instance, Settings.Instance.EquipKey ) )
                    {
                        __instance.OnToggleFeatActive();
                    }

                    if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                    {
                        __instance.OnSelectFeatsContinue();
                    }

                    float Scroll = InputManager.GetScroll( __instance );
                    if ( Scroll > 0 )
                    {
                        __instance.FeatSelectionPrev();
                    }
                    else if ( Scroll < 0 )
                    {
                        __instance.FeatSelectionNext();
                    }
                }
            }
        }
    }

}

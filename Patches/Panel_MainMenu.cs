using HarmonyLib;
using Il2Cpp;
using UnityEngine;

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
                    if ( Input.GetKeyDown( Settings.Instance.EquipKey ) )
                    {
                        __instance.OnToggleFeatActive();
                    }

                    if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
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

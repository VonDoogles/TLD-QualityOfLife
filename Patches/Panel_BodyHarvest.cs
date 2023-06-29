using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_BodyHarvest ), "UpdateMenuNavigation" )]
    internal class Patch_Panel_BodyHarvest_UpdateMenuNavigation
    {
        static void Postfix( Panel_BodyHarvest __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.HighlightSelection )
            {
                int SelectedIndex = __instance.m_SelectedButtonIndex;

                for ( int Index = 0; Index < __instance.m_MenuItems.Count; ++Index )
                {
                    GameObject Background = __instance.m_MenuItems[ Index ].m_Background;
                    if ( Background != null )
                    {
                        Background.SetActive( Index == SelectedIndex );
                    }
                }
            }

            if ( Settings.Instance.EnableMod && Settings.Instance.HighlightSelection )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    if ( __instance.IsTabHarvestSelected() )
                    {
                        __instance.OnHarvest();
                    }
                    else
                    {
                        __instance.OnQuarter();
                    }
                }

                if ( ModInput.GetKeyDown( __instance, KeyCode.Tab ) )
                {
                    if ( __instance.IsTabHarvestSelected() )
                    {
                        __instance.OnTabQuarterSelected();
                    }
                    else
                    {
                        __instance.OnTabHarvestSelected();
                    }
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 )
                {
                    __instance.OnToolPrev();
                }
                else if ( Scroll < 0 )
                {
                    __instance.OnToolNext();
                }
            }
        }
    }

}

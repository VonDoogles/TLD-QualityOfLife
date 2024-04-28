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

	[HarmonyPatch( typeof( Panel_BodyHarvest ), "StartQuarter" )]
	internal class Patch_Panel_BodyHarvest_StartQuarter
	{
		static void Postfix( Panel_BodyHarvest __instance, int durationMinutes, string quarterAudio )
		{
			if ( Settings.Instance.EnableMod && !Settings.Instance.AnimPlayHarvest )
			{
				Patch_Panel_BodyHarvest_Update.QuarterDuration = __instance.m_Settings.m_HarvestTimeSeconds;
				Patch_Panel_BodyHarvest_Update.QuarterElapsed = 0.0f;
			}
		}
	}

	[HarmonyPatch( typeof( Panel_BodyHarvest ), "Update" )]
	internal class Patch_Panel_BodyHarvest_Update
	{
		static public float QuarterDuration = 0.0f;
		static public float QuarterElapsed = 0.0f;

		static void Postfix( Panel_BodyHarvest __instance )
		{
			if ( QuarterDuration != 0.0f )
			{
				if ( Settings.Instance.EnableMod && !Settings.Instance.AnimPlayHarvest )
				{
					QuarterElapsed += Time.unscaledDeltaTime;

					float Progress = Mathf.Clamp01( QuarterElapsed / QuarterDuration );
					Panel_HUD Hud = InterfaceManager.GetPanel<Panel_HUD>();
					if ( Hud != null && Hud.m_AccelTimePopup != null && Hud.m_AccelTimePopup.m_Slider != null )
					{
						Hud.m_AccelTimePopup.m_Slider.value = Progress;
					}

					if ( QuarterElapsed >= QuarterDuration || !__instance.IsAcceleratingTime() )
					{
						QuarterDuration = 0.0f;
					}
				}
			}
		}
	}

}

using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    internal class Panel_Log_Utils
    {
        public static void ScrollDailyLogToEnd( Panel_Log __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                UISlider DaySlider = __instance.m_ScrollbarDays.GetComponentInChildren<UISlider>();
                if ( DaySlider != null )
                {
                    DaySlider.value = 1;
                    __instance.OnScrollbarDaysChange();
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_Log ), "Update" )]
    internal class Patch_Panel_Log_Update
    {
        static void Postfix( Panel_Log __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    if ( __instance.m_CollectibleSectionObject.activeSelf )
                    {
                        if ( __instance.m_GridItemParent.activeSelf )
                        {
                            __instance.OnExamineCollectible();
                        }
                        else
                        {
                            __instance.OnCollectionsSubScreen();
                        }
                    }
                }

                if ( __instance.m_CartographySectionObject.activeSelf )
                {
                    Vector3 MouseLoc = Input.mousePosition;

                    var PanelContains = delegate ( UIPanel Panel, Vector3 MouseLoc )
                    {
                        Vector3 Min = Panel.anchorCamera.WorldToScreenPoint( Panel.worldCorners[ 0 ] );
                        Vector3 Max = Min;

                        foreach ( Vector3 Corner in Panel.worldCorners )
                        {
                            Vector3 ScreenCorner = Panel.anchorCamera.WorldToScreenPoint( Corner );
                            Min.x = Math.Min( Min.x, ScreenCorner.x );
                            Min.y = Math.Min( Min.y, ScreenCorner.y );
                            Max.x = Math.Max( Max.x, ScreenCorner.x );
                            Max.y = Math.Max( Max.y, ScreenCorner.y );
                        }

                        Rect PanelRect = Rect.MinMaxRect( Min.x, Min.y, Max.x, Max.y );
                        return PanelRect.Contains( MouseLoc );
                    };

                    UIPanel RegionListPanel = __instance.m_SurveyListRegionScrollbar.transform.parent.GetComponent<UIPanel>();
                    UIPanel SurveyListPanel = __instance.m_SurveyListScrollbar.transform.parent.GetComponent<UIPanel>();

                    if ( SurveyListPanel != null && PanelContains( SurveyListPanel, MouseLoc ) )
                    {
                        if ( __instance.m_SurveyState != PanelLogSurveyStates.AchievementList )
                        {
                            __instance.m_SurveyState = PanelLogSurveyStates.AchievementList;
                        }
                    }
                    else if ( RegionListPanel != null && PanelContains( RegionListPanel, MouseLoc ) )
                    {
                        if ( __instance.m_SurveyState != PanelLogSurveyStates.RegionList )
                        {
                            __instance.m_SurveyState = PanelLogSurveyStates.RegionList;
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_Log ), "UpdateCartographyPage" )]
    internal class Patch_Panel_Log_UpdateCartographyPage
    {
        static void Postfix( Panel_Log __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                string SceneName = GameManager.m_LastOutdoorSceneSet.name;

                int RegionIndex = -1;
                for ( int Index = 0; Index < __instance.m_SurveyRegionList.Count; ++Index )
                {
                    SurveyRegionInfo RegionInfo = __instance.m_SurveyRegionList[ Index ];
                    if ( RegionInfo.m_SceneName == SceneName )
                    {
                        RegionIndex = Index;
                        break;
                    }
                }

                int ClickedIndex = -1;
                for ( int Index = 0; Index < __instance.m_SurveyRegionDisplayList.Count; ++Index )
                {
                    SurveyRegionListItem RegionItem = __instance.m_SurveyRegionDisplayList[ Index ];
                    if ( RegionItem != null && RegionItem.m_Index == RegionIndex )
                    {
                        ClickedIndex = Index;
                        break;
                    }
                }

                if ( ClickedIndex != -1 )
                {
                    __instance.OnSurveyRegionClicked( ClickedIndex );
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_Log ), "BuildDailyList" )]
    internal class Patch_Panel_Log_BuildDailyList
    {
        static bool Prefix( Panel_Log __instance, bool doSelectionReset )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( doSelectionReset )
                {
                    Panel_Log_Utils.ScrollDailyLogToEnd( __instance );
                }
            }
            return true;
        }
    }

    [HarmonyPatch( typeof( Panel_Log ), "Enable" )]
    internal class Patch_Panel_Log_Enable
    {
        static void Postfix( Panel_Log __instance, bool enable )
        {
            if ( Settings.Instance.UIExtraControls && enable )
            {
                Panel_Log_Utils.ScrollDailyLogToEnd( __instance );
            }
        }
    }

    [HarmonyPatch( typeof( Panel_Log ), "OnDailyLogButton" )]
    internal class Patch_Panel_Log_OnDailyLogButton
    {
        static void Postfix( Panel_Log __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                Panel_Log_Utils.ScrollDailyLogToEnd( __instance );
            }
        }
    }
}

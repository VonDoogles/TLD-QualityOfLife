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
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
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

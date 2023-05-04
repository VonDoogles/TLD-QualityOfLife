using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Crafting ), "SetFilter" )]
    internal class Patch_Panel_Crafting_SetFilter
    {
        static public bool ShouldSave = false;
        static public Panel_Crafting.Filter SavedFilter = Panel_Crafting.Filter.All;

        static void Postfix( Panel_Crafting __instance, Panel_Crafting.Filter filter )
        {
            if ( ShouldSave )
            {
                SavedFilter = filter;
            }
        }
    }

    [HarmonyPatch( typeof( Panel_Crafting ), "Enable", new Type[] { typeof( bool ), typeof( bool ) } )]
    internal class Patch_Panel_Crafting_Enable
    {
        static void Postfix( Panel_Crafting __instance, bool enable, bool fromPanel )
        {
            if ( Settings.Instance.CraftRememberFilter )
            {
                if ( enable )
                {
                    __instance.SetFilter( Patch_Panel_Crafting_SetFilter.SavedFilter );
                }
            }
            Patch_Panel_Crafting_SetFilter.ShouldSave = enable;
        }
    }

    [HarmonyPatch( typeof( Panel_Crafting ), "Update" )]
    internal class Patch_Panel_Crafting_Update
    {
        static void Postfix( Panel_Crafting __instance )
        {
            if ( Settings.Instance.UIExtraControls && __instance.m_CraftButton.isActiveAndEnabled )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBeginCrafting();
                }

                if ( InputManager.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.m_RequirementContainer.m_QuantitySelect.OnDecrease();
                }
                else if ( InputManager.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.m_RequirementContainer.m_QuantitySelect.OnIncrease();
                }
            }
        }
    }
}

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
            if ( Settings.Instance.EnableMod && Settings.Instance.CraftRememberFilter )
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
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls && __instance.m_CraftButton.isActiveAndEnabled )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBeginCrafting();
                }

                if ( ModInput.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.m_RequirementContainer.m_QuantitySelect.OnDecrease();
                }
                else if ( ModInput.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.m_RequirementContainer.m_QuantitySelect.OnIncrease();
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_Crafting ), "RefreshSelectedBlueprint" )]
    internal class Patch_Panel_Crafting_RefreshSelectedBlueprint
    {
        static void Postfix( Panel_Crafting __instance )
        {
            if ( __instance.SelectedBPI != null )
            {
                Il2CppSystem.Collections.Generic.List<GearItem> Tools = __instance.SelectedBPI.GetToolsAvailableToCraft( GameManager.GetInventoryComponent() );
                if ( Tools.Count > 0 )
                {
                    GearItem? BestTool = null;
                    int BestIndex = -1;

                    for ( int Index = 0; Index < Tools.Count; ++Index )
                    {
                        GearItem Tool = Tools[ Index ];
                        if ( Tool != null )
                        {
                            if ( BestTool == null || Tool.m_ToolsItem.m_CraftingAndRepairTimeModifier < BestTool.m_ToolsItem.m_CraftingAndRepairTimeModifier )
                            {
                                BestIndex = Index;
                                BestTool = Tool;
                            }
                        }
                    }

                    if ( BestTool != null )
                    {
                        CraftingRequirementContainer Container = __instance.GetComponentInChildren<CraftingRequirementContainer>();
                        CraftingRequirementMultiTool? MultiTool = Container?.m_MultiTool;
                        if ( Container != null && MultiTool != null )
                        {
                            MultiTool.m_SelectedIndex = MultiTool.m_ToolOptions.IndexOf( BestTool );
                            MultiTool.RefreshDisplayed();
                            Container.OnSelectedToolChanged();
                        }
                    }
                }
            }
        }
    }

}

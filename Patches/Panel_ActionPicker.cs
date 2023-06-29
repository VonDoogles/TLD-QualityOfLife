using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_ActionPicker ), "TakeCharcoalCallback" )]
    internal class Patch_Panel_ActionPicker_TakeCharcoalCallback
    {
        static bool Prefix( Panel_ActionPicker __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.FireCharcoalPickUnits )
            {
                GameObject InteractObj = __instance.GetObjectInteractedWith();
                if ( InteractObj != null )
                {
                    FireplaceInteraction Interaction = InteractObj.GetComponent<FireplaceInteraction>();
                    if ( Interaction != null && Interaction.Fire != null )
                    {
                        Panel_PickUnits PickUnits = InterfaceManager.GetPanel<Panel_PickUnits>();
                        if ( PickUnits != null )
                        {
                            GearItem Charcoal = GearItem.LoadGearItemPrefab( "GEAR_Charcoal" );
                            PickUnits.SetGearForHarvest( Charcoal );
                            PickUnits.m_numUnits = 1;
                            PickUnits.m_maxUnits = Interaction.Fire.GetAvailableCharcoalPieces();
                            PickUnits.Enable( true );
                            PickUnits.Refresh();
                            PickUnits.m_EnablePanelOnExit = EnablePanelOnExit.None;
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}

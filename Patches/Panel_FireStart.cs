using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_FireStart ), "Enable", new Type[] { typeof( bool ) } )]
    internal class Patch_Panel_FireStart_Enable
    {
        internal static string? AccelerantName;
        internal static string? StarterName;
        internal static string? FuelSourceName;
        internal static string? TingerName;

        static void Postfix( Panel_FireStart __instance, bool enable )
        {
            if ( enable )
            {
                if ( Settings.Instance.FireRememberSelection )
                {
                    __instance.m_SelectedAccelerantIndex = Math.Max( 0, __instance.m_AccelerantList.FindIndex( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == AccelerantName ) ) );
                    __instance.m_SelectedStarterIndex = Math.Max( 0, __instance.m_StarterList.FindIndex( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == StarterName ) ) );
                    __instance.m_SelectedFuelIndex = Math.Max( 0, __instance.m_FuelList.FindIndex( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == FuelSourceName ) ) );
                    __instance.m_SelectedTinderIndex = Math.Max( 0, __instance.m_TinderList.FindIndex( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == TingerName ) ) );
                }

                if ( Settings.Instance.FireAutoSelectMagLens )
                {
                    GearItem MagLens = __instance.m_StarterList.Find( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == "GEAR_MagnifyingLens" ) );
                    if ( MagLens != null && __instance.HasDirectSunlight() )
                    {
                        __instance.m_SelectedStarterIndex = __instance.m_StarterList.IndexOf( MagLens );
                    }
                }

                if ( Settings.Instance.FireAutoSelectLitTorch )
                {
                    GearItem Torch = __instance.m_StarterList.Find( (Il2CppSystem.Predicate<GearItem>)( Gear => Gear?.name == "GEAR_Torch" ) );

                    PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                    if ( Torch != null && PlayerMan.m_ItemInHands != null && PlayerMan.m_ItemInHands.IsLitTorch() )
                    {
                        __instance.m_SelectedStarterIndex = __instance.m_StarterList.IndexOf( Torch );
                    }
                }

                if ( Settings.Instance.FireAutoSkipBirchBark )
                {
                    FuelSourceItem Tinder = __instance.GetSelectedTinder();
                    if ( Tinder != null && Tinder.name == "GEAR_BarkTinder" )
                    {
                        __instance.IncreaseTinder();
                    }
                }

                __instance.Refresh();
            }
        }
    }

    [HarmonyPatch( typeof( Panel_FireStart ), "OnStartFire" )]
    internal class Patch_Panel_FireStart_OnStartFire
    {
        static void Postfix( Panel_FireStart __instance )
        {
            Patch_Panel_FireStart_Enable.AccelerantName = __instance.GetSelectedAccelerant()?.name;
            Patch_Panel_FireStart_Enable.StarterName = __instance.GetSelectedFireStarter()?.name;
            Patch_Panel_FireStart_Enable.FuelSourceName = __instance.GetSelectedFuelSource()?.name;
            Patch_Panel_FireStart_Enable.TingerName = __instance.GetSelectedTinder()?.name;
        }
    }

    [HarmonyPatch( typeof( Panel_FireStart ), "UpdateMenuNavigation" )]
    internal class Patch_Panel_FireStart_UpdateMenuNavigation
    {
        static void Postfix( Panel_FireStart __instance )
        {
            if ( Settings.Instance.HighlightSelection )
            {
                int SelectedIndex = __instance.m_SelectedButtonIndex;

                for ( int Index = 0; Index < __instance.m_StartFireSelectionRows.Count; ++Index )
                {
                    UISprite OnSelected = __instance.m_StartFireSelectionRows[ Index ].m_BackgroundOnSelected;
                    if ( OnSelected != null )
                    {
                        OnSelected.gameObject.SetActive( Index == SelectedIndex );
                    }
                }
            }

            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __instance.OnStartFire();
                }
            }
        }
    }

}

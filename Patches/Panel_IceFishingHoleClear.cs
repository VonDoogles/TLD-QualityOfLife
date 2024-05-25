using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_IceFishingHoleClear ), "Enable", new Type[] { typeof( bool ) } )]
	internal class Patch_Panel_IceFishingHoleClear_Enable
	{
        static void Postfix( Panel_IceFishingHoleClear __instance, bool enable )
		{
            if ( Settings.Instance.EnableMod && enable )
			{
                if ( Settings.Instance.FishingRememberSelection && __instance.m_ToolUsed != null )
                {
                    Il2CppSystem.Predicate<GearItem> MatchToolUsed = (Il2CppSystem.Predicate<GearItem>)( Gear =>
                    {
                        return Gear?.name == __instance.m_ToolUsed.name;
                    } );

					int Index = __instance.m_AvailableTools.FindIndex( MatchToolUsed );
					if ( Index != -1 )
					{
						__instance.SelectToolByIndex( Index );
					}
				}
			}
		}
	}

    [HarmonyPatch( typeof( Panel_IceFishingHoleClear ), "Update")]
    internal class Patch_Panel_IceFishingHoleClear_Update
    {
        static void Postfix( Panel_IceFishingHoleClear __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnBreakIce();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll > 0 )
                {
                    __instance.PrevTool();
                }
                else if ( Scroll < 0 )
                {
                    __instance.NextTool();
                }
            }
        }
    }
}

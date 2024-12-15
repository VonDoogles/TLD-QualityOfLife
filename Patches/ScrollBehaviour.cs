using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.UI.Scroll;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( ScrollBehaviour ), "UpdateSelection" )]
    internal class Patch_ScrollBehaviour_UpdateSelection
    {
        static bool Prefix( ScrollBehaviour __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.ScrollWheelItemSelection )
			{
				float Scroll = InputManager.GetScroll( __instance );
				if ( Scroll > 0 )
				{
					if ( __instance.SelectedIndex > 0 )
					{
						__instance.SetSelectedIndex( __instance.SelectedIndex - 1, false, true );
						return false;
					}
					else if ( __instance.m_WrapAround )
					{
						__instance.SetSelectedIndex( __instance.m_TotalItems - 1, false, true );
						return false;
					}
				}
				else if ( Scroll < 0 )
				{
					if ( __instance.SelectedIndex < ( __instance.m_TotalItems - 1 ) )
					{
						__instance.SetSelectedIndex( __instance.SelectedIndex + 1, false, true );
						return false;
					}
					else if ( __instance.m_WrapAround )
					{
						__instance.SetSelectedIndex( 0, false, true );
						return false;
					}
				}
			}
			return true;
		}
    }
}

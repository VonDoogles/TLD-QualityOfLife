using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_SurvivalSettings ), "Update" )]
    internal class Patch_Panel_SurvivalSettings_Upadte
    {
        static void Postfix( Panel_SurvivalSettings __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
			{
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.EquipKey ) )
				{
					__instance.m_ToggleButton?.OnClick();
				}
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
				{
					__instance.HandleOnClickContinue();
				}

				float Scroll = InputManager.GetScroll( __instance );
				if ( Scroll > 0 || ModInput.GetKeyDown( __instance, KeyCode.A ) )
				{
					__instance.GoToPreviousIndex();
				}
				else if ( Scroll < 0 || ModInput.GetKeyDown( __instance, KeyCode.D ) )
				{
					__instance.GoToNextIndex();
				}
			}
        }
    }
}

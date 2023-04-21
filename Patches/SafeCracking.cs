using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( SafeCracking ), "GetDialDelta" )]
    internal class Patch_SafeCracking_GetDialDelta
    {
        static void Postfix( SafeCracking __instance, ref float __result )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( __result == 0.0f )
                {
                    bool bPrecisionRotate = Input.GetKey( Settings.Instance.PrecisionRotateKey );
                    if ( bPrecisionRotate )
                    {
                        __result = InputManager.GetScroll( __instance );
                    }
                    else
                    {
                        __result = Math.Sign( InputManager.GetScroll( __instance ) ) * __instance.m_DegreesPerTick;
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( SafeCracking ), "Update" )]
    internal class Patch_SafeCracking_Update
    {
        static void Postfix( SafeCracking __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( __instance.m_Cracked && Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    Panel_SafeCracking Panel = __instance.m_SafeCracking.GetPanel();
                    if ( Panel != null )
                    {
                        Panel.OnOpen();
                    }
                }
            }
        }
    }

}

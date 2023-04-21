using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Confirmation ), "Update" )]
    internal class Patch_Panel_Confirmation_Update
    {
        static void Postfix( Panel_Confirmation __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                Panel_Confirmation.ConfirmationRequest CurrentRequest = __instance.GetCurrentConfirmationRequest();
                if ( CurrentRequest != null )
                {
                    if ( !CurrentRequest.IsInputType() )
                    {
                        if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) || Input.GetKeyDown( Settings.Instance.InteractKey ) )
                        {
                            __instance.OnConfirm();
                        }
                    }
                }
            }
        }
    }

}

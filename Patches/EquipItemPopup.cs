using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( EquipItemPopup ), "Update" )]
    internal class Patch_EquipItemPopup_Update
    {
        static void Postfix( EquipItemPopup __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.SeparateInteract )
            {
                __instance.m_ButtonPromptLeft.gameObject.SetActive( false );
                __instance.m_ButtonPromptRight.gameObject.SetActive( false );

                if ( __instance.m_EquipPopupWidget.isActiveAndEnabled )
                {
                    PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                    string PlaceStr = PlayerMan.GetPlaceMeshFireButtonString();
                    string CancelStr = PlayerMan.GetPlaceMeshAltFireButtonString();

                    __instance.m_ButtonPromptFire.ShowPromptForKey( PlaceStr, Settings.Instance.InteractKey.ToString() );
                    __instance.m_ButtonPromptFire.m_KeyboardButtonLabel.text = Settings.Instance.InteractKey.ToString();
                    __instance.m_ButtonPromptAltFire.ShowPromptForKey( CancelStr, Settings.Instance.DropKey.ToString() );
                    __instance.m_ButtonPromptAltFire.m_KeyboardButtonLabel.text = Settings.Instance.DropKey.ToString();
                }
            }
            else
            {
                __instance.m_ButtonPromptLeft.gameObject.SetActive( true );
                __instance.m_ButtonPromptRight.gameObject.SetActive( true );
            }
        }
    }
}

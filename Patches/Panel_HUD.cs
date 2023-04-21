using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_HUD ), "Update" )]
    internal class Patch_Panel_HUD_Update
    {
        static void SetGameObjectActive( GameObject? GameObj, bool Active )
        {
            if ( GameObj != null && GameObj.activeSelf != Active )
            {
                GameObj.SetActive( Active );
            }
        }

        static void Postfix( Panel_HUD __instance )
        {
            ButtonPrompt? PromptPutBack = __instance.m_InspectMode_InspectPrompts.transform.FindChild( "ButtonPromp_PutBack" )?.GetComponent<ButtonPrompt>();
            ButtonPrompt? PromptTake = __instance.m_InspectMode_InspectPrompts.transform.FindChild( "ButtonPromp_Take" )?.GetComponent<ButtonPrompt>();

            if ( Settings.Instance.SeparateInteract )
            {
                if ( PromptPutBack == null )
                {
                    PromptPutBack = GameObject.Instantiate( __instance.m_EquipItemPopup.m_ButtonPromptFire.gameObject ).GetComponent<ButtonPrompt>();
                    PromptPutBack.gameObject.name = "ButtonPromp_PutBack";
                    PromptPutBack.transform.parent = __instance.m_InspectMode_Take.transform.parent;
                    Vector3 LocalPos = __instance.m_InspectMode_Take.transform.localPosition;
                    LocalPos.y = 148;
                    PromptPutBack.transform.localPosition = LocalPos;
                    PromptPutBack.transform.localScale = __instance.m_InspectMode_Take.transform.localScale;
                    PromptPutBack.ShowPromptForKey( __instance.m_InspectMode_PutBack.text, Settings.Instance.DropKey.ToString() );
                    PromptPutBack.m_KeyboardButtonLabel.text = Settings.Instance.DropKey.ToString();
                }
                if ( PromptTake == null )
                {
                    PromptTake = GameObject.Instantiate( __instance.m_EquipItemPopup.m_ButtonPromptFire.gameObject ).GetComponent<ButtonPrompt>();
                    PromptTake.gameObject.name = "ButtonPromp_Take";
                    PromptTake.transform.parent = __instance.m_InspectMode_PutBack.transform.parent;
                    Vector3 LocalPos = __instance.m_InspectMode_PutBack.transform.localPosition;
                    LocalPos.y = 148;
                    PromptTake.transform.localPosition = LocalPos;
                    PromptTake.transform.localScale = __instance.m_InspectMode_PutBack.transform.localScale;
                    PromptTake.ShowPromptForKey( __instance.m_InspectMode_Take.text, Settings.Instance.InteractKey.ToString() );
                    PromptTake.m_KeyboardButtonLabel.text = Settings.Instance.InteractKey.ToString();
                }

                bool bShouldBeActive = __instance.m_InspectMode_StandardElementsParent.activeSelf;

                SetGameObjectActive( PromptPutBack?.gameObject, bShouldBeActive );
                SetGameObjectActive( PromptTake?.gameObject, bShouldBeActive );
                
                SetGameObjectActive( __instance.m_InspectMode_Take?.gameObject, false );
                SetGameObjectActive( __instance.m_InspectMode_PutBack?.gameObject, false );
            }
            else
            {
                SetGameObjectActive( PromptPutBack?.gameObject, false );
                SetGameObjectActive( PromptTake?.gameObject, false );
            }
        }
    }

}

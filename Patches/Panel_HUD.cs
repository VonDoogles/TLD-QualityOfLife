using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_HUD ), "Enable" )]
    internal class Patch_Panel_HUD_Enable
    {
        static float OffsetMax = 400.0f;

        static void Postfix( Panel_HUD __instance, bool enable )
        {
            if ( Settings.Instance.EnableMod && enable && __instance.m_BuffNotificationPanel != null )
            {
                Vector3 LocalPos = __instance.m_BuffNotificationPanel.transform.localPosition;
                float DesiredY = OffsetMax * Settings.Instance.BuffOffsetVertical;
                if ( LocalPos.y != DesiredY )
                {
                    LocalPos.y = DesiredY;
                    __instance.m_BuffNotificationPanel.transform.localPosition = LocalPos;
                }
            }

            if ( Settings.Instance.EnableMod && enable )
            {
                CreateWeightLabel( __instance.m_Sprite_CapacityBuff.transform.parent );

                CreateWindStatusBar( __instance.m_SmallSizeGroup, "StatusBars_Small/StatusBarHungerSpawner/StatusBar/Root" );
                CreateWindStatusBar( __instance.m_RegularSizeGroup, "StatusBars_Regular/StatusBarHungerSpawner/StatusBar/Root" );
                CreateWindStatusBar( __instance.m_LargeSizeGroup, "StatusBars_Large/StatusBarHungerSpawner/StatusBar/Root" );
            }
        }

        private static void CreateWeightLabel( Transform Parent )
        {
            if ( Parent != null )
            {
                Transform WeightLabel = Parent.FindChild( "WeightLabel" );
                if ( WeightLabel == null )
                {
                    Panel_Inventory PanelInv = InterfaceManager.GetPanel<Panel_Inventory>();
                    if ( PanelInv != null && PanelInv.m_Label_CarryCapacity != null )
                    {
                        GameObject GameObj = UnityEngine.Object.Instantiate( PanelInv.m_Label_CarryCapacity.gameObject );
                        if ( GameObj != null )
                        {
                            GameObj.name = "WeightLabel";
                            WeightLabel = GameObj.transform;
                            WeightLabel.parent = Parent;
                            WeightLabel.localPosition = Vector3.zero;
                            GameObj.AddComponent<WeightLabel>();
                        }
                    }
                }
                WidgetUtils.SetActive( WeightLabel, Settings.Instance.WeightLabel != WeightLabelType.None );
            }
        }

        static void CreateWindStatusBar( Transform Parent, string HungerName )
        {
            if ( Parent != null )
            {
                Transform HungerSpawner = Parent.FindChild( HungerName );
                if ( HungerSpawner != null )
                {
                    Transform StatusBarWind = HungerSpawner.FindChild( "StatusBarWind" );
                    if ( StatusBarWind == null )
                    {
                        GameObject GameObj = new GameObject( "StatusBarWind" );
                        if ( GameObj != null )
                        {
                            StatusBarWind = GameObj.transform;
                            GameObj.transform.parent = HungerSpawner;
                            GameObj.transform.localPosition = Vector3.zero;
                            GameObj.AddComponent<StatusBarWind>();
                        }
                    }
                    WidgetUtils.SetActive( StatusBarWind, Settings.Instance.WindStatusBar != WindStatusType.None );
                }
            }
        }
    }

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

            if ( Settings.Instance.EnableMod && Settings.Instance.SeparateInteract )
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

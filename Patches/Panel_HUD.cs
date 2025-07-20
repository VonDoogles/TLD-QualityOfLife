using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_HUD ), "Initialize" )]
    internal class Patch_Panel_HUD_Initialize
    {
        static void Postfix( Panel_HUD __instance )
        {
            if ( Settings.Instance.EnableMod )
            {
				WidgetUtils.GetControlSchemeMouse( __instance.m_InspectMode_Equip.transform, out UILabel? EquipLabel, out UISprite? EquipSprite );
				WidgetUtils.GetControlSchemeMouse( __instance.m_InspectMode_Take.transform, out UILabel? TakeLabel, out UISprite? TakeSprite );

				UILabel? PutBackLabel = __instance.m_Button_InspectModePutBack_PC.GetComponentInChildren<UILabel>( true );
				UISprite? PutBackSprite = __instance.m_Button_InspectModePutBack_PC.GetComponentInChildren<UISprite>( true );

				if ( EquipLabel != null )
				{
					if ( PutBackLabel == null && PutBackSprite != null )
					{
						PutBackLabel = GameObject.Instantiate( EquipLabel.gameObject, PutBackSprite.transform ).GetComponent<UILabel>();
						PutBackLabel.transform.localPosition = Vector3.zero;
						PutBackLabel.fontSize = 18;
					}

					if ( TakeLabel == null && TakeSprite != null )
					{
						TakeLabel = GameObject.Instantiate( EquipLabel.gameObject, TakeSprite.transform ).GetComponent<UILabel>();
						TakeLabel.transform.localPosition = Vector3.zero;
						TakeLabel.fontSize = 18;
					}
				}

				WidgetUtils.SetActive( PutBackLabel, false );
				WidgetUtils.SetActive( TakeLabel, false );
            }
        }
    }

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

                CreateHypoStatusBar( __instance.m_SmallSizeGroup, "StatusBars_Small/StatusBarColdSpawner/StatusBar/Root" );
                CreateHypoStatusBar( __instance.m_RegularSizeGroup, "StatusBars_Regular/StatusBarColdSpawner/StatusBar/Root" );
                CreateHypoStatusBar( __instance.m_LargeSizeGroup, "StatusBars_Large/StatusBarColdSpawner/StatusBar/Root" );
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
                Transform HungerRoot = Parent.FindChild( HungerName );
                if ( HungerRoot != null )
                {
                    Transform StatusBarWind = HungerRoot.FindChild( "StatusBarWind" );
                    if ( StatusBarWind == null )
                    {
                        GameObject GameObj = new GameObject( "StatusBarWind" );
                        if ( GameObj != null )
                        {
                            StatusBarWind = GameObj.transform;
                            GameObj.transform.parent = HungerRoot;
                            GameObj.transform.localPosition = Vector3.zero;
                            GameObj.AddComponent<StatusBarWind>();
                        }
                    }
                    WidgetUtils.SetActive( StatusBarWind, Settings.Instance.WindStatusBar != WindStatusType.None || Settings.Instance.ShowTemperatureLabels );
                }
            }
        }

        static void CreateHypoStatusBar( Transform Parent, string ColdName )
        {
            if ( Parent != null )
            {
                Transform ColdRoot = Parent.FindChild( ColdName );
                if ( ColdRoot != null )
                {
                    Transform Foreground = ColdRoot.FindChild( "Foreground" );
                    if ( Foreground != null && Foreground.GetComponent<StatusBarHypo>() == null )
                    {
                        Foreground.gameObject.AddComponent<StatusBarHypo>();
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_HUD ), "Update" )]
    internal class Patch_Panel_HUD_Update
    {
        static void Postfix( Panel_HUD __instance )
        {
			WidgetUtils.GetControlSchemeMouse( __instance.m_InspectMode_Take.transform, out UILabel? TakeLabel, out UISprite? TakeSprite );

			UILabel PutBackLabel = __instance.m_Button_InspectModePutBack_PC.GetComponentInChildren<UILabel>( true );
			UISprite PutBackSprite = __instance.m_Button_InspectModePutBack_PC.GetComponentInChildren<UISprite>( true );

            if ( Settings.Instance.EnableMod && Settings.Instance.SeparateInteract )
			{
				WidgetUtils.SetSprite( PutBackSprite, "genericButton_normal", 32, 30 );
				WidgetUtils.SetSprite( TakeSprite, "genericButton_normal", 32, 30 );
				WidgetUtils.SetLabelText( PutBackLabel, Settings.Instance.DropKey.ToString() );
				WidgetUtils.SetLabelText( TakeLabel, Settings.Instance.InteractKey.ToString() );
				WidgetUtils.SetActive( PutBackLabel, true );
				WidgetUtils.SetActive( TakeLabel, true );

				WidgetUtils.EnsureChildOrder( __instance.m_InspectMode_ButtonLayout, __instance.m_InspectMode_PutBack, __instance.m_InspectMode_Take );
			}
			else
			{
				WidgetUtils.SetSprite( PutBackSprite, "ico_mouseButtonR", 42, 42 );
				WidgetUtils.SetSprite( TakeSprite, "ico_mouseButtonL", 42, 42 );
				WidgetUtils.SetActive( PutBackLabel, false );
				WidgetUtils.SetActive( TakeLabel, false );

				WidgetUtils.EnsureChildOrder( __instance.m_InspectMode_ButtonLayout, __instance.m_InspectMode_Take, __instance.m_InspectMode_PutBack );
			}
        }
    }

    [HarmonyPatch( typeof( Panel_HUD ), "UpdateSafehouse" )]
    internal class Patch_Panel_HUD_UpdateSafehouse
    {
        static void Postfix( Panel_HUD __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.SafehouseDayTimeOnly )
            {
                if ( TimeOfDay.Instance != null && !TimeOfDay.Instance.IsDayWithExtendedHours( 0 ) )
                {
                    WidgetUtils.SetActive( __instance?.m_SafehouseRoot, false );
                }
            }
        }
    }

	[HarmonyPatch( typeof( Panel_HUD ), "UpdateStaminaBar" )]
	internal class Patch_Panel_HUD_UpdateStaminaBar
	{
        static void Postfix( Panel_HUD __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.ShowStaminaUntilFull )
			{
				PlayerMovement Movement = GameManager.GetPlayerMovementComponent();
				if ( Movement != null && Movement.CurrentStamina < Movement.CurrentMaxStamina )
				{
					__instance.m_SprintFadeTimeTracker = __instance.m_SprintBar_SecondsBeforeFadeOut;
				}
			}
		}
	}

}

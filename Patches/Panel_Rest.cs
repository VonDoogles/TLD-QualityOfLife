using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gameplay;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Rest ), "Enable", new Type[] { typeof( bool ) } )]
    internal class Patch_Panel_Rest_Enable
    {
        static Vector3 Offset = new Vector3( 80, 0, 0 );
		static int QuickHours = 10;

        static void Postfix( Panel_Rest __instance, bool enable )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.RestPreset && enable )
            {
                Transform Parent = __instance.m_ButtonIncrease.transform.parent;
                GenericButtonMouseSpawner ButtonSpawner = __instance.m_PickUpButton.GetComponent<GenericButtonMouseSpawner>();

				ExperienceMode XpMode = GameManager.GetExperienceModeManagerComponent().GetCurrentExperienceMode();
				QuickHours = ( XpMode.m_ThirstRateScale > 1.0f ) ? 8 : 10;

                GameObject? ButtonQuick10 = Parent.FindChild( "ButtonQuick10" )?.gameObject;
                if ( ButtonQuick10 == null )
                {
					var OnSet = () =>
                    {
                        __instance.m_SleepHours = GetQuickHours();
                    };

                    ButtonQuick10 = GameObject.Instantiate( ButtonSpawner.m_Prefab, Parent );
                    ButtonQuick10.name = "ButtonQuick10";
                    ButtonQuick10.transform.localPosition = __instance.m_ButtonIncrease.transform.localPosition + Offset;

                    UILabel Label = ButtonQuick10.GetComponentInChildren<UILabel>();
                    if ( Label != null )
                    {
                        Label.fontSize = 18;
                    }

                    UIButton Button = ButtonQuick10.GetComponent<UIButton>();
                    if ( Button != null )
                    {
                        Button.onClick.Add( new EventDelegate( OnSet ) );
                    }

                    BoxCollider Box = ButtonQuick10.GetComponent<BoxCollider>();
                    if ( Box != null )
                    {
                        Box.size = new Vector3( 48, 30, 0 );
                    }

                    Transform Background = ButtonQuick10.transform.FindChild( "Background" );
                    if ( Background != null )
                    {
                        Background.GetComponentInChildren<UISprite>().SetDimensions( 48, 38 );
                    }

                    Transform BackGlow = ButtonQuick10.transform.FindChild( "BackGlow" );
                    if ( BackGlow != null )
                    {
                        BackGlow.GetComponentInChildren<UISprite>().SetDimensions( 64, 83 );
                    }
                }

				if ( ButtonQuick10 != null )
				{
					UILocalize Localize = ButtonQuick10.GetComponentInChildren<UILocalize>();
					if ( Localize != null )
					{
						Localize.key = QuickHours.ToString();
						Localize.OnLocalize();
					}
				}
            }
        }

		static int GetQuickHours()
		{
			return QuickHours;
		}
    }

    [HarmonyPatch( typeof( Panel_Rest ), "Update" )]
    internal class Patch_Panel_Rest_Update
    {
        static void Postfix( Panel_Rest __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    if ( __instance.m_ShowPassTime && __instance.m_PassTimeButtonObject.activeInHierarchy )
                    {
                        __instance.OnPassTime();
                    }
                    else if ( __instance.m_SleepButton.activeInHierarchy )
                    {
                        __instance.OnRest();
                    }
                }

                if ( ModInput.GetKeyDown( __instance, KeyCode.Tab ) )
                {
                    if ( __instance.m_ShowPassTime && !__instance.m_ShowPassTimeOnly )
                    {
                        __instance.OnSelectRest();
                    }
                    else if ( !__instance.m_ShowPassTime )
                    {
                        __instance.OnSelectPassTime();
                    }
                }

                if ( ModInput.GetKeyDown( __instance, Settings.Instance.DropKey ) )
                {
                    __instance.OnPickUp();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll < 0 || ModInput.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.OnDecreaseHours();
                }
                else if ( Scroll > 0 || ModInput.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.OnIncreaseHours();
                }
            }
        }
    }

}

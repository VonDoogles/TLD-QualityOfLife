﻿using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_Rest ), "Enable", new Type[] { typeof( bool ) } )]
    internal class Patch_Panel_Rest_Enable
    {
        static Vector3 Offset = new Vector3( 80, 0, 0 );

        static void Postfix( Panel_Rest __instance, bool enable )
        {
            if ( Settings.Instance.UIExtraControls && enable )
            {
                Transform Parent = __instance.m_ButtonIncrease.transform.parent;
                GenericButtonMouseSpawner ButtonSpawner = __instance.m_PickUpButton.GetComponent<GenericButtonMouseSpawner>();

                Transform ButtonQuick10 = Parent.FindChild( "ButtonQuick10" );
                if ( ButtonQuick10 == null )
                {
                    var OnSet = () =>
                    {
                        __instance.m_SleepHours = 10;
                    };

                    GameObject NewButton = GameObject.Instantiate( ButtonSpawner.m_Prefab, Parent );
                    NewButton.name = "ButtonQuick10";
                    NewButton.transform.localPosition = __instance.m_ButtonIncrease.transform.localPosition + Offset;

                    UILabel Label = NewButton.GetComponentInChildren<UILabel>();
                    if ( Label != null )
                    {
                        Label.fontSize = 18;
                    }

                    UILocalize Localize = NewButton.GetComponentInChildren<UILocalize>();
                    if ( Localize != null )
                    {
                        Localize.key = "10";
                        Localize.OnLocalize();
                    }

                    UIButton Button = NewButton.GetComponent<UIButton>();
                    if ( Button != null )
                    {
                        Button.onClick.Add( new EventDelegate( OnSet ) );
                    }

                    BoxCollider Box = NewButton.GetComponent<BoxCollider>();
                    if ( Box != null )
                    {
                        Box.size = new Vector3( 48, 30, 0 );
                    }

                    Transform Background = NewButton.transform.FindChild( "Background" );
                    if ( Background != null )
                    {
                        Background.GetComponentInChildren<UISprite>().SetDimensions( 48, 38 );
                    }

                    Transform BackGlow = NewButton.transform.FindChild( "BackGlow" );
                    if ( BackGlow != null )
                    {
                        BackGlow.GetComponentInChildren<UISprite>().SetDimensions( 64, 83 );
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( Panel_Rest ), "Update" )]
    internal class Patch_Panel_Rest_Update
    {
        static void Postfix( Panel_Rest __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    if ( __instance.m_ShowPassTime )
                    {
                        __instance.OnPassTime();
                    }
                    else
                    {
                        __instance.OnRest();
                    }
                }

                if ( InputManager.GetKeyDown( __instance, KeyCode.Tab ) )
                {
                    if ( __instance.m_ShowPassTime )
                    {
                        __instance.OnSelectRest();
                    }
                    else
                    {
                        __instance.OnSelectPassTime();
                    }
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll < 0 || InputManager.GetKeyDown( __instance, KeyCode.A ) )
                {
                    __instance.OnDecreaseHours();
                }
                else if ( Scroll > 0 || InputManager.GetKeyDown( __instance, KeyCode.D ) )
                {
                    __instance.OnIncreaseHours();
                }
            }
        }
    }

}

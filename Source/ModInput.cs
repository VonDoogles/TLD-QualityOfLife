using HarmonyLib;
using Il2Cpp;
using System.Reflection;
using UnityEngine;

namespace QualityOfLife
{
    public static class ModInput
    {
        static bool CheckForUnityExplorer = true;
        static Type? TypeUIManager = null;

        public static bool ShouldProcessInput( KeyCode Key )
        {
            if ( CheckForUnityExplorer )
            {
                CheckForUnityExplorer = false;
                TypeUIManager = AccessTools.TypeByName( "UnityExplorer.UI.UIManager" );
            }

            if ( TypeUIManager != null )
            {
                MethodInfo? Method = TypeUIManager.GetMethod( "get_ShowMenu", BindingFlags.Public | BindingFlags.Static );
                if ( Method != null )
                {
                    bool? ShowMenu = Method.Invoke( TypeUIManager, null ) as bool?;
                    if ( ShowMenu != null && ShowMenu.Value )
                    {
                        return false;
                    }
                }
            }

            if ( uConsole.IsOn() )
            {
                return false;
            }

            Panel_Log PanelLog = InterfaceManager.GetPanel<Panel_Log>();
			if ( PanelLog != null && PanelLog.enabled && PanelLog.m_NotesTextField != null && PanelLog.m_NotesTextField.m_Input != null )
			{
				if ( PanelLog.m_NotesTextField.m_Input.isSelected )
				{
					return false;
				}
			}

            return Key != KeyCode.None;
        }

        public static bool GetKey( MonoBehaviour? Context, KeyCode Key )
        {
            if ( ShouldProcessInput( Key ) )
            {
                return Input.GetKey( Key );
            }
            return false;
        }

        public static bool GetKeyDown( MonoBehaviour? Context, KeyCode Key )
        {
            if ( ShouldProcessInput( Key ) )
            {
                return InputManager.GetKeyDown( Context, Key );
            }
            return false;
        }

        public static bool GetKeyUp( MonoBehaviour? Context, KeyCode Key )
        {
            if ( ShouldProcessInput( Key ) )
            {
                return Input.GetKeyUp( Key );
            }
            return false;
        }
    }
}

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
                    if ( ShowMenu != null )
                    {
                        return !ShowMenu.Value;
                    }
                }
            }

            if ( uConsole.IsOn() )
            {
                return false;
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

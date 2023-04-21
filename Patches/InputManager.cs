﻿using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( InputManager ), "GetFirePressed" )]
    internal class Patch_InputManager_GetFirePressed
    {
        static void Postrix( MonoBehaviour context, ref bool __result )
        {
            if ( __result && Settings.Instance.SeparateInteract )
            {
                PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                if ( PlayerMan != null )
                {
                    PlayerMan.SetCurrentInteraction( null );

                    if ( PlayerMan.IsInPlacementMode() )
                    {
                        __result = false;
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( InputManager ), "GetAltFirePressed" )]
    internal class Patch_InputManager_GetAltFirePressed
    {
        static void Postfix( MonoBehaviour context, ref bool __result )
        {
            if ( __result && Settings.Instance.SeparateInteract )
            {
                PlayerManager PlayerMan = GameManager.GetPlayerManagerComponent();
                if ( PlayerMan != null )
                {
                    PlayerMan.SetCurrentInteraction( null );

                    if ( PlayerMan.IsInPlacementMode() )
                    {
                        __result = false;
                    }
                }
            }
        }
    }

    [HarmonyPatch( typeof( InputManager ), "GetPickupPressed" )]
    internal class Patch_InputManager_GetPickupPressed
    {
        static void Postfix( MonoBehaviour context, ref bool __result )
        {
            if ( Settings.Instance.SeparateInteract )
            {
                __result = InputManager.GetKeyDown( context, Settings.Instance.InteractKey );
            }
        }
    }

    [HarmonyPatch( typeof( InputManager ), "GetPutBackPressed" )]
    internal class Patch_InputManager_GetPutBackPressed
    {
        static void Postfix( MonoBehaviour context, ref bool __result )
        {
            if ( Settings.Instance.SeparateInteract )
            {
                __result = InputManager.GetKeyDown( context, Settings.Instance.DropKey ) || InputManager.GetKeyDown( context, KeyCode.Escape );
            }
        }
    }

    [HarmonyPatch( typeof( InputManager ), "GetInteractPressed" )]
    internal class Patch_InputManager_GetInteractPressed
    {
        static bool Prefix( MonoBehaviour context, ref bool __result )
        {
            if ( Settings.Instance.SeparateInteract )
            {
                if ( Input.GetKeyDown( Settings.Instance.InteractKey ) )
                {
                    __result = true;
                }
                return false;
            }
            return true;
        }
    }

}

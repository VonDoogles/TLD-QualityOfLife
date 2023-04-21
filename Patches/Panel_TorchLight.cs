using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( Panel_TorchLight ), "Enable", new Type[] { typeof( bool ) } )]
    internal class Patch_Panel_TorchLight_Enable
    {
        static bool Prefix( Panel_TorchLight __instance, bool enable )
        {
            if ( enable )
            {
                Patch_Panel_TorchLight_OnUseSelectedItem.CallCount = 0;
            }
            return true;
        }
    }

    [HarmonyPatch( typeof( Panel_TorchLight ), "OnUseSelectedItem" )]
    internal class Patch_Panel_TorchLight_OnUseSelectedItem
    {
        static public int CallCount = 0;

        static bool Prefix( Panel_TorchLight __instance )
        {
            if ( Settings.Instance.TorchLightAlwaysShow )
            {
                if ( __instance.m_AvailableTools.Count == 1 )
                {
                    return ++CallCount > 1;
                }
            }
            return true;
        }
    }


    [HarmonyPatch( typeof( Panel_TorchLight ), "Update" )]
    internal class Patch_Panel_TorchLight_Update
    {
        static void Postfix( Panel_TorchLight __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( InputManager.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnUseSelectedItem();
                }
            }
        }
    }

}

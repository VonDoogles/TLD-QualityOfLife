using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_Clothing ), "Update" )]
    internal class Patch_Panel_Clothing_Update
    {
        static void Postfix( Panel_Clothing __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.DropKey ) )
                {
                    __instance.OnDropItem();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.EquipKey ) )
                {
                    __instance.OnUseClothingItem();
                }
                else if ( ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    __instance.OnActionsButton();
                }

                float Scroll = InputManager.GetScroll( __instance );
                if ( Scroll < 0 )
                {
                    __instance.NextTool();
                }
                else if ( Scroll > 0 )
                {
                    __instance.PrevTool();
                }
            }
        }
    }
}

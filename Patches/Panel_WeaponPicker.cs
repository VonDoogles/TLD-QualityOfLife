using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_WeaponPicker ), "Update" )]
    internal class Patch_Panel_WeaponPicker_Update
    {
        static void Postfix( Panel_WeaponPicker __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.UIExtraControls )
            {
                if ( ModInput.GetKeyDown( __instance, Settings.Instance.EquipKey ) || ModInput.GetKeyDown( __instance, Settings.Instance.InteractKey ) )
                {
                    int Index = __instance.m_SelectedItemIndex;
                    if ( Index >= 0 && Index < __instance.m_GridItemsList.Count )
                    {
                        __instance.m_GridItemsList[ Index ].OnClick();
                    }
                }
            }
        }
    }
}

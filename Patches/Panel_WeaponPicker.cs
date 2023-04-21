using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( Panel_WeaponPicker ), "Update" )]
    internal class Patch_Panel_WeaponPicker_Update
    {
        static void Postfix( Panel_WeaponPicker __instance )
        {
            if ( Settings.Instance.UIExtraControls )
            {
                if ( Input.GetKeyDown( Settings.Instance.EquipKey ) || Input.GetKeyDown( Settings.Instance.InteractKey ) )
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

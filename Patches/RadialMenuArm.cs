using HarmonyLib;
using Il2Cpp;

namespace QualityOfLife.Patches
{
    [HarmonyPatch( typeof( RadialMenuArm ), "UpdateStackLabel" )]
    internal class Patch_RadialMenuArm_UpdateStackLabel
    {
        static void Postfix( RadialMenuArm __instance )
        {
            if ( Settings.Instance.RadialCombineItems )
            {
                GearItem Item = __instance.GetGearItem();
                if ( Item != null )
                {
                    int Count = GameManager.GetInventoryComponent().GetNumGearWithName( Item.name );
                    __instance.m_StackLabel.text = Count.ToString();
                    __instance.m_StackLabel.ProcessText();
                    WidgetUtils.SetActive( __instance.m_StackLabel, true );
                }
            }
        }
    }
}

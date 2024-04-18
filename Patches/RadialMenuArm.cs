using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;

namespace QualityOfLife.Patches
{
    [HarmonyPatch( typeof( RadialMenuArm ), "UpdateStackLabel" )]
    internal class Patch_RadialMenuArm_UpdateStackLabel
    {
        static void Postfix( RadialMenuArm __instance )
        {
            if ( Settings.Instance.EnableMod && Settings.Instance.RadialCombineItems )
            {
				Inventory Inv = GameManager.GetInventoryComponent();
                GearItem Item = __instance.GetGearItem();

                if ( Inv != null && Item != null )
                {
                    int Count = Inv.GetNumGearWithName( Item.name );

					if ( Settings.Instance.RadialShowInsulatedFlaskContents )
					{
						foreach ( GearItemObject GearObj in GameManager.GetInventoryComponent().m_Items )
						{
							if ( GearObj != null && GearObj.m_GearItem != null && GearObj.m_GearItem.m_InsulatedFlask != null )
							{
								foreach ( GearItemObject ContentObj in GearObj.m_GearItem.m_InsulatedFlask.m_Items )
								{
									if ( ContentObj != null && ContentObj.m_GearItem != null && ContentObj.m_GearItem.name == Item.name )
									{
										++Count;
									}
								}
							}
						}
					}

                    __instance.m_StackLabel.text = Count.ToString();
                    __instance.m_StackLabel.ProcessText();
                    WidgetUtils.SetActive( __instance.m_StackLabel, true );
                }
            }
        }
    }
}

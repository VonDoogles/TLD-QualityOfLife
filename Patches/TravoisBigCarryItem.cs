using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.BigCarry;
using Il2CppVLB;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( TravoisBigCarryItem ), "Start" )]
	internal class Patch_TravoisBigCarryItem_Start
	{
		static void Postfix( TravoisBigCarryItem __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				if ( __instance.m_Container != null )
				{
					ContainerInteraction Interaction = __instance.m_Container.GetComponent<ContainerInteraction>();
					if ( Interaction != null )
					{
						Interaction.enabled = false;
					}

					__instance.m_Container.GetOrAddComponent<TravoisContainerInteraction>();

					if ( __instance.m_TravoisGearItem != null )
					{
						Container? GearContainer = GearHelper.FindOrCreateGearContainer( __instance.m_TravoisGearItem, __instance.m_Container );
						GearHelper.TransferItems( GearContainer, __instance.m_Container );
					}
				}
			}
		}
	}

	[HarmonyPatch( typeof( TravoisBigCarryItem ), "OnDestroy" )]
	internal class Patch_TravoisBigCarryItem_PickupCallback
    {
		static void Postfix( TravoisBigCarryItem __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				if ( __instance.m_TravoisGearItem != null )
				{
					Container? GearContainer = GearHelper.FindOrCreateGearContainer( __instance.m_TravoisGearItem, __instance.m_Container );
					GearHelper.TransferItems( __instance.m_Container, GearContainer );
				}
			}
        }
    }
}

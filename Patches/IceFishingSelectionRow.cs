using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gameplay.Fishing;
using UnityEngine;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( IceFishingSelectionRow ), "UpdateSelection" )]
    internal class Patch_IceFishingSelectionRow_UpdateSelection
    {
		static bool ItemCountWasSet = false;
		static Vector3? ItemCountLocalPosition = null;

		static Vector3 ItemCountOffset = new Vector3( 0, 26, 0 );


		static void Postfix( IceFishingSelectionRow __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.FishingShowRemainingBait )
			{
				SetItemCountLabel( __instance, __instance.SelectedItem );
				ItemCountWasSet = true;
			}
			else if ( ItemCountWasSet )
			{
				SetItemCountLabel( __instance, null );
				ItemCountWasSet = false;
			}
		}

		static void SetItemCountLabel( IceFishingSelectionRow __instance, GearItem? Item )
		{
			UILabel? ItemCount = __instance.transform.FindChild( "Label_ItemCount" )?.GetComponent<UILabel>();
			if ( ItemCount != null )
			{
				Inventory Inv = GameManager.GetInventoryComponent();
				if ( Inv != null && Item != null )
				{
					if ( !ItemCountLocalPosition.HasValue )
					{
						ItemCountLocalPosition = ItemCount.transform.localPosition;
					}

					int GearCount = Inv.GetNumGearWithName( Item.name );
					ItemCount.capsLock = false;
					ItemCount.text = ( GearCount > 1 ) ? $"x{GearCount}" : "";
					ItemCount.transform.localPosition = ItemCountOffset;
				}
				else
				{
					ItemCount.capsLock = true;
					ItemCount.text = "";

					if ( ItemCountLocalPosition.HasValue )
					{
						ItemCount.transform.localPosition = ItemCountLocalPosition.Value;
					}
				}
			}
		}
    }
}

﻿using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Cooking;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( CookableListItem ), "SetCookable" )]
    internal class Patch_CookableListItem_SetCookable
    {
		static void Postfix( CookableListItem __instance, CookableItem cookable, CookingPotItem cookingPot )
		{
			if ( cookable != null && cookable.m_GearItem != null )
			{
				Cookable? ToCook = cookable.m_GearItem.GetComponent<Cookable>();
				if ( ToCook != null && ToCook.m_CookedPrefab != null )
				{
					StackableItem? Stackable = ToCook.m_CookedPrefab.GetComponent<StackableItem>();
					if ( Stackable != null && Stackable.m_DefaultUnitsInItem > 1 )
					{
						string DisplayName = cookable.GetDisplayName();
						int DisplayCount = Stackable.m_DefaultUnitsInItem;
						WidgetUtils.SetLabelText( __instance.m_ItemName, $"{DisplayName} ({DisplayCount})" );
					}
				}
			}
		}
    }
}

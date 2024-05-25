using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using Il2CppVLB;
using MelonLoader.TinyJSON;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( Inventory ), "AddGear" )]
	internal class Patch_Inventory_AddGear
	{
		static void Postfix( Inventory __instance, GearItem gi, bool enableNotificationFlag )
		{
			TravoisContentItem.SetTravois( gi, null );
		}
	}

	[HarmonyPatch( typeof( Inventory ), "Serialize" )]
	internal class Patch_Inventory_Serialize
	{
		static bool Prefix( Inventory __instance )
		{
			return true;
		}

		static void Postfix( Inventory __instance, ref string __result )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				Variant Parsed = JSON.Load( __result );
				ProxyArray InnerItems = new();

				Il2CppSystem.Collections.Generic.List<GearItem> ItemList = new();
				TravoisUtil.GetTravoisContentItems( __instance, ItemList );

				foreach ( GearItem Item in ItemList )
				{
					TravoisContentItem ContentItem = Item.GetComponent<TravoisContentItem>();
					if ( ContentItem?.Travois != null )
					{
						ProxyObject Obj = new ProxyObject();
						Obj[ "ContainerID" ] = new ProxyNumber( ContentItem.Travois.m_InstanceID );
						Obj[ "InnerItemID" ] = new ProxyNumber( Item.m_InstanceID );
						InnerItems.Add( Obj );
					}
				}

				Parsed[ "QOL_InnerItems" ] = InnerItems;
				__result = Parsed.ToJSON();
				return;
			}
		}
	}

	[HarmonyPatch( typeof( Inventory ), "Deserialize" )]
	internal class Patch_Inventory_Deserialize
	{
		static void Postfix( Inventory __instance, string text )
		{
			if ( Settings.Instance.EnableMod )
			{
				if ( Settings.Instance.TravoisPickupWithContents )
				{
                    ProxyObject? Parsed = JSON.Load( text ) as ProxyObject;

                    // Backwards compatibility for v1.12.0
                    TryDeserialzeQOL_v1_12_0( __instance, Parsed );

					if ( Parsed != null && Parsed.TryGetValue( "QOL_InnerItems", out Variant InnerItems ) )
					{
						ProxyArray? Array = InnerItems as ProxyArray;
						if ( Array != null )
						{
							for ( int Idx = 0; Idx < Array.Count; ++Idx )
							{
								Variant Obj = Array[ Idx ];
								int ContainerID = Obj[ "ContainerID" ];
								int InnerItemID = Obj[ "InnerItemID" ];

								GearItem? InnerItem = GearHelper.GetItemByID( __instance.m_Items, InnerItemID );
								GearItem? OuterItem = GearHelper.GetItemByID( __instance.m_Items, ContainerID );

								if ( InnerItem != null && OuterItem != null )
								{
									TravoisContentItem.SetTravois( InnerItem, OuterItem );
								}
							}
						}
					}
				}

                Il2CppSystem.Collections.Generic.List<GearItem> CatTailItems = new();
				__instance.GearInInventory( "GEAR_CattailStalk", CatTailItems );

				foreach ( GearItem Item in CatTailItems )
				{
					CatTailHelper.TryUpdateCalories( Item?.m_FoodItem );
				}
			}
		}

        private static void TryDeserialzeQOL_v1_12_0( Inventory __instance, ProxyObject? Parsed )
        {
            if ( Parsed != null && Parsed.TryGetValue( "QOL_GearContainers", out Variant ArrayVariant ) )
            {
                ProxyArray? Array = ArrayVariant as ProxyArray;
                if ( Array != null )
                {
                    for ( int Idx = 0; Idx < Array.Count; ++Idx )
                    {
                        Variant Obj = Array[ Idx ];
                        int InstanceID = Obj[ "InstanceID" ];
                        string ContainerSerialized = Obj[ "ContainerSerialized" ];

                        GearItem? Item = GearHelper.GetItemByID( __instance.m_Items, InstanceID );
						Container? GearContainer = Item?.GetOrAddComponent<Container>();

						if ( GearContainer != null )
						{
							Il2CppSystem.Collections.Generic.List<GearItem> LoadedItems = new();
							GearContainer.Deserialize( ContainerSerialized, LoadedItems );
						}
                    }
                }
            }
        }
    }

	[HarmonyPatch( typeof( Inventory ), "GetExtraScentIntensity" )]
	internal class Patch_Inventory_GetCurrentTotalScentIntensity
    {
		static void Postfix( Inventory __instance, ref float __result )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				Il2CppSystem.Collections.Generic.List<GearItem> ItemList = new();
				__instance.GetItems( "GEAR_Travois", ItemList );

				foreach ( GearItem Item in ItemList )
				{
					if ( Item != null && Item.m_Travois != null )
					{
						Container GearContainer = Item.m_Travois.GetComponent<Container>();
						if ( GearContainer != null )
						{
							foreach ( GearItem InnerItem in GearContainer.m_Items )
							{
								if ( InnerItem != null && InnerItem.m_GearItemData != null )
								{
									__result += InnerItem.m_GearItemData.m_ScentIntensity;
								}
							}
						}
					}
				}
			}
		}
	}
}

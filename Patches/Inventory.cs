using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using Il2CppVLB;
using MelonLoader.TinyJSON;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( Inventory ), "Serialize" )]
	internal class Patch_Inventory_Serialize
	{
		static void Postfix( Inventory __instance, ref string __result )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				Variant Parsed = JSON.Load( __result );

				ProxyArray Array = new ProxyArray();

				foreach ( GearItemObject ItemObject in __instance.m_Items )
				{
					if ( ItemObject != null && ItemObject.m_GearItem != null )
					{
						Container GearContainer = ItemObject.m_GearItem.GetComponent<Container>();
						if ( GearContainer != null )
						{
							ProxyObject Obj = new ProxyObject();
							Obj[ "InstanceID" ] = new ProxyNumber( ItemObject.m_GearItem.m_InstanceID );
							Obj[ "ContainerSerialized" ] = new ProxyString( GearContainer.Serialize() );
							Array.Add( Obj );
						}
					}
				}

				Parsed[ "QOL_GearContainers" ] = Array;
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
								if ( Item != null )
								{
									Container GearContainer = Item.GetOrAddComponent<Container>();
									if ( GearContainer != null )
									{
										Il2CppSystem.Collections.Generic.List<GearItem> LoadedItems = new();
										GearContainer.Deserialize( ContainerSerialized, LoadedItems );

										if ( GearContainer.m_Items.Count <= 0 )
										{
											foreach ( GearItem Loaded in LoadedItems )
											{
												GearContainer.AddGear( Loaded );
											}
										}
									}
								}
							}
						}
					}
				}

                Il2CppSystem.Collections.Generic.List<GearItem> CatTailItems = new();
				__instance.GearInInventory( "GEAR_CattailStalk", CatTailItems );

				foreach ( GearItem Item in CatTailItems )
				{
					if ( Item != null )
					{
						CatTailHelper.TryUpdateCalories( Item.m_FoodItem );
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

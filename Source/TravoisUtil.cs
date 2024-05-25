using Il2Cpp;
using Il2CppTLD.BigCarry;
using Il2CppVLB;
using UnityEngine;

namespace QualityOfLife
{
    public static class TravoisUtil
    {
		static Func<GearItem, bool> HasTravoisContentItem = ( Item =>
		{
			TravoisContentItem ContentItem = Item.GetComponent<TravoisContentItem>();
			return ContentItem != null;
		} );

		public static void DropTravois( GearItem Gear )
		{
			if ( Gear != null && Gear.m_Travois != null && Gear.m_Travois.CanDropTravois() )
			{
				Transform PlayerTrans = GameManager.GetPlayerTransform();
				if ( Gear.m_Travois.TryFindValidTravoisDropPosition( PlayerTrans ) )
				{
					Gear.Drop( 1 );
				}
			}
		}

		public static void GetTravoisContentItems( Inventory Inv, Il2CppSystem.Collections.Generic.List<GearItem> ItemList )
		{
			if ( Inv != null )
			{
				foreach ( GearItem Item in Inv.m_Items )
				{
					if ( HasTravoisContentItem( Item ) )
					{
						ItemList.Add( Item );
					}
				}
			}
		}

        public static void MoveItemsToInventory()
        {
			Inventory Inv = GameManager.GetInventoryComponent();
			if ( Inv != null )
			{
                Il2CppSystem.Collections.Generic.List<GearItem> TravoisList = new();
                Inv.GetItems( "GEAR_Travois", TravoisList );

				foreach ( GearItem Travois in TravoisList )
				{
					Container? TravoisContainer = Travois?.GetComponent<Container>();

					while ( TravoisContainer?.m_Items.Count > 0 )
					{
						GearItem InnerItem = TravoisContainer.m_Items[ 0 ];
						TravoisContainer.RemoveGear( InnerItem, true );
						Inv.AddGear( InnerItem, false );
						TravoisContentItem.SetTravois( InnerItem, Travois );
					}
				}
			}
        }

        public static void NotifyHarvested( GearItem? Travois )
		{
			Inventory Inv = GameManager.GetInventoryComponent();
			if ( Inv != null )
			{
				Container? TravoisContainer = Travois?.GetComponent<Container>();

				while ( TravoisContainer?.m_Items.Count > 0 )
				{
					GearItem InnerItem = TravoisContainer.m_Items[ 0 ];
					TravoisContainer.RemoveGear( InnerItem, true );
					Inv.AddGear( InnerItem, false );
					TravoisContentItem.SetTravois( InnerItem, null );
				}
			}
		}

        public static void MoveItemsToTravois()
        {
			Inventory Inv = GameManager.GetInventoryComponent();
			if ( Inv != null )
			{
				Il2CppSystem.Collections.Generic.List<GearItem> ItemList = new();
                GetTravoisContentItems( Inv, ItemList );

				foreach ( GearItem Item in ItemList )
				{
					TravoisContentItem? ContentItem = Item.GetComponent<TravoisContentItem>();
					GearItem? Travois = ContentItem?.Travois;
					Container? TravoisContainer = Travois?.GetOrAddComponent<Container>();

					if ( TravoisContainer != null )
					{
						Inv.RemoveGear( Item.gameObject, true );
						TravoisContainer.AddGear( Item );
						TravoisContentItem.SetTravois( Item, null );
					}
				}
			}
        }

        public static bool AnyTravoisContains( GearItem gi )
        {
			Inventory Inv = GameManager.GetInventoryComponent();
			if ( Inv != null && gi != null )
			{
				Il2CppSystem.Collections.Generic.List<GearItem> TravoisList = new();
				Inv.GetItems( "GEAR_Travois", TravoisList );

				foreach ( GearItem Travois in TravoisList )
				{
					Container? TravoisContainer = Travois?.GetComponent<Container>();
					if ( TravoisContainer != null )
					{
						foreach ( GearItem Item in TravoisContainer.m_Items )
						{
							if ( Item != null && Item.m_InstanceID == gi.m_InstanceID )
							{
								return true;
							}
						}
					}
				}
			}
			return false;
        }
    }
}
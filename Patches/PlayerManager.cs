using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Interactions;
using UnityEngine;

namespace QualityOfLife
{

    [HarmonyPatch( typeof( PlayerManager ), "InteractiveObjectsProcess" )]
	internal class Patch_PlayerManager_InteractiveObjectsProcess
	{
        static bool Prefix( PlayerManager __instance )
		{
			if ( Settings.Instance.SeparateInteract )
			{
				float Range = __instance.ComputeModifiedPickupRange( __instance.GetDefaultPlacementDistance() );
				GameObject GameObj = __instance.GetInteractiveObjectUnderCrosshairs( Range );
				if ( GameObj != null )
				{
					IInteraction Interaction = GameObj.GetComponentInChildren<IInteraction>();
					if ( Interaction != null )
					{
						__instance.SetCurrentInteraction( Interaction );
					}
                }
				else
				{
					if ( __instance.ActiveInteraction != null && !__instance.ActiveInteraction.GetInteractiveObject().GetComponentInChildren<LoadingZone>() )
					{
						__instance.SetCurrentInteraction( null );
					}

					GameObject NearObj = __instance.GetInteractiveObjectNearCrosshairs( Range );
					__instance.IsInteractionNearCrosshair = NearObj != null;
				}

				return false;
			}
			return true;
		}
	}

	[HarmonyPatch( typeof( PlayerManager ), "UpdateActiveInteraction" )]
	internal class Patch_PlayerManager_UpdateActiveInteraction
	{
        static bool Prefix( PlayerManager __instance )
		{
			if ( Settings.Instance.SeparateInteract && __instance.ActiveInteraction != null )
			{
				if ( __instance.IsClickHoldActive() && Input.GetKeyUp( Settings.Instance.InteractKey ) )
				{
					__instance.TryCancelHoldInteraction();
				}
			}
			return true;
		}
	}

	[HarmonyPatch( typeof( PlayerManager ), "Update" )]
	internal class Patch_PlayerManager_Update
	{
        static bool Prefix( PlayerManager __instance )
		{
			if ( Settings.Instance.SeparateInteract )
			{
				__instance.m_PlaceMeshRotationDegreesPerSecond = 0;
				__instance.m_PlaceDecalRotationDegreesPerSecond = 0;

				if ( Input.GetKeyDown( Settings.Instance.PrecisionRotateKey ) )
				{
					__instance.m_PlaceMeshMouseWheelRotationDegrees = 1;
					__instance.m_PlaceDecalMouseWheelRotationDegrees = 2;
				}

				if ( Input.GetKeyUp( Settings.Instance.PrecisionRotateKey ) )
				{
					__instance.m_PlaceMeshMouseWheelRotationDegrees = 10;
					__instance.m_PlaceDecalMouseWheelRotationDegrees = 20;
				}

				if ( Input.GetKeyDown( Settings.Instance.DropKey ) )
				{
					bool InPlacementMode = __instance.IsInPlacementMode();

					if ( __instance.ActiveInteraction != null && !InPlacementMode )
					{
						GameObject ObjectToPlace = __instance.ActiveInteraction.GetInteractiveObject();
						if ( ObjectToPlace != null && ObjectToPlace.GetComponent<Inspect>() != null )
						{
							__instance.StartPlaceMesh( ObjectToPlace, PlaceMeshFlags.None );
						}
					}
					else if ( InPlacementMode )
					{
						__instance.CancelPlacementMode();
					}
				}
			}
			else
			{
                __instance.m_PlaceMeshRotationDegreesPerSecond = 60;
                __instance.m_PlaceDecalRotationDegreesPerSecond = 120;
            }

            if ( Input.GetKeyDown( Settings.Instance.NavigateKey ) )
            {
                Inventory Inv = GameManager.GetInventoryComponent();
				if ( Inv != null )
				{
					GearItem Charcoal = Inv.GetNonRuinedItem( "GEAR_Charcoal" );
					if ( Charcoal != null )
					{
						__instance.UseInventoryItem( Charcoal );
					}
				}
            }

			if ( Input.GetKeyDown( Settings.Instance.CraftingKey ) )
			{
				bool Enable = !InterfaceManager.IsPanelEnabled<Panel_Crafting>();
				if ( Enable )
				{
                    InterfaceManager.TrySetPanelEnabled<Panel_Clothing>( false );
                    InterfaceManager.TrySetPanelEnabled<Panel_FirstAid>( false );
					InterfaceManager.TrySetPanelEnabled<Panel_Inventory>( false );
					InterfaceManager.TrySetPanelEnabled<Panel_Log>( false );
                    InterfaceManager.TrySetPanelEnabled<Panel_Map>( false );

					InterfaceManager.TrySetPanelEnabled<Panel_Crafting>( true );
                }
				else
				{
					InterfaceManager.TrySetPanelEnabled<Panel_Crafting>( false );
				}
			}

			return true;
		}
	}

}

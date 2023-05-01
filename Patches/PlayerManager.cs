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
		static KeyCodeAction QuickSelectLightSource = new KeyCodeAction()
		{
			KeyCodeFunc = () => Settings.Instance.LightSourceKey,
			OnHold = () => OnLightSourceHold(),
			OnTap = () => OnLightSourceTap()
		};

		static KeyCodeAction QuickSelectWeapon = new KeyCodeAction()
		{
			KeyCodeFunc = () => Settings.Instance.WeaponKey,
			OnHold = () => OnWeaponHold(),
			OnTap = () => OnWeaponTap()
		};


        static bool Prefix( PlayerManager __instance )
		{
			if ( Settings.Instance.SeparateInteract )
			{
				__instance.m_PlaceMeshRotationDegreesPerSecond = 0;
				__instance.m_PlaceDecalRotationDegreesPerSecond = 0;

				if ( InputManager.GetKeyDown( __instance, Settings.Instance.PrecisionRotateKey ) )
				{
					__instance.m_PlaceMeshMouseWheelRotationDegrees = 1;
					__instance.m_PlaceDecalMouseWheelRotationDegrees = 2;
				}

				if ( Input.GetKeyUp( Settings.Instance.PrecisionRotateKey ) )
				{
					__instance.m_PlaceMeshMouseWheelRotationDegrees = 10;
					__instance.m_PlaceDecalMouseWheelRotationDegrees = 20;
				}

				if ( InputManager.GetKeyDown( __instance, Settings.Instance.DropKey ) )
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
					else if ( __instance.m_ItemInHands != null )
                    {
						bool bCanDropItemInHands = true;

						bCanDropItemInHands = bCanDropItemInHands && __instance.CheckIfCanDropGearItem( __instance.m_ItemInHands );
                        bCanDropItemInHands = bCanDropItemInHands && !__instance.IsInspectModeActive();
                        bCanDropItemInHands = bCanDropItemInHands && !InterfaceManager.IsPanelEnabled<Panel_Inventory>();

                        PlayerAnimation PlayerAnim = GameManager.GetPlayerAnimationComponent();
						if ( PlayerAnim != null )
						{
							PlayerAnimation.State State = PlayerAnim.GetState();
							if ( State != PlayerAnimation.State.Showing )
							{
								bCanDropItemInHands = false;
#if DEBUG
								Debug.Log( string.Format( "Can't drop item in hand during PlayerAnimation.State ({0})", State.ToString() ) );
#endif
							}
                        }

                        if ( bCanDropItemInHands )
						{
							switch ( Settings.Instance.DropItemInHands )
							{
								case DropItemType.Any:
									__instance.m_ItemInHands.Drop( 1 );
									break;

								case DropItemType.LightSource:
									if ( IsLightSource( __instance.m_ItemInHands ) )
									{
										__instance.m_ItemInHands.Drop( 1 );
									}
									break;

								case DropItemType.NonWeapons:
									if ( !__instance.m_ItemInHands.IsWeapon() )
									{
										__instance.m_ItemInHands.Drop( 1 );
									}
									break;

								case DropItemType.None:
									break;
							}
						}
					}
                }
            }
            else
			{
                __instance.m_PlaceMeshRotationDegreesPerSecond = 60;
                __instance.m_PlaceDecalRotationDegreesPerSecond = 120;
            }

			QuickSelectLightSource.Update();
			QuickSelectWeapon.Update();

            if ( InputManager.GetKeyDown( __instance, Settings.Instance.NavigateKey ) )
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

			if ( InputManager.GetKeyDown( __instance, Settings.Instance.CraftingKey ) )
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

		static void OnLightSourceHold()
		{
            Panel_ActionsRadial ActionsRadial = InterfaceManager.GetPanel<Panel_ActionsRadial>();
            if ( ActionsRadial != null )
            {
                ActionsRadial.ShowLightSourceRadial();
            }
        }

        static void OnLightSourceTap()
		{
			InputManager.ExecuteLightSourceAction();
        }

		static void OnWeaponHold()
		{
            Panel_ActionsRadial ActionsRadial = InterfaceManager.GetPanel<Panel_ActionsRadial>();
            if ( ActionsRadial != null )
            {
                ActionsRadial.ShowWeaponRadial();
            }
        }

        static void OnWeaponTap()
		{
			InputManager.ExecuteFirearmAction();
		}

        private static bool IsLightSource( GearItem Item )
        {
			if ( Item != null )
			{
				if ( Item.m_FlareItem != null || Item.m_KeroseneLampItem != null || Item.m_TorchItem != null || Item.m_MatchesItem != null )
				{
					return true;
				}
			}
			return false;
        }
    }

    [HarmonyPatch( typeof( PlayerManager ), "OnEatingComplete" )]
    internal class Patch_PlayerManager_OnEatingComplete
    {
		static bool Prefix( PlayerManager __instance, bool success, bool playerCancel, float progress )
		{
			if ( Settings.Instance.FoodEatPickUnits )
			{
				if ( __instance.m_FoodItemEaten.m_StackableItem != null && __instance.m_FoodItemEaten.m_StackableItem.m_Units > 1 )
				{
					GearItem ItemPrefab = GearItem.LoadGearItemPrefab( __instance.m_FoodItemEaten.name );

					float CaloriesPerUnit = ItemPrefab.m_FoodItem.m_CaloriesTotal;
					float ConsumedCalories = __instance.m_FoodItemEatenStartingCalories * progress;
					int ConsumedUnits = (int)Math.Floor( ConsumedCalories / CaloriesPerUnit );

					if ( progress == 1.0f )
					{
						ConsumedUnits -= 1;
					}

					__instance.m_FoodItemEaten.m_FoodItem.m_CaloriesRemaining = __instance.m_FoodItemEaten.m_FoodItem.m_CaloriesRemaining % ItemPrefab.m_FoodItem.m_CaloriesTotal;
					__instance.m_FoodItemEaten.m_FoodItem.m_CaloriesTotal = ItemPrefab.m_FoodItem.m_CaloriesTotal;

					if ( ConsumedUnits > 0 )
					{
						GameManager.GetInventoryComponent().RemoveUnits( __instance.m_FoodItemEaten, ConsumedUnits );
					}
				}
			}
            return true;
		}
    }

    [HarmonyPatch( typeof( PlayerManager ), "UseFoodInventoryItem" )]
    internal class Patch_PlayerManager_UseFoodInventoryItem
    {
        static bool Prefix( PlayerManager __instance, GearItem gi )
        {
			if ( Settings.Instance.FoodEatPickUnits )
			{
				if ( gi != null && gi.m_StackableItem != null && gi.m_StackableItem.m_Units > 1 )
				{
					Panel_PickUnits PickUnits = InterfaceManager.GetPanel<Panel_PickUnits>();
					if ( PickUnits != null )
					{
						Panel_Inventory Inventory = InterfaceManager.GetPanel<Panel_Inventory>();
						if ( Inventory != null )
						{
							if ( Inventory.IsEnabled() )
							{
								Inventory.Enable( false );
								Inventory.m_LastSelectedGearItem = gi;
								PickUnits.m_EnablePanelOnExit = EnablePanelOnExit.Inventory;
							}
							else
							{
								PickUnits.m_EnablePanelOnExit = EnablePanelOnExit.None;
							}
						}

						PickUnits.SetGearForHarvest( gi );
						PickUnits.m_numUnits = 1;
						PickUnits.m_maxUnits = gi.m_StackableItem.m_Units;
						PickUnits.Enable( true );
						PickUnits.Refresh();
					}
					return false;
				}
			}
            return true;
        }
    }

}

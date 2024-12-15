using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using Il2CppTLD.Interactions;
using Il2CppTLD.SaveState;
using UnityEngine;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( PlayerManager ), "InteractiveObjectsProcess" )]
	internal class Patch_PlayerManager_InteractiveObjectsProcess
	{
        static bool Prefix( PlayerManager __instance )
		{
			bool CatTailDisabled = !Settings.Instance.CatTailHarvestStalk && !Settings.Instance.CatTailHarvestTinder;

			if ( Settings.Instance.EnableMod && Settings.Instance.SeparateInteract || CatTailDisabled )
			{
				float Range = __instance.ComputeModifiedPickupRange( __instance.GetDefaultPlacementDistance() );
				GameObject GameObj = __instance.GetInteractiveObjectUnderCrosshairs( Range );
				if ( GameObj != null )
				{
					if ( CatTailDisabled && GameObj.name == "OBJ_CatTailShrub" )
					{
						return false;
					}

					IInteraction[] Interactions = GameObj.GetComponentsInChildren<IInteraction>();
					foreach ( IInteraction Interaction in Interactions )
					{
						if ( Interaction != null && Interaction.IsEnabled )
						{
							__instance.SetCurrentInteraction( Interaction );
							break;
						}
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

	[HarmonyPatch( typeof( PlayerManager ), "PlaceMeshInWorld" )]
    internal class Patch_PlayerManager_PlaceMeshInWorld
	{
		static bool Prefix( PlayerManager __instance )
		{
			if ( __instance.m_ObjectToPlace != null )
			{
				GearItem ItemToPlace = __instance.m_ObjectToPlaceGearItem;
				if ( ItemToPlace != null && ItemToPlace.m_FoodItem != null )
				{
					foreach ( GearItemObject GearObj in GameManager.GetInventoryComponent().m_Items )
					{
						if ( GearObj != null && GearObj.m_GearItem != null && GearObj.m_GearItem.m_InsulatedFlask != null )
						{
							if ( GearObj.m_GearItem.m_InsulatedFlask.IsItemCompatibleWithFlask( ItemToPlace ) )
							{
								GearObj.m_GearItem.m_InsulatedFlask.TryRemoveItem( ItemToPlace );
							}
						}
					}
				}
			}
			return true;
		}
	}

    [HarmonyPatch( typeof( PlayerManager ), "UpdateActiveInteraction" )]
	internal class Patch_PlayerManager_UpdateActiveInteraction
	{
        static bool Prefix( PlayerManager __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.SeparateInteract && __instance.ActiveInteraction != null )
			{
				bool bClickAndHold = !BaseStateSingleton<SettingsState>.Instance.m_DisableClickHold;

				if ( bClickAndHold && __instance.IsClickHoldActive() && ModInput.GetKeyUp( __instance, Settings.Instance.InteractKey ) )
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
			if ( !Settings.Instance.EnableMod )
			{
				return true;
			}

            bool bCanDropItemInHands = true;
			bool bDropKeyDown = ModInput.GetKeyDown( __instance, Settings.Instance.DropKey );

            if ( Settings.Instance.SeparateInteract )
			{
				__instance.m_PlaceMeshRotationDegreesPerSecond = 0;
				__instance.m_PlaceDecalRotationDegreesPerSecond = 0;

				if ( ModInput.GetKeyDown( __instance, Settings.Instance.PrecisionRotateKey ) )
				{
					__instance.m_PlaceMeshMouseWheelRotationDegrees = 1;
					__instance.m_PlaceDecalMouseWheelRotationDegrees = 2;
				}

				if ( ModInput.GetKeyUp( __instance, Settings.Instance.PrecisionRotateKey ) )
				{
					__instance.m_PlaceMeshMouseWheelRotationDegrees = 10;
					__instance.m_PlaceDecalMouseWheelRotationDegrees = 20;
				}

				if ( bDropKeyDown )
				{
					bool InPlacementMode = __instance.IsInPlacementMode();

					if ( __instance.ActiveInteraction != null && !InPlacementMode )
					{
                        GameObject ObjectToPlace = __instance.ActiveInteraction.GetInteractiveObject();
						if ( ObjectToPlace != null && ObjectToPlace.GetComponent<Inspect>() != null )
						{
							__instance.StartPlaceMesh( ObjectToPlace, PlaceMeshFlags.None );
						}
                        bCanDropItemInHands = false;
                    }
                    else if ( InPlacementMode )
					{
						__instance.CancelPlacementMode();
                        bCanDropItemInHands = false;
                    }
                }
            }
            else
			{
                __instance.m_PlaceMeshRotationDegreesPerSecond = 60;
                __instance.m_PlaceDecalRotationDegreesPerSecond = 120;
            }

			if ( bDropKeyDown && __instance.GetControlMode() == PlayerControlMode.BigCarry )
			{
				bCanDropItemInHands = false;
				InputManager.ExecuteHolsterAction();
			}

			if ( bCanDropItemInHands && bDropKeyDown && __instance.m_ItemInHands != null )
			{
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

            QuickSelectLightSource.Update();
			QuickSelectWeapon.Update();

            if ( ModInput.GetKeyDown( __instance, Settings.Instance.NavigateKey ) )
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

			if ( ModInput.GetKeyDown( __instance, Settings.Instance.ObjectivesKey ) )
			{
				WidgetUtils.ToggleMiniNavPanel<Panel_MissionsStory>();
			}

			if ( ModInput.GetKeyDown( __instance, Settings.Instance.TradesKey ) )
			{
				WidgetUtils.TogglePanelLogState( PanelLogState.Trader );
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
			if ( Settings.Instance.EnableMod && Settings.Instance.FoodEatPickUnits )
			{
				if ( __instance.m_FoodItemEaten.m_StackableItem != null && __instance.m_FoodItemEaten.m_StackableItem.m_Units > 1 )
				{
					GearItem ItemPrefab = GearItem.LoadGearItemPrefab( __instance.m_FoodItemEaten.name );
                    ItemPrefab.CacheComponents();

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
			if ( Settings.Instance.EnableMod && Settings.Instance.FoodEatPickUnits )
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
								InventoryGridDataItem ItemToSelect = new ();
								ItemToSelect.m_GearItem = gi;

								Inventory.Enable( false );
								Inventory.m_LastSelectedItem = ItemToSelect;
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

	[HarmonyPatch( typeof( PlayerManager ), "EnterInspectGearMode", new Type[] { typeof( GearItem ), typeof( Container ), typeof( IceFishingHole ), typeof( Harvestable ), typeof( CookingPotItem ) } )]
    internal class Patch_PlayerManager_EnterInspectGearMode
	{
#if false // 2024-06-26: Disabled for now until I have time to thoroughly investigate it. 
        static bool Prefix( PlayerManager __instance, GearItem gear, Container c, IceFishingHole hole, Harvestable h, CookingPotItem pot )
		{
			if ( Settings.Instance.EnableMod && ModInput.GetKey( __instance, Settings.Instance.AutoPickupKey ) )
			{
				Inventory Inv = GameManager.GetInventoryComponent();
				if ( Inv != null )
				{
					Inv.AddGear( gear, true );
					return false;
				}
			}
			return true;
		}
#endif

        static void Postfix( PlayerManager __instance, GearItem gear, Container c, IceFishingHole hole, Harvestable h, CookingPotItem pot )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				Panel_HUD HUD = InterfaceManager.GetPanel<Panel_HUD>();

				if ( HUD != null && gear != null && gear.m_Travois != null )
				{
					MeasurementUnits Units = BaseStateSingleton<SettingsState>.Instance.m_Units;

					WidgetUtils.SetActive( HUD.m_InspectMode_Equip, true );
					WidgetUtils.SetLabelText( HUD.m_InspectMode_Equip, "Transfer" );
					WidgetUtils.SetLabelText( HUD.m_InspectMode_Weight, gear.GetItemWeightKG().ToFormattedStringWithUnits() );

					if ( HUD.m_InspectMode_ButtonLayout != null )
					{
						HUD.m_InspectMode_ButtonLayout.Reposition();
					}
				}
			}
        }
    }

	[HarmonyPatch( typeof( PlayerManager ), "OnEquipFromStandardGearItemInspection" )]
	internal class Patch_PlayerManager_OnEquipFromStandardGearItemInspection
	{
		static bool Prefix( PlayerManager __instance )
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				GearItem Gear = __instance.GearItemBeingInspected();
				if ( Gear != null && Gear.m_Travois != null )
				{
					ContainerInteraction Interaction = Gear.m_Travois.BigCarryItem.m_Container.GetComponent<ContainerInteraction>();
					if ( Interaction != null )
					{
						__instance.ExitInspectGearMode( true );
						WidgetUtils.SetActive( Gear, false );
						Interaction.PerformInteraction();
					}

					return false;
				}
			}
			return true;
		}
	}

}

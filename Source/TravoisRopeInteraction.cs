using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppTLD.BigCarry;
using Il2CppTLD.Interactions;
using MelonLoader;
using UnityEngine;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class TravoisRopeInteraction : SimpleInteraction
    {
		public TravoisRopeInteraction( IntPtr ptr ) : base( ptr ) { }
		public TravoisRopeInteraction() : base( ClassInjector.DerivedConstructorPointer<TravoisContainerInteraction>() ) => ClassInjector.DerivedConstructorBody( this );

		public override void InitializeInteraction()
		{
			CanInteract = true;
			HoverText = "Mountaineering Rope";
			m_EventEntries = new Il2CppSystem.Collections.Generic.List<InteractionEventEntry>();
		}

		public override void UpdateInteraction()
		{
		}

		public override bool PerformInteraction()
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisUseWithRope )
			{
				Panel_ActionPicker ActionPicker = InterfaceManager.GetPanel<Panel_ActionPicker>();
				if ( ActionPicker != null )
				{
					ActionPickerItemData? ClimbAction = null;
					ActionPickerItemData? TravoisAction = null;
					TravoisBigCarryItem? ToDetach = GetTravoisToDetach();

					if ( ToDetach != null )
					{
						bool bPlayerAtTop = IsAtTop( this );
						bool bTravoisAtTop = IsAtTop( ToDetach );

						if ( bPlayerAtTop )
						{
							if ( bTravoisAtTop )
							{
								ClimbAction = new ActionPickerItemData( "ico_DropTravois", "Lower", (Il2CppSystem.Action)OnRopeLowerTravois );
								TravoisAction = new ActionPickerItemData( "ico_DropTravois", "Detach", (Il2CppSystem.Action)OnRopeDetachTravois );
							}
							else
							{
								ClimbAction = new ActionPickerItemData( "ico_climb", "Climb", (Il2CppSystem.Action)OnRopeClimb );
								TravoisAction = new ActionPickerItemData( "ico_DropTravois", "Raise", (Il2CppSystem.Action)OnRopeRaiseTravois );
							}
						}
						else // Player is at bottom
						{
							if ( bTravoisAtTop )
							{
								ClimbAction = new ActionPickerItemData( "ico_climb", "Climb", (Il2CppSystem.Action)OnRopeClimb );
							}
							else
							{
								ClimbAction = new ActionPickerItemData( "ico_climb", "Climb", (Il2CppSystem.Action)OnRopeClimb );
								TravoisAction = new ActionPickerItemData( "ico_DropTravois", "Detach", (Il2CppSystem.Action)OnRopeDetachTravois );
							}
						}
					}
					else if ( GetTravoisToAttach() != null )
					{
						ClimbAction = new ActionPickerItemData( "ico_climb", "Climb", (Il2CppSystem.Action)OnRopeClimb );
						TravoisAction = new ActionPickerItemData( "ico_DropTravois", "Attach", (Il2CppSystem.Action)OnRopeAttachTravois );
					}

					if ( TravoisAction != null )
					{
						ActionPicker.m_ActionPickerItemDataList.Clear();
						ActionPicker.m_ActionPickerItemDataList.Add( ClimbAction );
						ActionPicker.m_ActionPickerItemDataList.Add( TravoisAction );
						InterfaceManager.TrySetPanelEnabled<Panel_ActionPicker>( true );
					}
					else
					{
						OnRopeClimb();
					}
				}
			}
			else
			{
				OnRopeClimb();
			}
			return true;
		}

		TravoisBigCarryItem? GetTravoisToAttach()
		{
			RopeClimbPoint ClimbPoint = GetComponent<RopeClimbPoint>();
			vp_FPSPlayer Player = GameManager.GetVpFPSPlayer();

			if ( Player != null && ClimbPoint != null && ClimbPoint.m_Rope != null )
			{
				TravoisBigCarryItem AttachedTravois = ClimbPoint.m_Rope.GetComponentInChildren<TravoisBigCarryItem>( true );
				if ( AttachedTravois != null )
				{
					return null;
				}

				Collider[] Overlaps = Physics.OverlapSphere( Player.transform.position, 1.0f );
				foreach ( Collider Col in Overlaps )
				{
					TravoisBigCarryItem BigCarry = Col.GetComponent<TravoisBigCarryItem>();
					if ( BigCarry != null && BigCarry.transform.parent == null )
					{
						return BigCarry;
					}
				}
			}

			return null;
		}

		TravoisBigCarryItem? GetTravoisToDetach()
		{
			RopeClimbPoint ClimbPoint = GetComponent<RopeClimbPoint>();
			vp_FPSPlayer Player = GameManager.GetVpFPSPlayer();

			if ( Player != null && ClimbPoint != null && ClimbPoint.m_Rope != null )
			{
				TravoisBigCarryItem AttachedTravois = ClimbPoint.m_Rope.GetComponentInChildren<TravoisBigCarryItem>( true );
				if ( AttachedTravois != null )
				{
					return AttachedTravois;
				}
			}

			return null;
		}

		bool IsAtTop( Component? Comp )
		{
			if ( Comp != null )
			{
				RopeClimbPoint ClimbPoint = GetComponent<RopeClimbPoint>();
				if ( ClimbPoint != null && ClimbPoint.m_Rope != null && ClimbPoint.m_Rope.m_Spline != null )
				{
					float Param = ClimbPoint.m_Rope.m_Spline.GetClosestPointParam( Comp.transform.position, 5 );
					return Param < 0.5f;
				}
			}
			return false;
		}

		void OnRopeClimb()
		{
			RopeClimbPoint ClimbPoint = GetComponent<RopeClimbPoint>();
			if ( ClimbPoint != null )
			{
				ClimbPoint.PerformInteraction();
			}
		}

		void OnRopeAttachTravois()
		{
			RopeClimbPoint ClimbPoint = GetComponent<RopeClimbPoint>();
			if ( ClimbPoint != null && ClimbPoint.m_Rope != null )
			{
				TravoisBigCarryItem? TravoisToAttach = GetTravoisToAttach();
				if ( TravoisToAttach != null )
				{
					Panel_GenericProgressBar GenericProgress = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
					if ( GenericProgress != null )
					{
						OnExitDelegate OnExit = new Action<bool, bool, float>( ( success, playerCancel, progress ) =>
						{
							if ( success )
							{
								TravoisToAttach.transform.SetParent( ClimbPoint.m_Rope.transform, true );
								TravoisToAttach.enabled = false;
							}
						} );
						GenericProgress.Launch( "Attaching Travois...", 2.0f, 5.0f, 0.0f, false, OnExit );
					}
				}
			}
		}

		void OnRopeDetachTravois()
		{
			TravoisBigCarryItem? TravoisToDetach = GetTravoisToDetach();
			if ( TravoisToDetach != null )
			{
				Panel_GenericProgressBar GenericProgress = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
				if ( GenericProgress != null )
				{
					OnExitDelegate OnExit = new Action<bool, bool, float>( ( success, playerCancel, progress ) =>
					{
						if ( success )
						{
							TravoisToDetach.transform.SetParent( null, true );
							TravoisToDetach.enabled = true;
						}
					} );
					GenericProgress.Launch( "Detaching Travois...", 2.0f, 5.0f, 0.0f, false, OnExit );
				}
			}
		}

		void OnRopeLowerTravois()
		{
			RopeClimbPoint ClimbPoint = GetComponent<RopeClimbPoint>();
			TravoisBigCarryItem? TravoisToDetach = GetTravoisToDetach();

			if ( ClimbPoint != null && TravoisToDetach != null && ClimbPoint.m_Rope?.m_Spline?.Length > 0.0f )
			{
				Vector3 DesiredPos = RopeUtil.GetAttachPositionBottom( ClimbPoint.m_Rope );
				Quaternion DesiredQuat = Quaternion.identity;

				bool bCanDrop = TravoisToDetach.m_TravoisDropSettings.TryFindDropPositionAndAngle( DesiredPos, DesiredQuat, out Vector3 DropPos, out Quaternion DropQuat );
				if ( bCanDrop )
				{
					Panel_GenericProgressBar GenericProgress = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
					if ( GenericProgress != null )
					{
						OnExitDelegate OnExit = new Action<bool, bool, float>( ( success, playerCancel, progress ) =>
						{
							if ( success )
							{
								TravoisToDetach.transform.position = DropPos;
								TravoisToDetach.transform.rotation = DropQuat;
							}
						} );
						GenericProgress.Launch( "Lowering Travois...", 5.0f, 10.0f, 0.0f, false, OnExit );
					}
				}
				else
				{
					Panel_Confirmation Confirmation = InterfaceManager.GetPanel<Panel_Confirmation>();
					if ( Confirmation != null )
					{
						Confirmation.ShowErrorMessage( "Can't find position to drop Travois at bottom of rope." );
					}
				}
			}
		}

		void OnRopeRaiseTravois()
		{
			RopeClimbPoint ClimbPoint = GetComponent<RopeClimbPoint>();
			TravoisBigCarryItem? TravoisToDetach = GetTravoisToDetach();

			if ( ClimbPoint != null && TravoisToDetach != null && ClimbPoint.m_Rope?.m_Spline?.Length > 0.0f )
			{
				Vector3 DesiredPos = RopeUtil.GetAttachPositionTop( ClimbPoint.m_Rope );
				Quaternion DesiredQuat = Quaternion.identity;

				bool bCanDrop = TravoisToDetach.m_TravoisDropSettings.TryFindDropPositionAndAngle( DesiredPos, DesiredQuat, out Vector3 DropPos, out Quaternion DropQuat );
				if ( bCanDrop )
				{
					Panel_GenericProgressBar GenericProgress = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
					if ( GenericProgress != null )
					{
						OnExitDelegate OnExit = new Action<bool, bool, float>( ( success, playerCancel, progress ) =>
						{
							if ( success )
							{
								TravoisToDetach.transform.position = DropPos;
								TravoisToDetach.transform.rotation = DropQuat;
							}
						} );
						GenericProgress.Launch( "Raising Travois...", 5.0f, 15.0f, 0.0f, false, OnExit );
					}
				}
				else
				{
					Panel_Confirmation Confirmation = InterfaceManager.GetPanel<Panel_Confirmation>();
					if ( Confirmation != null )
					{
						Confirmation.ShowErrorMessage( "Can't find position to drop Travois at top of rope." );
					}
				}
			}
		}
	}
}

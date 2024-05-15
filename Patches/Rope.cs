using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Interactions;
using Il2CppVLB;
using UnityEngine;

namespace QualityOfLife
{
	// Do the same for RopeLedge?

    [HarmonyPatch( typeof( Rope ), "Start" )]
    internal class Patch_Rope_Start
    {
        static void Postfix( Rope __instance )
        {
			if ( Settings.Instance.EnableMod )
			{
				//RopeUtil.CreateSphere( RopeUtil.GetAttachPositionTop( __instance ) );
				//RopeUtil.CreateSphere( RopeUtil.GetAttachPositionBottom( __instance ) );

				foreach ( RopeClimbPoint ClimbPoint in __instance.GetComponentsInChildren<RopeClimbPoint>( true ) )
				{
					if ( ClimbPoint != null )
					{
						SimpleInteraction Interaction = ClimbPoint.GetComponent<SimpleInteraction>();
						if ( Interaction != null )
						{
							Interaction.enabled = false;
						}

						ClimbPoint.GetOrAddComponent<TravoisRopeInteraction>();
					}
				}
			}
        }
    }

	public static class RopeUtil
	{
		public static void CreateSphere( Vector3 Position )
		{
			GameObject Sphere = GameObject.CreatePrimitive( PrimitiveType.Sphere );
			Sphere.transform.position = Position;
			Sphere.transform.localScale = Vector3.one * 0.5f;

			Collider Col = Sphere.GetComponent<Collider>();
			if ( Col != null )
			{
				Col.enabled = false;
			}
		}

		public static Vector3 GetAttachPositionBottom( Rope InRope )
		{
			Vector3 RetVal = Vector3.zero;
			if ( InRope != null )
			{
				RetVal = InRope.m_Spline.GetPositionOnSpline( 1.0f );
				RetVal = RetVal - InRope.transform.forward * 2 + Vector3.up * 6;

				RaycastHit HitInfo;
				Physics.Raycast( RetVal, Vector3.down, out HitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore );
				RetVal = HitInfo.point + Vector3.up * 0.25f;
			}
			return RetVal;
		}

		public static Vector3 GetAttachPositionTop( Rope InRope )
		{
			Vector3 RetVal = Vector3.zero;
			if ( InRope != null )
			{
				RetVal = InRope.m_Spline.GetPositionOnSpline( 3.0f / InRope.m_Spline.Length );
			}
			return RetVal;
		}
	}
}

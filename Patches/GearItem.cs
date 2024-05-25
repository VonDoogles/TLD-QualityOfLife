﻿using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife.Patches
{
	[HarmonyPatch( typeof( GearItem ), "Drop" )]
	internal class Patch_GearItem_Drop
	{
		static void Postfix( GearItem __instance, int numUnits, bool playSound, bool stickToFeet, bool force )
		{
			MeatScale.TryAdd( __instance );
		}
	}

	[HarmonyPatch( typeof( GearItem ), "CacheComponents" )]
	internal class Patch_GearItem_CacheComponents
	{
		static void Postfix( GearItem __instance )
		{
			MeatScale.TryAdd( __instance );
		}
	}

    [HarmonyPatch( typeof( GearItem ), "StickToGroundAtPlayerFeet" )]
    internal class Patch_GearItem_StickToGroundAtPlayerFeet
    {
        static void Postfix( GearItem __instance, Vector3 pos )
        {
            if ( __instance != null && Settings.Instance.EnableMod && !Settings.Instance.StickNorth )
            {
                GameObject GameObj = __instance.GetInteractiveObject();
                if ( GameObj != null )
                {
                    float Angle = UnityEngine.Random.value * 360;
                    Vector3 Up = GameObj.transform.up;
                    Quaternion Rot = Quaternion.AngleAxis( Angle, Up ) * GameObj.transform.rotation;
                    GameObj.transform.rotation = Rot;
                }
            }
        }
    }

    [HarmonyPatch( typeof( GearItem ), "GetItemWeightKG", new Type[] { typeof( bool ) } )]
    internal class Patch_GearItem_GetItemWeightKG
    {
        static void Postfix( GearItem __instance, bool ignoreClothingBonus, ref float __result )
        {
			if ( Settings.Instance.EnableMod && Settings.Instance.TravoisPickupWithContents )
			{
				if ( __instance != null && __instance.m_Travois != null )
				{
					Container? GearContainer = null;

					if ( __instance.m_Travois.BigCarryItem != null )
					{
						GearContainer = __instance.m_Travois.BigCarryItem.m_Container;
					}
					else
					{
						GearContainer = __instance.gameObject.GetComponent<Container>();
					}

					if ( GearContainer != null )
					{
						__result += GearContainer.GetTotalWeightKG();
					}
				}
			}
        }
    }
}

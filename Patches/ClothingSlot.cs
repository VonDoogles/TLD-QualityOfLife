using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace QualityOfLife
{
	[HarmonyPatch( typeof( ClothingSlot ), "UpdateSlotInfo" )]
    internal class Patch_ClothingSlot_UpdateSlotInfo
    {
		static void Postfix( ClothingSlot __instance )
		{
			UIProgressBar? ConditionFill = __instance.transform.FindChild( "ConditionFill" )?.GetComponent<UIProgressBar>();
			if ( ConditionFill == null && Settings.Instance.ClothingConditionBars )
			{
				ConditionFill = WidgetUtils.MakeProgressBar( "ConditionFill", __instance.transform, 60 );
			}

			if ( ConditionFill != null )
			{
				ConditionFill.transform.localPosition = new Vector3( 8, -30, 0 );
				ConditionFill.mFG.transform.localPosition = new Vector3( -38, 0, 0 );

				if ( __instance.m_GearItem != null && Settings.Instance.ClothingConditionBars )
				{
					Color FillColor = __instance.m_GearItem.GetColorBasedOnCondition();
					FillColor.a = 0.65f;
					ConditionFill.mFG.color = FillColor;
					ConditionFill.value = __instance.m_GearItem.GetNormalizedCondition();
					WidgetUtils.SetActive( ConditionFill, true );
				}
				else
				{
					WidgetUtils.SetActive( ConditionFill, false );
				}
			}
		}
    }
}

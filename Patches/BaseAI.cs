using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.IntBackedUnit;

namespace QualityOfLife
{
    [HarmonyPatch( typeof( BaseAi ), "EnterDead" )]
    internal class Patch_BaseAI_EnterDead
    {
        private static bool GetMeatSettings( AiSubType SubType, out float MeatMin, out float MeatMax )
        {
            MeatMin = -1.0f;
            MeatMax = -1.0f;

            switch ( SubType )
            {
                case AiSubType.Bear:
                    MeatMin = Settings.Instance.BearMeatMinKG;
                    MeatMax = Settings.Instance.BearMeatMaxKG;
                    return true;

                case AiSubType.Moose:
                    MeatMin = Settings.Instance.MooseMeatMinKG;
                    MeatMax = Settings.Instance.MooseMeatMaxKG;
                    return true;

                case AiSubType.Stag:
                    MeatMin = Settings.Instance.DeerMeatMinKG;
                    MeatMax = Settings.Instance.DeerMeatMaxKG;
                    return true;

                case AiSubType.Wolf:
                    MeatMin = Settings.Instance.WolfMeatMinKG;
                    MeatMax = Settings.Instance.WolfMeatMaxKG;
                    return true;
            }

            return false;
        }

		static bool ShouldRerollMeatAvailable( BodyHarvest Body, float MeatMin, float MeatMax )
		{
			// Only reroll when settings are different from unmodded values.
			if ( Body.m_MeatAvailableMin.ToQuantity( 1.0f ) != MeatMin && Body.m_MeatAvailableMax.ToQuantity( 1.0f ) != MeatMax )
			{
				// Only reroll when current amount is outside the settings range.
				return ( Body.m_MeatAvailableKG.ToQuantity( 1.0f ) < MeatMin || Body.m_MeatAvailableKG.ToQuantity( 1.0f ) > MeatMax );
			}
			return false;
		}

        static bool Prefix( BaseAi __instance )
        {
            if ( Settings.Instance.EnableMod )
            {
                // Check m_MeatAvailableMaxKG > 0.0f to skip starving wolves from Zone of Contamination.
                if ( __instance.m_BodyHarvest != null && !__instance.m_BodyHarvest.m_HasHarvested && __instance.m_BodyHarvest.m_MeatAvailableMax.ToQuantity( 1.0f ) > 0.0f )
                {
                    if ( GetMeatSettings( __instance.m_AiSubType, out float MeatMin, out float MeatMax  ) )
                    {
						if ( ShouldRerollMeatAvailable( __instance.m_BodyHarvest, MeatMin, MeatMax ) )
						{
							float MeatKG = UnityEngine.Random.Range( MeatMin, MeatMax );
                            __instance.m_BodyHarvest.m_MeatAvailableKG = ItemWeight.FromKilograms( MeatKG );
                        }
                    }
                }
            }
            return true;
        }
    }
}

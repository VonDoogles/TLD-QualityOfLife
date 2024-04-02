using Il2Cpp;

namespace QualityOfLife
{
    public static class CatTailHelper
	{
		static float CatTailCaloriesDefault = 0.0f;


		static float GetCatTailCalories( FoodItem Food )
		{
			if ( Food != null && CatTailCaloriesDefault == 0.0f )
			{
				CatTailCaloriesDefault = Food.m_CaloriesTotal;
			}

			if ( Settings.Instance.EnableMod )
			{
				switch ( Settings.Instance.CatTailCalories )
				{
					case CatTailCalorieType.OneThird:	return CatTailCaloriesDefault * 1 / 3;
					case CatTailCalorieType.TwoThird:	return CatTailCaloriesDefault * 2 / 3;
					default: break;
				}
			}

			return CatTailCaloriesDefault;
		}

		public static void TryUpdateCalories( FoodItem Food )
		{
			if ( Food != null )
			{
				float Remaining = Food.m_CaloriesRemaining;
				float CurrentTotal = Food.m_CaloriesTotal;
				float DesiredTotal = GetCatTailCalories( Food );

				if ( CurrentTotal != DesiredTotal )
				{
					float RemainingPct = Remaining / CurrentTotal;

					Food.m_CaloriesRemaining = RemainingPct * DesiredTotal;
					Food.m_CaloriesTotal = DesiredTotal;
				}
			}
		}

		public static void TryUpdateInventory()
		{
			Inventory Inv = GameManager.GetInventoryComponent();
			if ( Inv != null )
			{
				Il2CppSystem.Collections.Generic.List<GearItem> CatTailItems = new();
				Inv.GearInInventory( "GEAR_CattailStalk", CatTailItems );

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
}

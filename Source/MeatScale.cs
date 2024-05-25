using Il2Cpp;
using Il2CppVLB;
using MelonLoader;
using UnityEngine;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class MeatScale : MonoBehaviour
    {
		const float OneThird = 1.0f / 3.0f;
		const float TwoThird = 2.0f / 3.0f;

		public float FoodScale = 1.0f;

		public GearItem? Gear = null;


		public static void TryAdd( GearItem Gear )
		{
			if ( Settings.Instance.EnableMod )
			{
				FoodItem? Food = Gear?.m_FoodItem;
				if ( Food != null )
				{
					bool bShouldAdd = Food.m_IsMeat || Food.m_IsFish;
					if ( bShouldAdd )
					{
						Gear.GetOrAddComponent<MeatScale>();
					}
				}
			}
		}

		public void Start()
		{
			Gear = GetComponent<GearItem>();
		}

		public void Update()
		{
			float DesiredScale = GetFoodItemScale( Gear?.m_FoodItem );
			if ( DesiredScale != FoodScale )
			{
				UpdateScale( DesiredScale );
			}
		}

		public void UpdateScale( float DesiredScale )
		{
			FoodScale = DesiredScale;

			if ( Gear != null )
			{
				Vector3 MeshScale = Vector3.one * DesiredScale;

				foreach ( MeshRenderer Mesh in Gear.m_MeshRenderers )
				{
					Mesh.transform.localScale = MeshScale;
				}
			}
		}

		public float GetFoodItemScale( FoodItem? Food )
		{
			float DesiredScale = 1.0f;

			if ( Food != null )
			{
				if ( Settings.Instance.EnableMod && ( Settings.Instance.FoodScaleMeatOneThird != 0.0f ||
													  Settings.Instance.FoodScaleMeatTwoThird != 0.0f ) )
				{
					float CaloriePct = 0.0f;

					if ( Food.m_CaloriesTotal > 0.0f )
					{
						CaloriePct = 100.0f * ( Food.m_CaloriesRemaining / Food.m_CaloriesTotal );
					}

					if ( CaloriePct < Settings.Instance.FoodScaleMeatOneThird )
					{
						DesiredScale = OneThird;
					}
					else if ( CaloriePct < Settings.Instance.FoodScaleMeatTwoThird )
					{
						DesiredScale = TwoThird;
					}
				}
			}

			return DesiredScale;
		}
    }
}

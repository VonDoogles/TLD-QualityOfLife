using Il2Cpp;
using Il2CppTLD.Gear;
using Il2CppTLD.IntBackedUnit;
using Il2CppTLD.SaveState;
using MelonLoader;
using UnityEngine;

namespace QualityOfLife
{
    [RegisterTypeInIl2Cpp]
    public class RefuelPanel : MonoBehaviour
    {
		public UISprite? FuelSupply_BackgroundDec;
		public Transform? FuelSupply_ButtonDec;

		public UISprite? FuelSupply_BackgroundInc;
		public Transform? FuelSupply_ButtonInc;

		public Transform? FuelSupply_TextureTrans;
		public UITexture? FuelSupply_Texture;

		public Transform? FuelTarget_TextureTrans;
		public UITexture? FuelTarget_Texture;

		public int FuelIndex;
		public GearItem? FuelItem;
		public Il2CppSystem.Collections.Generic.List<GearItem> FuelList = new();

		public GearItem? ExamineItem;

		public bool IsRefueling { get; private set; }


		public void Awake()
		{
			FuelSupply_TextureTrans = transform.FindChild( "FuelDisplay/FuelSupply_Texture" );
			FuelSupply_Texture = FuelSupply_TextureTrans.GetComponent<UITexture>();

			FuelTarget_TextureTrans = transform.FindChild( "FuelDisplay/Lantern_Texture" );
			FuelTarget_Texture = FuelTarget_TextureTrans.GetComponent<UITexture>();

			Panel_Rest Rest = InterfaceManager.GetPanel<Panel_Rest>();

			if ( FuelSupply_TextureTrans != null && Rest != null )
			{
				Vector3 ButtonScale = new Vector3( 0.7f, 0.7f, 1.0f );

				FuelSupply_BackgroundDec = WidgetUtils.MakeSprite( "Background", FuelSupply_TextureTrans, new Vector3( -64, 0, 0 ), "arrow_nav2_back" );
				if ( FuelSupply_BackgroundDec != null )
				{
					FuelSupply_BackgroundDec.transform.localScale = ButtonScale;
				}

				FuelSupply_BackgroundInc = WidgetUtils.MakeSprite( "Background", FuelSupply_TextureTrans, new Vector3(  64, 0, 0 ), "arrow_nav3_back" );
				if ( FuelSupply_BackgroundInc != null )
				{
					FuelSupply_BackgroundInc.transform.localScale = ButtonScale;
				}

				FuelSupply_ButtonDec = GameObject.Instantiate( Rest.m_ButtonDecrease )?.transform;
				if ( FuelSupply_ButtonDec != null )
				{
					FuelSupply_ButtonDec.gameObject.name = "FuelSupply_ButtonDec";
					FuelSupply_ButtonDec.parent = FuelSupply_TextureTrans;
					FuelSupply_ButtonDec.localPosition = new Vector3( -64, 0, 0 );
					FuelSupply_ButtonDec.localScale = ButtonScale;

					UIButton? Button = FuelSupply_ButtonDec.GetComponent<UIButton>();
					if ( Button != null )
					{
						Button.onClick.Clear();
						Button.onClick.Add( new EventDelegate( (EventDelegate.Callback)OnDecrease ) );
					}
				}

				FuelSupply_ButtonInc = GameObject.Instantiate( Rest.m_ButtonIncrease )?.transform;
				if ( FuelSupply_ButtonInc != null )
				{
					FuelSupply_ButtonInc.gameObject.name = "FuelSupply_ButtonInc";
					FuelSupply_ButtonInc.parent = FuelSupply_TextureTrans;
					FuelSupply_ButtonInc.localPosition = new Vector3( 64, 0, 0 );
					FuelSupply_ButtonInc.localScale = ButtonScale;

					UIButton? Button = FuelSupply_ButtonInc.GetComponent<UIButton>();
					if ( Button != null )
					{
						Button.onClick.Clear();
						Button.onClick.Add( new EventDelegate( (EventDelegate.Callback)OnIncrease ) );
					}
				}
			}

			IsRefueling = false;
		}

		void OnEnable()
		{
			IsRefueling = false;

			if ( Settings.Instance.EnableMod && Settings.Instance.FuelSelectSource )
			{
				WidgetUtils.SetActive( FuelSupply_BackgroundDec, true );
				WidgetUtils.SetActive( FuelSupply_ButtonDec, true );
				WidgetUtils.SetActive( FuelSupply_BackgroundInc, true );
				WidgetUtils.SetActive( FuelSupply_ButtonInc, true );

				Panel_Inventory_Examine? Examine = InterfaceManager.GetPanel<Panel_Inventory_Examine>();
				ExamineItem = Examine?.m_GearItem;

				Inventory Inv = GameManager.GetInventoryComponent();
				if ( Inv != null )
				{
					Inv.GetGearItems( (Il2CppSystem.Predicate<GearItem>)IsFuelItem, FuelList );
				}

				FuelIndex = ( FuelList.Count > 0 ) ? 0 : -1;
				RefreshFuel();
			}
			else
			{
				WidgetUtils.SetActive( FuelSupply_BackgroundDec, false );
				WidgetUtils.SetActive( FuelSupply_ButtonDec, false );
				WidgetUtils.SetActive( FuelSupply_BackgroundInc, false );
				WidgetUtils.SetActive( FuelSupply_ButtonInc, false );

				if ( FuelTarget_Texture != null )
				{
					FuelTarget_Texture.mainTexture = Utils.GetInventoryIconTextureFromPrefabName( "GEAR_KeroseneLampB" );
				}

				if ( FuelSupply_Texture != null )
				{
					FuelSupply_Texture.mainTexture = Utils.GetInventoryIconTextureFromPrefabName( "GEAR_JerrycanRusty" );
				}
			}
		}

		void OnDecrease()
		{
			if ( !IsRefueling )
			{
				FuelIndex = Mathf.Max( 0, FuelIndex - 1 );
				RefreshFuel();
			}
		}

		void OnIncrease()
		{
			if ( !IsRefueling )
			{
				FuelIndex = Mathf.Min( FuelList.Count - 1, FuelIndex + 1 );
				RefreshFuel();
			}
		}

		public void OnRefuel()
		{
            ItemLiquidVolume FuelAvailable = GetFuelAvailable();
            ItemLiquidVolume MaxFuelToAdd = GetMaxFuelToAdd();

			if ( FuelAvailable > ItemLiquidVolume.Zero && MaxFuelToAdd > ItemLiquidVolume.Zero )
			{
				Panel_GenericProgressBar GenericProgress = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
				if ( GenericProgress != null )
				{
					IsRefueling = true;
					OnExitDelegate OnExit = new Action<bool, bool, float>( OnRefuelFinished );
					GenericProgress.Launch( "Refueling...", 3.0f, 0.0f, 0.0f, true, OnExit );
				}
			}
		}

		void OnRefuelFinished( bool Success, bool PlayerCancel, float Progress )
		{
			if ( ExamineItem != null && FuelItem != null )
			{
                ItemLiquidVolume MaxFuelToAdd = GetMaxFuelToAdd();
                ItemLiquidVolume FuelToTransfer = ItemLiquidVolume.Min( MaxFuelToAdd, GetFuelAvailable() * Progress );

				if ( FuelToTransfer > ItemLiquidVolume.Zero )
				{
					// Remove from source
					if ( FuelItem.m_LiquidItem != null )
					{
						FuelItem.m_LiquidItem.RemoveLiquid( FuelToTransfer, out float HealthPct );
					}
					else if ( FuelItem.m_KeroseneLampItem != null )
					{
						FuelItem.m_KeroseneLampItem.m_CurrentFuelLiters = FuelItem.m_KeroseneLampItem.m_CurrentFuelLiters - FuelToTransfer;
					}

					// Add to target
					if ( ExamineItem.m_LiquidItem != null )
					{
						ExamineItem.m_LiquidItem.m_Liquid = ExamineItem.m_LiquidItem.m_Liquid + FuelToTransfer;
					}
					else if ( ExamineItem.m_KeroseneLampItem != null )
					{
						ExamineItem.m_KeroseneLampItem.AddFuel( FuelToTransfer );
					}
				}

				if ( GetFuelAvailable() <= ItemLiquidVolume.Zero )
				{
					FuelList.Remove( FuelItem );
				}
			}

			RefreshFuel();
			IsRefueling = false;
		}

        ItemLiquidVolume GetFuelAvailable()
		{
			if ( FuelItem != null )
			{
				if ( FuelItem.m_LiquidItem != null )
				{
					return FuelItem.m_LiquidItem.GetVolumeLitres();
				}
				else if ( FuelItem.m_KeroseneLampItem != null )
				{
					return FuelItem.m_KeroseneLampItem.m_CurrentFuelLiters;
				}
			}
			return ItemLiquidVolume.Zero;
		}

        ItemLiquidVolume GetFuelCapacity()
		{
			if ( FuelItem != null )
			{
				if ( FuelItem.m_LiquidItem != null )
				{
					return FuelItem.m_LiquidItem.GetCapacityLitres();
				}
				else if ( FuelItem.m_KeroseneLampItem != null )
				{
					return FuelItem.m_KeroseneLampItem.m_MaxFuel;
				}
			}
			return ItemLiquidVolume.Zero;
		}

        ItemLiquidVolume GetMaxFuelToAdd()
		{
			if ( ExamineItem != null )
			{
				if ( ExamineItem.m_LiquidItem != null )
				{
					return ExamineItem.m_LiquidItem.GetCapacityLitres() - ExamineItem.m_LiquidItem.GetVolumeLitres();
				}
				else if ( ExamineItem.m_KeroseneLampItem != null )
				{
					return ExamineItem.m_KeroseneLampItem.GetMaxFuelToAdd();
				}
			}
			return ItemLiquidVolume.Zero;
		}

        ItemLiquidVolume GetTargetFuel()
		{
			if ( ExamineItem != null )
			{
				if ( ExamineItem.m_LiquidItem != null )
				{
					return ExamineItem.m_LiquidItem.GetVolumeLitres();
				}
				else if ( ExamineItem.m_KeroseneLampItem != null )
				{
					return ExamineItem.m_KeroseneLampItem.m_CurrentFuelLiters;
				}
			}
			return ItemLiquidVolume.Zero;
		}

        ItemLiquidVolume GetTargetCapacity()
		{
			if ( ExamineItem != null )
			{
				if ( ExamineItem.m_LiquidItem != null )
				{
					return ExamineItem.m_LiquidItem.GetCapacityLitres();
				}
				else if ( ExamineItem.m_KeroseneLampItem != null )
				{
					return ExamineItem.m_KeroseneLampItem.m_MaxFuel;
				}
			}
			return ItemLiquidVolume.Zero;
		}

		void RefreshFuel()
		{
			FuelIndex = Mathf.Min( FuelIndex, FuelList.Count - 1 );

			if ( FuelIndex >= 0 && FuelIndex < FuelList.Count )
			{
				FuelItem = FuelList[ FuelIndex ];
				WidgetUtils.SetActive( FuelSupply_ButtonDec, FuelIndex > 0 );
				WidgetUtils.SetActive( FuelSupply_ButtonInc, FuelIndex < ( FuelList.Count - 1 ) );
			}
			else
			{
				FuelItem = null;
				WidgetUtils.SetActive( FuelSupply_ButtonDec, false );
				WidgetUtils.SetActive( FuelSupply_ButtonInc, false );
			}

			Panel_Inventory_Examine? Examine = InterfaceManager.GetPanel<Panel_Inventory_Examine>();
			if ( Examine != null )
			{
				if ( Examine.m_LanternFuelAmountLabel != null && ExamineItem != null && ExamineItem.m_KeroseneLampItem )
				{
					string LitersCur = ExamineItem.m_KeroseneLampItem.m_CurrentFuelLiters.ToString();
					string LitersMax = ExamineItem.m_KeroseneLampItem.m_MaxFuel.ToFormattedStringWithUnits();
					Examine.m_LanternFuelAmountLabel.text = $"{LitersCur}/{LitersMax}";
				}

				if ( FuelTarget_Texture != null )
				{
					if ( ExamineItem != null )
					{
						FuelTarget_Texture.mainTexture = Utils.GetInventoryIconTextureFromPrefabName( ExamineItem.name );
					}
					else
					{
						FuelTarget_Texture.mainTexture = Utils.GetInventoryIconTextureFromPrefabName( "GEAR_KeroseneLampB" );
					}
				}

				if ( Examine.m_LanternFuelAmountLabel != null )
				{
					string LitersCur = GetTargetFuel().ToString();
					string LitersMax = GetTargetCapacity().ToFormattedStringWithUnits();
					Examine.m_LanternFuelAmountLabel.text = $"{LitersCur}/{LitersMax}";
				}

				if ( Examine.m_FuelSupplyAmountLabel != null )
				{
					string LitersCur = GetFuelAvailable().ToString();
					string LitersMax = GetFuelCapacity().ToFormattedStringWithUnits();

                    Examine.m_FuelSupplyAmountLabel.text = $"{LitersCur}/{LitersMax}";
				}

				WidgetUtils.SetActive( Examine.m_RequiresFuelMessage, GetFuelAvailable() <= ItemLiquidVolume.Zero );
			}

			if ( FuelSupply_Texture != null )
			{
				if ( FuelItem != null )
				{
					FuelSupply_Texture.mainTexture = Utils.GetInventoryIconTextureFromPrefabName( FuelItem.name );
				}
				else
				{
					FuelSupply_Texture.mainTexture = Utils.GetInventoryIconTextureFromPrefabName( "GEAR_JerrycanRusty" );
				}
			}
		}

		void Update()
		{
			if ( Settings.Instance.EnableMod && Settings.Instance.FuelSelectSource )
			{
				Panel_Inventory_Examine? Examine = InterfaceManager.GetPanel<Panel_Inventory_Examine>();

				if ( ModInput.GetKeyDown( Examine, KeyCode.A ) )
				{
					OnDecrease();
				}
				else if ( ModInput.GetKeyDown( Examine, KeyCode.D ) )
				{
					OnIncrease();
				}
			}
		}

		bool IsFuelItem( GearItem Gear )
		{
			if ( Gear != null && Gear != ExamineItem )
			{
				if ( Gear.m_KeroseneLampItem != null && Gear.m_KeroseneLampItem.m_CurrentFuelLiters > ItemLiquidVolume.Zero )
				{
					return true;
				}
				else if ( Gear.m_LiquidItem != null && Gear.m_LiquidItem.LiquidType == LiquidType.GetKerosene() && Gear.m_LiquidItem.m_Liquid > ItemLiquidVolume.Zero )
				{
					return true;
				}
			}
			return false;
		}
    }
}

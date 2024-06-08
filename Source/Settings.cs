using Il2Cpp;
using ModSettings;
using QualityOfLife.Patches;
using System.Reflection;
using UnityEngine;

namespace QualityOfLife
{
	internal enum CatTailCalorieType
	{
		// Cat Tails have 1/3 the normal calories. (50)
		OneThird = 0,
		// Cat Tails have 2/3 the normal calories. (100)
		TwoThird,
		// Cat Tails have the normal amount of calories. (150).
		Default
	}

    internal enum DropItemType
    {
        // Disable dropping item in hand.
        None = 0,
        // Allow dropping any item in hand.
        Any,
        // Only drop light source items in hand.
        LightSource,
        // Allow dropping any item in hand that isn't a weapon.
        NonWeapons
    }

	internal enum ItemDropRotationType
	{
		// Use base game functionality.
		StickNorth = 0,
		// Drop items facing the same direction as the player.
		PlayerFacing,
		// Drop items facing a random diration.
		Random
	}

    internal enum WeightLabelType
    {
        // Weight Label is disabled.
        None = 0,
        // Weight Label is always visible.
        AlwaysOn,
        // Weight Label auto fades with the other hud elements.
        AutoHide
    }

    internal enum WindStatusType
    {
        // Wind StatusBar is disabled.
        None = 0,
        // Wind StatusBar only shows direction.
        DirectionOnly,
        // Wind StatusBar shows both direction and speed.
        DirectionAndSpeed,
		// Wind StatusBar only shows the shelter status icon.
		ShelterOnly,
    }

    internal class Settings : JsonModSettings
    {
        [Section( "Mod" )]
        [Name( "Enable Mod" )]
        [Description( "Enable / Disable Mod.  Allows turning off all features without changing individual settings." )]
        public bool EnableMod = true;


		[Section( "Animation" )]
		[Name( "Play Harvest Anims" )]
		[Description( "When true, plays the new harvest anims, when false uses the old progress bar." )]
		public bool AnimPlayHarvest = true;


        [Section( "Crafting" )]
        [Name( "Remember Filter" )]
        [Description( "Remembers which crafting filter was selected, and restores it when the crafting UI is opened." )]
        public bool CraftRememberFilter = true;


        [Section( "Difficulty" )]
		[Name( "Allow Take Torch" )]
		[Description( "Allows taking torchs from lit fires." )]
		public bool FireAllowTakeTortch = true;

		[Name( "Cat Tail Calories" )]
		[Description( "The amount of calories Cat Tail stalks have.\n"+
					  "OneThird - 1/3 the normal calories. (50)\n"+
					  "TwoThird - 2/3 the normal calories. (100)\n"+
					  "Default - the normal amount of calories. (150)"+
					  "*Note*\n"+
					  "Calorie count for inventory is saved. "+
					  "Before removing/disabling this mod, set Cat Tail Calories back to Default and save the game." )]
		public CatTailCalorieType CatTailCalories = CatTailCalorieType.Default;

		[Name( "Cat Tail Harvest Stalk" )]
		[Description( "Enable/Disable harvesting Cat Tail stalks." )]
		public bool CatTailHarvestStalk = true;

		[Name( "Cat Tail Harvest Tinder" )]
		[Description( "Enable/Disable harvesting Cat Tail tinder." )]
		public bool CatTailHarvestTinder = true;

		[Name( "Item Drop Rotation" )]
		[Description( "The rotation of items when dropped.\n"+
					  "StickNorth - Use base game behaviour.\n"+
					  "PlayerFacing - Drop items facing the same direction as the player.\n"+
					  "Random - Drop items facing a random diration." )]
		public ItemDropRotationType ItemDropRotation = ItemDropRotationType.PlayerFacing;

		[Name( "Bear Meat Min (KG)" )]
		[Description( "The minimum amount of meat in KG a bear carcas will have. (Default: 25)" )]
		[Slider(0f, 25f, 51)]
		public float BearMeatMinKG = 25.0f;

		[Name( "Bear Meat Max (KG)" )]
		[Description( "The maximum amount of meat in KG a bear carcas will have. (Default: 40)" )]
		[Slider(0f, 40f, 81)]
		public float BearMeatMaxKG = 40.0f;

		[Name( "Deer Meat Min (KG)" )]
		[Description( "The minimum amount of meat in KG a deer carcas will have. (Default: 8)" )]
		[Slider(0f, 8f, 17)]
		public float DeerMeatMinKG = 8.0f;

		[Name( "Deer Meat Max (KG)" )]
		[Description( "The maximum amount of meat in KG a deer carcas will have. (Default: 12.5)" )]
		[Slider(0f, 12.5f, 26)]
		public float DeerMeatMaxKG = 12.5f;

		[Name( "Moose Meat Min (KG)" )]
		[Description( "The minimum amount of meat in KG a moose carcas will have. (Default: 30)" )]
		[Slider(0f, 30f, 61)]
		public float MooseMeatMinKG = 30.0f;

		[Name( "Moose Meat Max (KG)" )]
		[Description( "The maximum amount of meat in KG a moose carcas will have. (Default: 45)" )]
		[Slider(0f, 45f, 91)]
		public float MooseMeatMaxKG = 45.0f;

		[Name( "Wolf Meat Min (KG)" )]
		[Description( "The minimum amount of meat in KG a wolf carcas will have. (Default: 3)" )]
		[Slider(0f, 3f, 7)]
		public float WolfMeatMinKG = 3.0f;

		[Name( "Wolf Meat Max (KG)" )]
		[Description( "The maximum amount of meat in KG a wolf carcas will have. (Default: 6)" )]
		[Slider(0f, 6f, 13)]
		public float WolfMeatMaxKG = 6.0f;


        [Section( "Food" )]
        [Name( "Cook Filter Reheat" )]
        [Description( "Filter reheatable items in the Cook UI." )]
        public bool FoodCookFilterReheat = true;

        [Name( "Eat Pick Units" )]
        [Description( "Show the Pick Units UI to select how many stackable food items to eat." )]
        public bool FoodEatPickUnits = true;

		[Name( "Scale Meat 1/3 Size" )]
		[Description( "Meat on the ground that has less than this percent of full calories is scaled to 1/3 size." )]
		[Slider(0f, 100f, 101)]
		public float FoodScaleMeatOneThird = 50.0f;

		[Name( "Scale Meat 2/3 Size" )]
		[Description( "Meat on the ground that has less than this percent of full calories is scaled to 2/3 size." )]
		[Slider(0f, 100f, 101)]
		public float FoodScaleMeatTwoThird = 100.0f;


        [Section( "Fire" )]
        [Name( "Auto-Select Lit Torch" )]
        [Description( "Auto selects the torch as the fire starter if the player is holding a lit torch." )]
        public bool FireAutoSelectLitTorch = true;

        [Name( "Charcoal Pick Units" )]
        [Description( "Show the Pick Units UI to select how much charcoal to take." )]
        public bool FireCharcoalPickUnits = true;

        [Name( "Auto-Select Mag Lens" )]
        [Description( "If the the Mag Lens is able to be used, then auto select it.  A lit torch takes precedence if 'Auto-Select Lit Torch' is enabled." )]
        public bool FireAutoSelectMagLens = true;

        [Name( "Auto-Skip Birch Bark" )]
        [Description( "Auto selects next tinder type when Fire Start UI is opened.  This prevents accidental usage of Birch Bark as tinder.\nIf tinder is required and Birch Bark is the only tinder available, this does nothing as the next item is still Birch Bark." )]
        public bool FireAutoSkipBirchBark = true;

        [Name( "Remember Selection" )]
        [Description( "Remember previous selected items and auto select them.  Above options take precedence is enabled." )]
        public bool FireRememberSelection = true;


		[Section( "Fishing" )]
		[Name( "Remember Selection" )]
		[Description( "Remember previous selected items and auto select them." )]
		public bool FishingRememberSelection = true;

		[Name( "Show Remaining Bait" )]
		[Description( "Show remaining bait on fishing UI." )]
		public bool FishingShowRemainingBait = true;


		[Section( "Fuel" )]
		[Name( "Select Source" )]
		[Description( "When enabled, allows choosing which fuel source to use when refueling." )]
		public bool FuelSelectSource = true;


        [Section( "Input" )]
        [Name( "Separate Interact" )]
        [Description( "Separate interact with objects so it doesn't share the same controls as shooting." )]
        public bool SeparateInteract = true;

        [Name( "Highlight Selection" )]
        [Description( "Changes the background color of items on the Harvest, FireStart and IceFishing UI to show which row has keyboard focus." )]
        public bool HighlightSelection = true;

        [Name( "Rest Preset" )]
        [Description( "Adds a preset button for 10 hours to the rest UI." )]
        public bool RestPreset = true;

        [Name( "UI Extra Controls" )]
        [Description( "Adds extra shortcut keys to various UI. (See README.md)" )]
        public bool UIExtraControls = true;

        [Name( "Drop Item in Hands" )]
        [Description( "Which type of items held in player's hands can be dropped with the Drop key. (None, Any, LightSource, NonWeapons)" )]
        public DropItemType DropItemInHands = DropItemType.Any;

        [Name( "QuickSelect Hold Duration" )]
        [Description( "Hold long QuickSelect Key must be held down to show Radial UI. (Default: 0.25)" )]
        public float QuickSelectHoldDuration = 0.25f;

        [Name( "Accept / Interact" )]
        [Description( "The key for accepting menu choices and interacting with objects in the world." )]
        public KeyCode InteractKey = KeyCode.E;

        [Name( "Drop / Put Back" )]
        [Description( "The key for dropping items or putting them back while inpecting them." )]
        public KeyCode DropKey = KeyCode.Q;

        [Name( "Equip / Consume" )]
        [Description( "The key equipping items or consuming them." )]
        public KeyCode EquipKey = KeyCode.Space;

        [Name( "Precision Rotate" )]
        [Description( "When this key is held, rotating the placement object with mouse wheel is more precise." )]
        public KeyCode PrecisionRotateKey = KeyCode.LeftShift;

        [Name( "Navigate Quick Select" )]
        [Description( "Quick select key to equip Charcoal." )]
        public KeyCode NavigateKey = KeyCode.BackQuote;

        [Name( "Crafting UI" )]
        [Description( "Quick select key to toggle the crafting UI. (Hold to show menu. Press for default behaviour.)" )]
        public KeyCode CraftingKey = KeyCode.X;

        [Name( "LightSource Radial" )]
        [Description( "Quick select key to show the LightSource radial UI. (Hold to show menu. Press for default behaviour.)" )]
        public KeyCode LightSourceKey = KeyCode.Alpha5;

        [Name( "Weapon Radial" )]
        [Description( "Quick select key to show the Weapon radial UI." )]
        public KeyCode WeaponKey = KeyCode.Alpha6;

        [Name( "Auto Pickup" )]
        [Description( "Modifier key to auto pickup items instead of inspecting them." )]
        public KeyCode AutoPickupKey = KeyCode.LeftShift;


        [Section( "Radial" )]
        [Name( "Combine Items" )]
        [Description( "The radial menu will only show one item of each type.\n" +
                      "Mouse wheel will select which item of that type is used. (IE: Birch Tea, Coffee)\n" +
                      "Also sorts the items by Heat status first, then by Calories remaining and finally by HP." )]
        public bool RadialCombineItems = true;

        [Name( "Show Ruined Food" )]
        [Description( "Always show ruined food items in the radial menu." )]
        public bool RadialShowRuinedFood = true;

		[Name( "Show Insulated Flask Contents" )]
		[Description( "Always show Insulated Flasks contents in the radial menu." )]
		public bool RadialShowInsulatedFlaskContents = true;


        [Section( "Repair" )]
        [Name( "Repair Colored Amount" )]
        [Description( "Change the color of the repair amount when it is less than the full amount." )]
        public bool RepairColoredAmount = true;


        [Section( "Torch" )]
        [Name( "Lowest Torch" )]
        [Description( "The radial menu will equip the lowest quality torch." )]
        public bool TorchUseLowest = true;

        [Name( "Always Select Starter" )]
        [Description( "Always show the torch light UI to select the starter item.  This helps prevent accidental usage of matches." )]
        public bool TorchLightAlwaysShow = true;


        [Section( "Toilet" )]
        [Name( "Water Non Potable" )]
        [Description( "When enabled, water taken from toilets is non potable." )]
        public bool ToiletNonPotable = true;


		[Section( "Travois" )]
		[Name( "Pickup With Contents" )]
		[Description( "When enabled, allows picking up a Travois while items are still inside.  Also enables transfering items to a Travois in your inventory." )]
		public bool TravoisPickupWithContents = false;

		[Name( "Use With Rope" )]
		[Description( "When enabled, allows attaching a Travois to a rope and raising/lowering it at climb points." )]
		public bool TravoisUseWithRope = false;

		[Name( "Show In Radial" )]
		[Description( "When enabled, the travois will appear under the navigation radial menu.  This can be used to deploy the Travois." )]
		public bool TravoisShowInRadial = true;


        [Section( "UI" )]
        [Name( "Buff Notification Offset" )]
        [Description( "A vertical offset applied to the buff notification UI." )]
        public float BuffOffsetVertical = 0.5f;

		[Name( "Clothing Condition Bars" )]
		[Description( "When enabled, shows bars at the bottom of each clothing slot to show the current condition of the item." )]
		public bool ClothingConditionBars = true;

        [Name( "Console Dark Mode" )]
        [Description( "Change the console to use a dark mode color scheme." )]
        public bool ConsoleDarkMode = true;

        [Name( "Weight Label" )]
        [Description( "Show a weight label on the hud." )]
        public WeightLabelType WeightLabel = WeightLabelType.AutoHide;

        [Name( "Wind Status Bar" )]
        [Description( "Show a wind status bar." )]
        public WindStatusType WindStatusBar = WindStatusType.DirectionAndSpeed;

		[Name( "Show Temperature Labels" )]
		[Description( "Show labels on the cold and wind status bars that display FeelsLike and WindChill temps." )]
		public bool ShowTemperatureLabels = true;


        public static Settings Instance = new();

        public static void OnLoad()
        {
            Instance.AddToModSettings( "Quality of Life" );
        }

        protected override void OnChange( FieldInfo field, object? oldValue, object? newValue )
        {
			string TravoisPickupWithContentsWarning = "\n\"Pickup With Contents\" in QoL v1.12.0 caused this mod to handle save/load for items in a Travois while in your inventory.  "+
													  "Before disabling the option or this mod, MAKE SURE all Travois in your inventory are empty and save, or save with QoL v1.13.0 or newer!  "+
													  "Otherwise you WILL loose all items inside such Travois!  "+
													  "This does not affect Travois that are deployed in the world.";

            if ( field.Name == nameof( TravoisPickupWithContents ) )
            {
                Panel_Confirmation Confirmation = InterfaceManager.GetPanel<Panel_Confirmation>();
                if ( Confirmation != null )
                {
                    UISprite? BackGlow = Confirmation.m_GenericMessageGroup.m_Parent.transform.Find( "BG_Container /BackGlow" )?.GetComponent<UISprite>();

					var OnWarningConfirm = () =>
					{
						Confirmation.m_GenericMessageGroup.m_MessageLabel.lineWidth /= 2;
                        if ( BackGlow != null )
                        {
                            BackGlow.width /= 2;
                        }
                    };

                    Confirmation.ShowErrorMessage( TravoisPickupWithContentsWarning, OnWarningConfirm );
					Confirmation.m_GenericMessageGroup.m_MessageLabel.lineWidth *= 2;
					Confirmation.m_GenericMessageGroup.m_MessageLabel_InputFieldTitle.text = "WARNING!!!";
					WidgetUtils.SetActive( Confirmation.m_GenericMessageGroup.m_MessageLabel_InputFieldTitle, true );

                    if ( BackGlow != null )
                    {
                        BackGlow.width *= 2;
                    }
                }
            }
        }

        protected override void OnConfirm()
        {
            base.OnConfirm();
            Patch_uConsole_Start.UpdateConsoleColor();

			if ( EnableMod )
			{
				CatTailHelper.TryUpdateInventory();
			}
        }
    }
}
using ModSettings;
using UnityEngine;

namespace QualityOfLife
{
    internal class Settings : JsonModSettings
    {
        [Section( "Crafting" )]
        [Name( "Remember Filter" )]
        [Description( "Remembers which crafting filter was selected, and restores it when the crafting UI is opened." )]
        public bool CraftRememberFilter = true;


        [Section( "Fire" )]
        [Name( "Auto-Select Lit Torch" )]
        [Description( "Auto selects the torch as the fire starter if the player is holding a lit torch." )]
        public bool FireAutoSelectLitTorch = true;

        [Name( "Auto-Select Mag Lens" )]
        [Description( "If the the Mag Lens is able to be used, then auto select it.  A lit torch takes precedence if 'Auto-Select Lit Torch' is enabled." )]
        public bool FireAutoSelectMagLens = true;

        [Name( "Auto-Skip Birch Bark" )]
        [Description( "Auto selects next tinder type when Fire Start UI is opened.  This prevents accidental usage of Birch Bark as tinder.\nIf tinder is required and Birch Bark is the only tinder available, this does nothing as the next item is still Birch Bark." )]
        public bool FireAutoSkipBirchBark = true;

        [Name( "Remember Selection" )]
        [Description( "Remember previous selected items and auto select them.  Above options take precedence is enabled." )]
        public bool FireRememberSelection = true;


		[Section( "Input" )]
		[Name( "Separate Interact" )]
		[Description( "Separate interact with objects so it doesn't share the same controls as shooting." )]
		public bool SeparateInteract = true;

		[Name( "Highlight Selection" )]
		[Description( "Changes the background color of items on the Harvest and FireStart UI to show which row has keyboard focus." )]
		public bool HighlightSelection = true;

		[Name( "UI Extra Controls" )]
		[Description( "Adds extra shortcut keys to the Breakdown, Clothing, FireStart, Harvest, Inventory, Menu, Reading, Rest and TorchLight UI."+
					  "\nAccept"+
					  "\n    Confirms Selection / Examine Item"+
					  "\nA / D"+
					  "\n    Adjust Tool / Adjust Time"+
					  "\nMouseWheel"+
					  "\n    Adjust Tool / Rotate Safe Dial / Scroll Clothing Items"+
					  "\nTab"+
					  "\n    Cycles between Harvest / Quarter and Rest / PassTime"+
					  "\nDrop"+
					  "\n    Drop Item"+
					  "\nEquip"+
					  "\n    Equip / Wear / Remove Item"+
					  "\nPrecision Rotate"+
					  "\n    Slows down mouse wheel adjustment for SafeCracking and Placement."+
					  "" )]
		public bool UIExtraControls = true;

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
		[Description( "Quick select key to toggle the crafting UI." )]
		public KeyCode CraftingKey = KeyCode.X;


        [Section( "Map" )]
        [Name( "Show Player Icon" )]
        [Description( "Show the player's location on the map.  It does not reveal any of the map, just renders the player's icon." )]
        public bool MapShowPlayerIcon = true;


		[Section( "Repair" )]
		[Name( "Repair Colored Amount" )]
		[Description( "Change the color of the repair amount when it is less than the full amount." )]
		public bool RepairColoredAmount = true;

		[Section( "Torch" )]
        [Name( "Lowest Torch" )]
        [Description( "The radial menu will equip the lowest quality torch." )]
        public bool TorchUseLowest = true;

        [Name( "Always Select StarterName" )]
        [Description( "Always show the torch light UI to select the starter item.  This helps prevent accidental usage of matches." )]
        public bool TorchLightAlwaysShow = true;


        public static Settings Instance = new();

        public static void OnLoad()
        {
            Instance.AddToModSettings( "Quality of Life" );
        }
    }
}
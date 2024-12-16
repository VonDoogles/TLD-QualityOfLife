# Quality Of Life
A mod for The Long Dark that makes several changes to the UI in hopes to make the game more consistent and easier to control.  Also contains some options to make the game more difficult.

> [!IMPORTANT]
> Requires the use of MelonLoader v0.6.6.

> [!IMPORTANT]
> Quality Of Life requires the ModSettings mod to be installed.  (https://tldmods.com/)

> [!WARNING]
> When using **Cat Tail Calories**, calorie count for inventory is saved.  Before removing/disabling this mod, set Cat Tail Calories back to Default and save the game.

> [!CAUTION]
> **Pickup With Contents** in QoL v1.12.0 caused this mod to handle save/load for items in a Travois while in your inventory.  Before disabling the option or this mod, MAKE SURE all Travois in your inventory are empty and save, or save with QoL v1.13.0 or newer!  Otherwise you WILL loose all items inside such Travois!  This does not affect Travois that are deployed in the world.

# Features
## Animation
### Play Harvest Anims
When true, plays the new harvest anims, when false uses the old progress bar.

## Crafting
### Remember Filter
Remembers which crafting filter was selected, and restores it when the crafting UI is opened.

## Difficulty
### Allow Take Torch
Allows taking torchs from lit fires.

### Cat Tail Calories
The amount of calories Cat Tail stalks have.
- OneThird - 1/3 the normal calories. (50)
- TwoThird - 2/3 the normal calories. (100)
- Default - the normal amount of calories. (150)

> [!WARNING]
> When using **Cat Tail Calories**, calorie count for inventory is saved.  Before removing/disabling this mod, set Cat Tail Calories back to Default and save the game.

### Cat Tail Harvest Stalk
Enable/Disable harvesting Cat Tail stalks.

### Cat Tail Harvest Tinder
Enable/Disable harvesting Cat Tail tinder.

### Item Drop Rotation
The rotation of items when dropped.
- StickNorth - Use base game behaviour.
- PlayerFacing - Drop items facing the same direction as the player.
- Random - Drop items facing a random diration.


### Meat Settings
Settings to control the min/max KG of meat that Bear, Deer, Moose and Wolf carcas will have.

## Food
### Cook Filter Reheat
Filter reheatable items in the Cook UI.

### Eat Pick Units
Show the Pick Units UI to select how many stackable food items to eat.

### Scale Meat 1/3 Size
Meat on the ground that has less than this percent of full calories is scaled to 1/3 size.

### Scale Meat 2/3 Size
Meat on the ground that has less than this percent of full calories is scaled to 2/3 size.

## Fire
### Auto-Select Lit Torch
Auto selects the torch as the fire starter if the player is holding a lit torch.

### Charcoal Pick Units
Show the Pick Units UI to select how much charcoal to take.

### Auto-Select Mag Lens
If the the Mag Lens is able to be used, then auto select it.  A lit torch takes precedence if 'Auto-Select Lit Torch' is enabled.

### Auto-Skip Birch Bark
Auto selects next tinder type when Fire Start UI is opened.  This prevents accidental usage of Birch Bark as tinder.\nIf tinder is required and Birch Bark is the only tinder available, this does nothing as the next item is still Birch Bark.

### Remember Selection
Remember previous selected items and auto select them.  Above options take precedence is enabled.

## Fishing
### Remember Selection
Remember previous selected items and auto select them.

### Show Remaining Bait
Show remaining bait on fishing UI.

### Show Tipup Condition
Show Tipup condition on fishing UI.

## Fuel
### Select Source
When enabled, allows choosing which fuel source to use when refueling.

## Input
### Separate Interact
Separate interact with objects so it doesn't share the same controls as shooting.

### Highlight Selection
Changes the background color of items on the Harvest and FireStart UI to show which row has keyboard focus.

### Rest Preset
Adds a preset button for 10 hours to the rest UI.

### UI Extra Controls
Adds extra shortcut keys to verious UI screens.
- Affliciton:
    - Interact: Treats the selected wound.
    - A / D / MouseWheel: Scrolls the wound list.
- Badges:
    - A / D / MouseWheel: Scrolls the active list.
    - Tab: Cycles between Challenges and Feats.
- BasicMenu:
    - Interact: Selects the highlight menu option.
- Breakdown:
    - Interact: Starts breakdown.
    - A / D / MouseWheel: Scrolls tool list.
- Clothing:
    - Drop: Drops the selected item.
    - Equip: Toggles the wear status of the selected item.
    - Interact: Examines the selected item.
    - MouseWheel: Scrolls the available items list for the selected clothing slot.
- Confirmation:
    - Interact: Confirms the dialog prompt. (Does not work on input dialogs like Rename.)
- Container:
    - Equip / Interact: Transfers the selected item to the other side.
    - Tab: Changes focus to the other side.
- Cooking:
    - Interact: Start cooking.
    - MouseWheel: Increase / Decrease what water amount when making water.
- Crafting:
    - Interact: Start crafting.
    - A / D: Increase / Decrease quantity.
- Feed Fire:
    - Interact: Feed the selected item to the fire.
- Fire Start:
    - Interact: Start the fire.
- Gear Select:
    - Interact: Select gear.
    - A / D / MouseWheel: Scroll the gear list.
- IceFishing:
    - Interact: Start fishing.
    - A / D / MouseWheel: Increase / Decrease time.
- IceFishing HoleClear:
    - Interact: Start breaking ice.
    - A / D / MouseWheel: Scroll the tool list.
- Inventory:
    - Drop: Drops the selected item.
    - Equip: Equips the selected item.
    - Interact: Examines the selected item.
- Inventory Examine:
    - Escape: Cancel in-progress item repair.
    - A / D / MouseWheel: Increase/Decrease read time.
    - Interact: Start reading / Select action tool / Select action.
- Log
    - Auto scroll to botton of the daily list when opened.
    - Interact: Selects selected sub-screen / Selects select item in sub-screens.
- MainMenu
    - Equip: Toggle selected feat.
    - Interact: Continue on the feats screen.
    - MouseWheel: Scroll feats list.
- Map
    - A / D: Cycle shown region map.
    - Tab: Toggle world map.
- Milling
    - Interact: Start repairing.
- Pick Units
    - Interact: Pick selected number of units.
    - Equip: Pick all units.
- Pick Water
    - Interact: Take select amount of water.
    - Equip: Take all water.
- Rest:
    - Added button to set time directly to 10 hours.
    - Interact: Start PassTime / Rest.
    - Tab: Cycle between PassTime / Rest.
    - Drop: Pickup bedroll.
    - A / D / MouseWheel: Increase / Decrease time.
- Select Region Map
    - Interact: Continue with selected region.
- Snow Shelter Build
    - Interact: Start building.
- Show Shelter Interact
    - Interact: Start selected action.
- Torch Light
    - Interact: Use selected item.
- Weapon Picker
    - Equip / Interact: Use selected weapon.
- Safe Cracking
    - Interact: Opens safe if unlocked.
    - MouseWheel: Rotate safe dial.
    - Precision Rotate: When held, slows down mouse wheel rotation.

### Drop Item in Hands
Which type of items held in player's hands can be dropped with the Drop key. (None, Any, LightSource, NonWeapons)

### QuickSelect Hold Duration
Hold long QuickSelect Key must be held down to show Radial UI. (Default: 0.25)

### Accept / Interact
The key for accepting menu choices and interacting with objects in the world.

### Drop / Put Back
The key for dropping items or putting them back while inpecting them.

### Equip / Consume
The key equipping items or consuming them.

### Precision Rotate
When this key is held, rotating the placement object with mouse wheel is more precise.

### Navigate Quick Select
Quick select key to equip Charcoal.

### Crafting UI
Quick select key to toggle the crafting UI.

### LightSource Radial
Quick select key to show the LightSource radial UI. (Hold to show menu. Press for default behaviour.)

### Weapon Radial
Quick select key to show the Weapon radial UI. (Hold to show menu. Press for default behaviour.)

### Auto Pickup
Modifier key to auto pickup items instead of inspecting them.

## Radial
### Combine Items
The radial menu will only show one item of each type.  Mouse wheel will select which item of that type is used. (IE: Birch Tea, Coffee)
Also sorts the items by Heat status first, then by Calories remaining and finally by HP.

### Show Ruined Food
Always show ruined food items in the radial menu.

### Show Insulated Flask Contents
Always show Insulated Flasks contents in the radial menu.

## Repair
### Repair Colored Amount
Change the color of the repair amount when it is less than the full amount.

## Torch
### Lowest Torch
The radial menu will equip the lowest quality torch.

### Always Select Starter
Always show the torch light UI to select the starter item.  This helps prevent accidental usage of matches.

## Toilet
### Water Non Potable
When enabled, water taken from toilets is non potable.

## Travois
### Pickup With Contents
When enabled, allows picking up a Travois while items are still inside.  Also enables transfering items to a Travois in your inventory.

> [!CAUTION]
> **Pickup With Contents** in QoL v1.12.0 caused this mod to handle save/load for items in a Travois while in your inventory.  Before disabling the option or this mod, MAKE SURE all Travois in your inventory are empty and save, or save with QoL v1.13.0 or newer!  Otherwise you WILL loose all items inside such Travois!  This does not affect Travois that are deployed in the world.

### Use With Rope
When enabled, allows attaching a Travois to a rope and raising/lowering it at climb points.

### Show In Radial
When enabled, the travois will appear under the navigation radial menu.  This can be used to deploy the Travois.

## UI
### Buff Notification Offset
A vertical offset applied to the buff notification UI.

### Console Dark Mode
Change the console to use a dark mode color scheme.

### Weight Label
Show a weight label on the hud.

### Wind Status Bar
Show a wind status bar.

### Show Temperature Labels
Show labels on the cold and wind status bars that display FeelsLike and WindChill temps.

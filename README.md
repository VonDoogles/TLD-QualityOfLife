# Quality Of Life
A mod for The Long Dark that makes several changes to the UI in hopes to make the game more consistent and easier to control.

# Features
## Crafting
### Remember Filter
Remembers which crafting filter was selected, and restores it when the crafting UI is opened.

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

## Input
### Separate Interact
Separate interact with objects so it doesn't share the same controls as shooting.

### Highlight Selection
Changes the background color of items on the Harvest and FireStart UI to show which row has keyboard focus.

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

## Map
### Show Player Icon
Show the player's location on the map.  It does not reveal any of the map, just renders the player's icon.

## Radial
### Combine Items
The radial menu will only show one item of each type.  Mouse wheel will select which item of that type is used. (IE: Birch Tea, Coffee)
Also sorts the items by Heat status first, then by Calories remaining and finally by HP.

## Repair
### Repair Colored Amount
Change the color of the repair amount when it is less than the full amount.

## Torch
### Lowest Torch
The radial menu will equip the lowest quality torch.

### Always Select Starter
Always show the torch light UI to select the starter item.  This helps prevent accidental usage of matches.


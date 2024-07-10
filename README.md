# Text Based Adventure Framework
## How To Use This Framework
The project should contain a sample story to showcase how elements of the framework function. To start creating your own story, follow these steps:

### Step 1: Create and set up a new scene
In your new scene, drag in the Framework prefab. This should come included with all variables set up. Now remove all the TestScene ScriptableObjects from the Scenes list on the Gamemanager object. Set up the player's starting money by modifying the "Starting Money" variable on the Gamemanager object.

### Step 2: Create a starting scene
Right click in the project folder widget and select "Story Scene" from the "StorythreadSO" option under the "Create" option. This will create a blank story scene. Input "Start" (Case sensitive) as the Scene Tag. This is the only story scene that requires a specific scene tag. The project will still function without this scene tag, but it may have unexpected behavior.

The "Scene Title" field is what will be displayed in place of the text in the editor view that should currently say "Title".
The "Scene Description" field is the body of text that will describe what happens in your scene.
You can set a sprite to use as your background image. If you don't, the background image will be the same as the previous story scene's.
The "Uses Master Button" checkbox will enable the story scene to use the highest up button (In editor view, this is the one with white text). Check this box if your story scene doesn't have much text in it. Leave it unchecked if you'd like some more room.

### Step 3: Create a choice
A text based adventure isn't much if there's only one story scene. Add an element to the Scene Choices array. Note that choices will be displayed in game in the order in which they are input into the array.

**"Choice Text"** is what will appear on the button itself. For example, if I set this text to "Explore the forest" the button will appear with the text "Explore the forest" on it.
**"Destination"** is the scene tag (**NOT TITLE**) that clicking this button will send you to. For example, if I set this to "Forest" then the button will send me to the scene with the scene tag "Forest". If I have a different scene titled "Forest" but with a different scene tag, it won't be reached via this button.
**"Tools"** is a list of tags that are required to select this choice. We'll get into tags later.
**"Coin Cost"** is the amount of money this choice requires and consumes. Note that you can set this value to a negative number to grant coins. For example, I can charge a player 20 coins to buy a potion by setting the value to 20, or give them 30 in exchange for some labor by setting the value to -30.
**"Lockout Tools"** is a list of tags that forbid a player from selecting this choice. This will be explained later.
**"Lower Time Bound"** prevents an option from being selected if the day counter is below this value. For example, if I set this to 3 then the player can't select this option until it's at least day 3.
**"Upper Time Bound"** prevents an option from being selected if the day counter is *above* this value. For example, if I set this to 7 then the player can't select this option if it's day 7 or later.
**"Hide When Invalid"** determines if the choice should be displayed in greyed out text if it cannot be selected, or if the button shouldn't appear at all.
**"Advances Day"** determines if selecting this choice should increment the day counter.
**"Rewards"** is a list of all items the choice will give you.

Once you've created at least once choice, you'll need a second story scene to go to. Create one and make sure that the Scene Tag (**NOT TITLE**) matches the "Destination" field for the choice you created.

If you start the game now, nothing will happen. This is because you need to add every story scene you make to the GameManager's Scenes array. Do that for your starting story scene and any other story scenes you've created.

### Step 4: Create some items
While some stories may thrive on choice alone, you'll likely want to award your players some items they can use in certain situations. Items are also simple to create. When you were creating story scenes you may have noticed the "Story Item" option under StorythreadSO. These scriptableobjects contain all the data for your items.

**"Name"** is the display name of the item.
**"Persists"** determines whether the item will remain in your inventory if the story is reset (we'll get to that).
**"Is Secret"** determines whether the item will be displayed on the inventory screen. For example, if I wanted to track whether the player has stolen from a merchant, but I don't want that to be obvious to the player, I can give them a secret item with the Thief trait. It won't appear in their inventory, and the player can face the consequences for their actions later without being any the wiser.
The **"Traits"** list is a list of tags the item has. These tags (which are case sensitive) are what is looked for in the Tools and Lockout Tools options in your story scene. For example, if I give an item the "Axe" tag, that item can be used to enable any choice that requires the Axe tag. If I also give it the "Weapon" tag it can be used in any choice that requires the Axe or Weapon tags. Under the right circumstances, it might even be used for both at the same time!

Once you've created an item, you can give it to the player by adding an element to the "Rewards" array of a choice. Select that item as the "Item Data" and input a count. For example, if I wanted to give the player an axe, I would set the item data to my "Axe" item and give it a count of 1. If I wanted to give them five gold bars, I'd use my Gold Bar item data and give a count of 5.

You can leave the count at 0, but this may cause some unexpected behavior.

### Step 5: Create restrictions for a choice
Almost any story will do this at some point. You can't chop down the tree without an axe, you can't unlock the chest without a key, and you can't get service from a shopkeeper if you've stolen from them.

To make requirements for selecting a choice, add an element into the "Tools" array.

**"Trait"** is the item tag this choice will look for. If the player doesn't have any items with this tag, the choice cannot be selected. For example, if I add "Axe" to this array, then the player can't select this choice unless they have an item with the "Axe" tag. This could be a makeshift axe, or a fire axe, or even a battleaxe. As long as it has the Axe tag it'll be usable.
**"Amount"** is the amount of items with this tag you'll need. If you set this number higher than 1, you'll need more than one item with that tag for the choice to be selectable. For example, if I wanted to require multiple logs to start a fire, I might set add a "Fire Material" requirement with an amount of 5, meaning the player will need five items with the Fire Material tag. These can be different items or the same item.
**"Consumed"** determines whether or not the item in question will be removed from your inventory upon selecting the choice. A log is likely consumed when used for a fire. I'd tick this box in that case. An axe probably will survive cutting down a single tree. I'd leave this box unticked in that case.
**WARNING: Currently there is no way to prioritize which items will be removed if multiple have the same tag. They will be removed in the order they appear in the inventory (which is the order they were added/last modified in). If you want to give the player a choice of which item to consume, take them to an intermediary scene.**

That's all good, but what if you want to stop a player from doing an action under certain circumstances? That's what Lockout Tools is for.

**"Trait"** is the item tag this choice will look for. If the player has any items with this tag, the choice will become invalid. For example, if the secret item I gave the player earlier has the "Thief" tag, I can set a lockout tool to "Thief" and the player will not be able to visit the shop again.
**"Maximum"** is the upper limit on the amount of matching tags, similar to "Amount" in the Tools array. If you set this number to a value above 1, the player will still be able to select this option if the total number of matching tagged items in their inventory is less than this number. If I want to give the player a second chance, I may set the maximum to 2, so the first time they steal they can get away with it, but if they steal a second time, they're out of the shop forever.

### Step 6: End the story
If, at any point, for any reason, you wish for a player to start the story from the beginning, just set a choice's destination as "RESET" (case sensitive). This will take the player back to the story scene with the "Start" tag and reset their coin count, day counter, and inventory (but it will not remove any items set to persist). This is helpful for when, for example, a player dies.

## FAQ
Q: Why are my story scenes not being detected?
A: Make sure you've added them to the Scenes array in the Gamemanager.

Q: How do I change the order in which choices are displayed?
A: You have to change the order the choices appear in that story scene's Choices array. Buttons display choices in the same order as the array.

Q: The background for a scene is different in some cases. Why?
A: If you don't set a background image to use, it won't change from the previous story scene. This can be handy in situations like extended scenes in one area, but it can mean your forest background can persist into town if you never go through a story scene that sets the background back to the town background.

Q: Can I have multiple items with the same tags but different names?
A: Yes. Tags are pure data and shouldn't affect anything organization-wise.

Q: Can I have multiple items with the same name, but different tags?
A: Not within the same story. Currently the inventory code checks for the item's name when checking for duplicate items. This makes enough sense design-wise that it's not worth fixing; you don't want the player's inventory to be filled with items all named "Iron Sword" and have them confused about what the difference is.

Q: Can I have multiple story scenes with the same title?
A: Yes. Title is display only and doesn't affect the code.

Q: Can I have multiple story scenes with the same tag?
A: No. Tags are data-significant and cannot be duplicated within the same story, for the same reasons you can't have a duplicate file name on your device.

Q: Does that mean I can't have a story scene with the "End" tag because of the example scenes?
A: No, as long as you don't add said example story scene to your copy of the Gamemanager. Each copy of the Gamemanager has a different Scenes array, and only the things in that version of the array are relevant to the story. Just make sure you aren't using the same tag twice within the same story.

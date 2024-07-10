using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StoryNavigation : MonoBehaviour
{
    public StoryScene[] Scenes;
    public GameObject TitleUI;
    private TextMeshProUGUI titleText;
    public GameObject bodyUI;
    private TextMeshProUGUI bodyText;
    public GameObject statusUI;
    private TextMeshProUGUI statusText;
    public GameObject coinUI;
    private TextMeshProUGUI coinText;
    public GameObject background;

    public GameObject[] buttons;
    

    private bool[] globalValid;

    private TextMeshProUGUI[] buttonText;

    private string currentScene = "Start";
    private string[] sceneTags;

    [Space]
    
    public TextMeshProUGUI printInventory;
    public List<StoryScene.InventoryEntry> inventory;


    [Space] private int day = 1;
    public int startingCoins = 10;
    [SerializeField] private int money = 10;
    

    // Start is called before the first frame update
    void Start()
    {
        titleText = TitleUI.GetComponent<TextMeshProUGUI>();
        bodyText = bodyUI.GetComponent<TextMeshProUGUI>(); //Set the title and body text components for easier access.
        statusText = statusUI.GetComponent<TextMeshProUGUI>();
        coinText = coinUI.GetComponent<TextMeshProUGUI>();
        
        
        buttonText = new TextMeshProUGUI[buttons.Length]; //Initialize the button array
        for (int i = 0; i < buttonText.Length; i++) //For each one...
        {
            buttonText[i] = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
        } //Get all the text components for easier access
        globalValid = new bool[buttons.Length]; //Initialize the valid choice array

        
        sceneTags = new string[Scenes.Length]; //Initialize the tags array
        for (int i = 0; i < Scenes.Length; i++) //For each one...
        {
            sceneTags[i] = Scenes[i].sceneTag; //Get the story scene's tag for easier reference.
        }
        
        titleText.text= Scenes[findIndex(currentScene)].sceneTitle; //Set the title text
        bodyText.text = Scenes[findIndex(currentScene)].sceneDescription; //And the body text
        
        renderChoices(); //Render the buttons.
    }
    

    // Update is called once per frame
    void Update()
    {
        titleText.text = Scenes[findIndex(currentScene)].sceneTitle; //Set the title text
        bodyText.text = Scenes[findIndex(currentScene)].sceneDescription; //And the body text
        if (Scenes[findIndex(currentScene)].backgroundImage != null)
        {
            background.GetComponent<Image>().sprite = Scenes[findIndex(currentScene)].backgroundImage;
        }
        
        statusText.text = "Day: " + day + "\nCoins: " + money;
        renderChoices();

        //Print Inventory
        printInventory.text = ""; //Reset it so it doesn't just keep repeating
        coinText.text = "Coins: " + money;
        foreach (var item in inventory)
        {
            if (!item.itemData.isSecret)
            {
                printInventory.text += item.itemData.name + "\n";
            }
        }
    }
    

    private int findIndex(string tag) //This converts the scene tag into the position in the array for easier use.
    {
        for (int i = 0; i < sceneTags.Length; i++) //For each tag...
        {
            if (tag == sceneTags[i]) //If it's equal to the inputted string...
            {
                return i; //Return that position in the array
            }
        }

        Debug.Log("Error! Scene tagged " + tag + " does not exist! Make sure it's spelled right and case sensitive. In the meantime, resetting position...");
        return 0; //Go back to zero if it's not found.
    }
    

    private void renderChoices() //This takes the button information and displays it on the buttons in the scene.
    {
        int location = findIndex(currentScene); //Get the scene index for easier use

        for (int i = 0; i < Scenes[location].sceneChoices.Length; i++) //For each option...
        {
            StoryScene.choice choice = Scenes[location].sceneChoices[i];
            
            
            
            if (choice.destination != "None") //If the option exists...
            {
                if (Scenes[location].usesMasterButton)
                {
                    if (IsChoiceValid(Scenes[location].sceneChoices[i])) //If the choice can be selected...
                    {
                        buttons[i].SetActive(true); //Set the button to active
                        buttonText[i].text = Scenes[location].sceneChoices[i].choiceText; //Activate the button and set it's text.
                        buttonText[i].color = Color.black; //Make it black
                        globalValid[i] = true; //Set the bool to allow the button to be selectable
                    }
                    else //If the choice is invalid for any reason...
                    {
                        buttons[i].SetActive(!Scenes[location].sceneChoices[i].hideWhenInvalid);
                        buttonText[i].text = Scenes[location].sceneChoices[i].choiceText; //Activate the button and set it's text.
                        buttonText[i].color = Color.gray; //Make it grey
                        globalValid[i] = false;
                    }
                }
                else
                {
                    buttons[0].SetActive(false);
                    
                    
                    if (IsChoiceValid(Scenes[location].sceneChoices[i])) //If the choice can be selected...
                    {
                        buttons[i + 1].SetActive(true); //Set the button to active
                        buttonText[i + 1].text = Scenes[location].sceneChoices[i].choiceText; //Activate the button and set it's text.
                        buttonText[i + 1].color = Color.black; //Make it black
                        globalValid[i + 1] = true; //Set the bool to allow the button to be selectable
                    }
                    else //If the choice is invalid for any reason...
                    {
                        buttons[i + 1].SetActive(!Scenes[location].sceneChoices[i].hideWhenInvalid);
                        buttonText[i + 1].text = Scenes[location].sceneChoices[i].choiceText; //Activate the button and set it's text.
                        buttonText[i + 1].color = Color.gray; //Make it grey
                        globalValid[i + 1] = false;
                    }
                }
            }
        }

        if (Scenes[location].usesMasterButton)
        {
            for (int i = Scenes[location].sceneChoices.Length; i < buttons.Length; i++) //Clean up the buttons that aren't being used
            {
                buttons[i].SetActive(false); //By setting them to inactive
                globalValid[i] = false; //And making them unpressable.
            }
        }
        else
        {
            for (int i = Scenes[location].sceneChoices.Length + 1; i < buttons.Length; i++) //Clean up the buttons that aren't being used
            {
                buttons[i].SetActive(false); //By setting them to inactive
                globalValid[i] = false; //And making them unpressable.
            }
        }
        
        
        
    }
    

    public bool IsChoiceValid(StoryScene.choice choice) //This determines whether a given choice is selectable or not.
    {
        if (choice.tools.Length > 0)
        {
            foreach (var requirement in choice.tools) //For each requirement there is...
            {
                bool hasTool = false; //Initialize a bool...
                int total = requirement.amount;
                foreach (var item in inventory) //For each item in the inventory...
                {
                    foreach (var trait  in item.itemData.traits) //For each trait an item has...
                    {
                        if (trait == requirement.trait) //If it's the tool you need to choose the option...
                        {
                            total -= item.itemCount;
                        }

                        if (total <= 0)
                        {
                            hasTool = true; //Mark that you have the tool.
                        }
                    }
                }

                if (hasTool == false) //If you didn't have the tool...
                {
                    return false; //Then return false.
                }
            }
        }
        
        
        if (choice.lockoutTools.Length > 0) //If there's a tool that prevents you from picking the option...
        {
            foreach (var requirement in choice.lockoutTools) //For each requirement there is...
            {
                int bucket = 0;
                foreach (var item in inventory) //For each item in the inventory...
                {
                    foreach (var trait in item.itemData.traits) //For each trait the item has...
                    {
                        if (trait == requirement.trait) //If it's the tool that locks the option...
                        {
                            bucket += item.itemCount;
                        }
                    }
                }

                if (bucket >= requirement.maximum && bucket != 0) //If you have more than the choice allows...
                {
                    return false; //Then return false.
                }
            }
        }


        if (choice.lowerTimeBound > day) //If the option only becomes available at a day you haven't reached yet...
        {
            return false;
        }

        if (choice.upperTimeBound != 0 && choice.upperTimeBound < day) //If the option becomes unavailable at a day you've passed...
        {
            return false;
        }
        
        if (money < choice.coinCost) //If you don't have enough money...
        {
            return false; //Then return false.
        }

        return true; //If none of the above are false, then return true.
    }
    

    public void ButtonInput(int input) //This handles the input from the buttons.
    {
        StoryScene.choice chosen;
        
        if (Scenes[findIndex(currentScene)].usesMasterButton)
        {
            chosen = Scenes[findIndex(currentScene)].sceneChoices[input]; //Convert the input into a quick choice variable.
        }
        else
        {
            chosen = Scenes[findIndex(currentScene)].sceneChoices[input - 1]; //Convert the input into a quick choice variable.
        }
        
        
        
        if (globalValid[input]) //If it's valid (determined earlier)
        {
            //Remove items
            foreach (var requirement in chosen.tools) //For each requirement there is...
            {
                if (requirement.consumesItem)
                {
                    int total = requirement.amount;
                    for(int i = 0; i < inventory.Count; i++) //Compare against all items in your inventory...
                    {
                        bool done = false;
                        
                        StoryScene.InventoryEntry thisItem = inventory[i];
                        foreach (var trait in inventory[i].itemData.traits)
                        {
                            if (trait == requirement.trait) //When you find the matching trait...
                            {
                                
                                thisItem.itemCount -= total; //Subtract from the count.
                                Debug.Log(thisItem.itemCount + " Are left");
                                total = 0;
                                if (thisItem.itemCount < 0) //If you've subtracted more than you have...
                                {
                                    total = math.abs(thisItem.itemCount); //Reset the total to be what the difference was.
                                    Debug.Log(total + " left to remove");
                                }
                                
                                
                                inventory.RemoveAt(i); //Remove the item
                                if (thisItem.itemCount > 0) //If you still have at least one...
                                { 
                                    inventory.Add(thisItem); //Add the item back with the new count.
                                }
                                else //If it's no longer in your inventory...
                                {
                                    i--; //Back up so you don't skip any items
                                }

                                if (total <= 0) //If there's no more to subtract...
                                {
                                    Debug.Log("Oh, that's all?");
                                    done = true; //Then you're done with this requirement
                                    break; //So stop checking items.
                                }
                                
                                
                            }
                        }

                        if (done) //If you already removed an item...
                        {
                            break; //Stop checking for this requirement.
                        }
                    }
                }
            }
            
            //Add items
            if (chosen.destination == "RESET") //If the button ends the story...
            {
                money = startingCoins; //Reset to the starting coins
                day = 1; //Reset the day count.
                for (int i = 0; i < inventory.Count; i++) //Remove all resettable items
                {
                    if (!inventory[i].itemData.persists) //If the item doesn't persist...
                    {
                        Debug.Log("Removed " + i + ", " + inventory.Count + " remaining");
                        inventory.Remove(inventory[i]); //Remove it.
                        i --;
                    }
                }

                if (chosen.rewards.Length > 0) //If the choice has a reward...
                {
                    AddItemToInventory(chosen.rewards.ToList());
                }
                
                currentScene = "Start";
            }
            else //if the button does not end the story...
            {
                money -= chosen.coinCost; //Subtract the coin cost from your coin total.
                if (chosen.advancesDay) //If the choice advances the day...
                {
                    day++; //Increment the day variable.
                }

                if (chosen.rewards.Length > 0) //If the choice has a reward...
                {
                    AddItemToInventory(chosen.rewards.ToList());
                }

                currentScene = chosen.destination; //Move to the destination.
            }
        }
        else
        {
            Debug.Log( input + " goes Boink"); //Notify the debug log that the choice couldn't be pressed.
        }
                
    }

    public void AddItemToInventory(List<StoryScene.InventoryEntry> rewards)
    {
        foreach (var rewardItem in rewards)
        {
            StoryScene.InventoryEntry itemHandler = rewardItem;
            if (rewardItem.itemCount == 0) //Failsafe for if you forget to input a count
            {
                Debug.Log("Oh careful! Can't have zero " + rewardItem.itemData.name + "!");
                itemHandler.itemCount = 1;
            }
            
            if (inventory.Count == 0) //If you've got nothing in your inventory...
            {
                Debug.Log("Lots of space in your bag, eh?");
                inventory.Add(itemHandler); //Add it to your inventory.
            }
            else
            {
                Debug.Log("Let's see if we can fit it in there...");
                
                bool match = false;
                
                for(int i = 0; i < inventory.Count; i++) //Compare against all items in your inventory...
                {
                    StoryScene.InventoryEntry thisItem = inventory[i];
                    if (itemHandler.itemData.name == inventory[i].itemData.name) //If you already have the item...
                    {
                        Debug.Log("Oh hey, you've already got this one!");
                        match = true;
                        thisItem.itemCount += itemHandler.itemCount; //Add to the count.
                        inventory.RemoveAt(i);
                        inventory.Add(thisItem);
                        
                        break; //Stop checking items
                    }
                }
                
                if (!match) //If you didn't already have it...
                {
                    Debug.Log("First time?");
                    inventory.Add(itemHandler); //Add it to your inventory.
                }
            }
        }
    }
    
    
}

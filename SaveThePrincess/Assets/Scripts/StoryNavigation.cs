using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject[] buttons;
    

    private bool[] globalValid;

    private TextMeshProUGUI[] buttonText;

    private string currentScene = "Start";
    private string[] sceneTags;

    [Space]
    
    public TextMeshProUGUI printInventory;
    public List<string> inventory;
    public List<string> secretInventory;

    [Space] private int day = 1;
    [SerializeField] private int money = 10;
    

    // Start is called before the first frame update
    void Start()
    {
        titleText = TitleUI.GetComponent<TextMeshProUGUI>();
        bodyText = bodyUI.GetComponent<TextMeshProUGUI>(); //Set the title and body text components for easier access.
        statusText = statusUI.GetComponent<TextMeshProUGUI>();

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
        statusText.text = "Day: " + day + "\n Coins: " + money;
        renderChoices();

        //Print Inventory
        printInventory.text = " "; //Reset it so it doesn't just keep repeating
        foreach (string Item in inventory)
        {
            printInventory.text += Item + "\n";
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
            if (Scenes[location].sceneChoices[i].destination != "None") //If the option exists...
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
                    buttons[i].SetActive(true);
                    buttonText[i].text = Scenes[location].sceneChoices[i].choiceText; //Activate the button and set it's text.
                    buttonText[i].color = Color.gray; //Make it grey
                    globalValid[i] = false;
                }
            }
        }

        for (int i = Scenes[location].sceneChoices.Length; i < buttons.Length; i++) //Clean up the buttons that aren't being used
        {
            buttons[i].SetActive(false); //By setting them to inactive
            globalValid[i] = false; //And making them unpressable.
        }
    }
    

    public bool IsChoiceValid(StoryScene.choice choice) //This determines whether a given choice is selectable or not.
    {
        if (choice.tool != "None")
        {
            bool hasTool = false;
            foreach (string item in inventory) //For each item in the inventory...
            {
                if (item == choice.tool) //If it's the tool you need to choose the option...
                {
                    hasTool = true; //Mark that you have the tool.
                }
            }
            foreach (string item in secretInventory) //For each item in the secret inventory...
            {
                if (item == choice.tool) //If it's the tool you need to choose the option...
                {
                    hasTool = true; //Mark that you have the tool.
                }
            }

            if (hasTool == false) //If you didn't have the tool...
            {
                return false; //Then return false.
            }
        }
        
        
        if (choice.lockoutTool != "None") //If there's a tool that prevents you from picking the option...
        {
            foreach (string item in inventory) //For each item in the inventory...
            {
                if (item == choice.lockoutTool) //If it's the tool that locks the option...
                {
                    return false; //Then return false.
                }
            }
            foreach (string item in secretInventory) //For each item in the secret inventory...
            {
                if (item == choice.lockoutTool) //If it's the tool that locks the option...
                {
                    return false; //Then return false.
                }
            }
        }

        if (money < choice.coinCost) //If you don't have enough money...
        {
            return false; //Then return false.
        }

        return true; //If none of the above are false, then return true.
    }
    

    public void ButtonInput(int input) //This handles the input from the buttons.
    {
        StoryScene.choice chosen = Scenes[findIndex(currentScene)].sceneChoices[input]; //Convert the input into a quick choice variable.
        
        if (globalValid[input]) //If it's valid (determined earlier)
        {
            money -= chosen.coinCost; //Subtract the coin cost from your coin total.
            if (chosen.advancesDay) //If the choice advances the day...
            {
                day++; //Increment the day variable.
            }
            
            if (chosen.choiceReward != "None") //If the choice has a reward...
            {
                inventory.Add(chosen.choiceReward); //Add it to your inventory.
            }
        
            if (chosen.secretReward != "None") //And ditto with secret rewards.
            {
                secretInventory.Add(chosen.secretReward);
            }
            
            currentScene = chosen.destination; //Move to the destination.
        }
        else
        {
            Debug.Log( input + " goes Boink"); //Notify the debug log that the choice couldn't be pressed.
        }
                
    }
}

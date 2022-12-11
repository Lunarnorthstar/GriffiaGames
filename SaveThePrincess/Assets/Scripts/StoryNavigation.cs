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

    public GameObject[] buttons;
    

    private bool[] globalValid;

    private TextMeshProUGUI[] buttonText;

    private string currentScene = "Start";
    private string[] sceneTags;

    [Space]
    
    public TextMeshProUGUI printInventory;
    public List<string> inventory;

    [Space] private int day = 1;
    private int money = 10;
    

    // Start is called before the first frame update
    void Start()
    {
        titleText = TitleUI.GetComponent<TextMeshProUGUI>();
        bodyText = bodyUI.GetComponent<TextMeshProUGUI>(); //Set the title and body text components for easier access.

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
                bool valid = false; //Initialize a bool to track the valid state of the button. This is for cleaner code, mainly.
                
                foreach (string item in inventory) //For each item in the inventory...
                {
                    if (item == Scenes[location].sceneChoices[i].tool) //If it's the tool you need to choose the option...
                    {
                        valid = true; //Then the option is valid.
                    }
                }

                if ((Scenes[location].sceneChoices[i].tool == "None" || valid) && money >= Scenes[location].sceneChoices[i].coinCost) //If it's selectable (no requirement or you have the requirement) and you have enough money...
                {
                    buttons[i].SetActive(true); //Set the button to active
                    buttonText[i].text = Scenes[location].sceneChoices[i].choiceText; //Activate the button and set it's text.
                    buttonText[i].color = Color.black; //Make it black
                    globalValid[i] = true; //Set the bool to allow the button to be selectable
                }
                else if ((Scenes[location].sceneChoices[i].tool != "None" && !valid) || money < Scenes[location].sceneChoices[i].coinCost) //If it's not selectable (has a requirement that you do not have) or you don't have the money
                {
                    buttons[i].SetActive(true);
                    buttonText[i].text = Scenes[location].sceneChoices[i].choiceText; //Activate the button and set it's text.
                    buttonText[i].color = Color.gray; //Make it grey
                    globalValid[i] = false;
                }
            }
        }

        for (int i = Scenes[location].sceneChoices.Length; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
            globalValid[i] = false;
        }
    }




    public void ButtonInput(int input)
    {
        int location = findIndex(currentScene);
        
        if (globalValid[input])
        {
            money -= Scenes[location].sceneChoices[input].coinCost;
            if (Scenes[location].sceneChoices[input].advancesDay)
            {
                day++;
            }
            
            currentScene = Scenes[location].sceneChoices[input].destination;
            PickUp();
        }
        else
        {
            Debug.Log( input + " goes Boink");
        }
                
    }


    private void PickUp()
    {
        int location = findIndex(currentScene);
        if (Scenes[location].sceneReward != "None")
        {
            inventory.Add(Scenes[location].sceneReward);
        }
    }
}

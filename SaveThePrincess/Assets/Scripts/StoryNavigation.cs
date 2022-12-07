using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryNavigation : MonoBehaviour
{
    public StoryScene[] Scenes;
    public GameObject TitleUI;
    private Text titleText;
    public GameObject bodyUI;
    private Text bodyText;

    public GameObject buttonA;
    public GameObject buttonB;
    public GameObject buttonC;

    private bool aValid;
    private bool bValid;
    private bool cValid;

    private Text buttonAText;
    private Text buttonBText;
    private Text buttonCText;

    private string currentScene = "Start";
    private string[] sceneTags;

    public List<string> inventory;


    // Start is called before the first frame update
    void Start()
    {
        titleText = TitleUI.GetComponent<Text>();
        bodyText = bodyUI.GetComponent<Text>();
        buttonAText = buttonA.GetComponentInChildren<Text>();
        buttonBText = buttonB.GetComponentInChildren<Text>();
        buttonCText = buttonC.GetComponentInChildren<Text>(); //Get all the text components for easier access

        sceneTags = new string[Scenes.Length]; //Initialize the tags array
        for (int i = 0; i < Scenes.Length; i++) //For each one...
        {
            sceneTags[i] = Scenes[i].sceneTag; //Get the story scene's tag for easier reference.
        }
        
        titleText.text = Scenes[findIndex(currentScene)].sceneTitle; //Set the title text
        bodyText.text = Scenes[findIndex(currentScene)].sceneDescription; //And the body text
        
        renderChoices(); //Render the buttons.
    }

    // Update is called once per frame
    void Update()
    {
        titleText.text = Scenes[findIndex(currentScene)].sceneTitle; //Set the title text
        bodyText.text = Scenes[findIndex(currentScene)].sceneDescription; //And the body text
        renderChoices();
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

        return 0; //Go back to zero if it's not found.
    }

    private void renderChoices() //This takes the button information and displays it on the buttons in the scene.
    {
        int location = findIndex(currentScene); //Get the scene index for easier use
        
        
        if (Scenes[location].ChoiceA != "None") //If option A exists...
        {
            bool valid = false;

            foreach (string item in inventory)
            {
                if (item == Scenes[location].ATool)
                {
                    valid = true;
                }
            }

            if (Scenes[location].ATool == "None" || valid) //If it's selectable (no requirement or you have the requirement)
            {
                buttonA.SetActive(true);
                buttonAText.text = Scenes[location].ChoiceA; //Activate the button and set it's text.
                buttonAText.color = Color.black; //Make it grey
                aValid = true;
            }
            else if (Scenes[location].ATool != "None" && !valid) //If it's not selectable (has a requirement that you do not have)
            {
                buttonA.SetActive(true);
                buttonAText.text = Scenes[location].ChoiceA; //Activate the button and set it's text.
                buttonAText.color = Color.gray; //Make it grey
                aValid = false;
            }
        }
        else
        {
            buttonA.SetActive(false); //If it doesn't, deactivate the button.
            aValid = false;
        }
        //Same for these other two, but with the other buttons.
        if (Scenes[location].ChoiceB != "None")
        {
            bool valid = false;

            foreach (string item in inventory)
            {
                if (item == Scenes[location].BTool)
                {
                    valid = true;
                }
            }

            if (Scenes[location].BTool == "None" || valid) //If it's selectable (no requirement or you have the requirement)
            {
                buttonB.SetActive(true);
                buttonBText.text = Scenes[location].ChoiceB; //Activate the button and set it's text.
                buttonBText.color = Color.black; //Make it grey
                bValid = true;
            }
            else if (Scenes[location].BTool != "None" && !valid) //If it's not selectable (has a requirement that you do not have)
            {
                buttonB.SetActive(true);
                buttonBText.text = Scenes[location].ChoiceB; //Activate the button and set it's text.
                buttonBText.color = Color.gray; //Make it grey
                bValid = false;
            }
        }
        else
        {
            buttonB.SetActive(false);
            bValid = false;
        }
        
        if (Scenes[location].ChoiceC != "None")
        {
            bool valid = false;

            foreach (string item in inventory)
            {
                if (item == Scenes[location].CTool)
                {
                    valid = true;
                }
            }

            if (Scenes[location].CTool == "None" || valid) //If it's selectable (no requirement or you have the requirement)
            {
                buttonC.SetActive(true);
                buttonCText.text = Scenes[location].ChoiceC; //Activate the button and set it's text.
                buttonCText.color = Color.black; //Make it grey
                cValid = true;
            }
            else if (Scenes[location].CTool != "None" && !valid) //If it's not selectable (has a requirement that you do not have)
            {
                buttonC.SetActive(true);
                buttonCText.text = Scenes[location].ChoiceC; //Activate the button and set it's text.
                buttonCText.color = Color.gray; //Make it grey
                cValid = false;
            }
        }
        else
        {
            buttonC.SetActive(false);
            cValid = false;
        }
    }

    public void ButtonInput(string input)
    {
        int location = findIndex(currentScene);
        switch (input)
        {
            case "a":
                if (aValid)
                {
                    currentScene = Scenes[location].ADestination;
                    PickUp();
                }
                else
                {
                    Debug.Log("A goes Boink");
                }
                break;
            case "b" :
                if (bValid)
                {
                    currentScene = Scenes[location].BDestination;
                    PickUp();
                }
                else
                {
                    Debug.Log("B goes Boink");
                }
                break;
            case "c" :
                if (cValid)
                {
                    currentScene = Scenes[location].CDestination;
                    PickUp();
                }
                else
                {
                    Debug.Log("C goes Boink");
                }
                break;
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

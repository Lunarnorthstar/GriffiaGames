using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Story Scene", menuName = "ScriptableObjects/Story Scene", order = 1)]


public class StoryScene : ScriptableObject
{
    [Tooltip("The internal name for the scene looked for by the game when moving between scenes")]
    public string sceneTag;

    [Space]

    [Tooltip("The name displayed on the header text for the scene")]
    public string sceneTitle;
    [Tooltip("The body text of the scene")]
    [TextArea(15, 20)] public string sceneDescription;

    [Tooltip("The scene background if it changes the background")]
    public Sprite backgroundImage;

    [Tooltip("Whether the scene uses the top, centered, button.")]
    public bool usesMasterButton;
    
    [System.Serializable]
    public struct Item
    {
        public string name;
        public bool persists;
        public bool isSecret;
    }
    
    [System.Serializable]
    public struct choice
    {
        [Tooltip("The text displayed on the topmost button")]
        public string choiceText;
        [Tooltip("The tag of the scene the topmost button will take you to")]
        public string destination;
        [Tooltip("The tag of the item required for this button")]
        public string[] tools;
        [Tooltip("The tag of the item that will prevent you from using the button")]
        public string[] lockoutTools;
        [Tooltip("Whether the choice advances the day counter")]
        public bool advancesDay;

        public int coinCost;

        public Item[] rewards;
    }
    [Tooltip("Input 'None' (case sensitive) for no tool requirement")]
    public choice[] sceneChoices;





}

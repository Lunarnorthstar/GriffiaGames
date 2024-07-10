using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Story Scene", menuName = "StorythreadSO/Story Scene", order = 1)]


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

    [Tooltip("Whether the scene uses the topmost button.")]
    public bool usesMasterButton;
    
    [System.Serializable]
    public struct InventoryEntry //For each item in your inventory.
    {
        [Tooltip("Which ScriptableObject this item is")]
        public Item itemData;
        [Tooltip("How much of it you have")]
        public int itemCount;
    }

    [System.Serializable]
    public struct toolData //For each item required by a choice.
    {
        [Tooltip("The trait for this requirement")]
        public string trait;
        [Tooltip("How many you need")]
        public int amount;
        [Tooltip("Whether it's consumed by the choice")]
        public bool consumesItem;
    }

    [System.Serializable]
    public struct forbidData //For each item that locks you out of a choice.
    {
        [Tooltip("The trait this barrier looks for")]
        public string trait;
        [Tooltip("If set to a number above 0, having total matching traits lower than this will still allow you to select this option.")]
        public int maximum;
    }
    
    [System.Serializable]
    public struct choice
    {
        [Tooltip("The text displayed on the button")]
        public string choiceText;
        [Tooltip("The tag of the scene the button will take you to")]
        public string destination;
        [Tooltip("The tag of the item required for this button")]
        public toolData[] tools;
        public int coinCost;
        [Tooltip("The tag of the item that will prevent you from using the button")]
        public forbidData[] lockoutTools;
        [Tooltip("The day at which this option will become available")]
        public int lowerTimeBound;
        [Tooltip("The day after which this option will become unavailable")]
        public int upperTimeBound;
        [Tooltip("Whether the choice is hidden if the requirements are not met")]
        public bool hideWhenInvalid;
        [Tooltip("Whether the choice advances the day counter")]
        public bool advancesDay;

        [Tooltip("What items this choice will give the player")]
        public InventoryEntry[] rewards;
    }
    
    public choice[] sceneChoices;





}

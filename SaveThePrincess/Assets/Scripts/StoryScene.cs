using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Scene", menuName = "ScriptableObjects/Story Scene", order = 1)]


public class StoryScene : ScriptableObject
{
    [Tooltip("The internal name for the scene looked for by the game when moving between scenes")]
    public string sceneTag;

    [Space] 
    
    [Tooltip("The item received when arriving in the scene")]  public string sceneReward = "None";
    
    [Space]
    
    [Tooltip("The name displayed on the header text for the scene")]
    public string sceneTitle;
    [Tooltip("The body text of the scene")]
    [TextArea(15, 20)] public string sceneDescription;
    
    [Space]
    
    [Tooltip("The text displayed on the topmost button")]
    public string ChoiceA;
    [Tooltip("The tag of the scene the topmost button will take you to")]
    public string ADestination;
    [Tooltip("The tag of the item required for this button")]
    public string ATool = "None";
    
    [Space]
    
    [Tooltip("The text displayed on the middle button")]
    public string ChoiceB;
    [Tooltip("The tag of the scene the middle button will take you to")]
    public string BDestination;
    [Tooltip("The tag of the item required for this button")]
    public string BTool = "None";
    
    [Space]
    
    [Tooltip("The text displayed on the bottom button")]
    public string ChoiceC;
    [Tooltip("The tag of the scene the bottom button will take you to")]
    public string CDestination;
    [Tooltip("The tag of the item required for this button")]
    public string CTool = "None";


}

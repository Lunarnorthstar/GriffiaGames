using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Item", menuName = "StorythreadSO/Story Item", order = 2)]

public class Item : ScriptableObject
{
    [Tooltip("The display name of the item")]
    public string name;
    [Tooltip("Whether the item will remain in your inventory after reaching an ending")]
    public bool persists;
    [Tooltip("Whether the item will be hidden from the inventory screen")]
    public bool isSecret;
    [Tooltip("For designating what an item can be used for. For example, an axe would be given the 'chopping' trait. This is used to find things to use the item for.")]
    public string[] traits;

}

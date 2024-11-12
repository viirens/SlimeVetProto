using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeItemRequirement", menuName = "Data/Slime Item Requirement")]
public class SlimeItemRequirement : ScriptableObject
{
    public List<Item> possibleItems;

    public List<Item> GetItemsByTier(int tier)
    {
        Debug.Log("Getting items by tier: " + tier);
        List<Item> tieredItems = new List<Item>();
        foreach (Item item in possibleItems)
        {
            Debug.Log("Item tier: " + item.tier);
            if (item.tier == tier)
            {
                tieredItems.Add(item);
            }
        }
        return tieredItems;
    }
}


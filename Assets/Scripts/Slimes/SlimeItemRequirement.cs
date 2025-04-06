using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeItemRequirement", menuName = "Data/Slime Item Requirement")]
public class SlimeItemRequirement : ScriptableObject
{
    public List<Item> possibleItems;

    public List<Item> GetItemsByTier(int tier)
    {
        List<Item> tieredItems = new List<Item>();
        foreach (Item item in possibleItems)
        {
            if (item.tiers != null && item.tiers.Contains(tier))
            {
                tieredItems.Add(item);
            }
        }
        return tieredItems;
    }
}


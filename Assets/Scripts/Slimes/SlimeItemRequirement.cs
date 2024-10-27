using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeItemRequirement", menuName = "Data/Slime Item Requirement")]
public class SlimeItemRequirement : ScriptableObject
{
    public List<Item> possibleItems;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public bool Stackable;
    public ToolAction onAction;
    [SerializeField] public ResourceNodeType resourceNodeType;
}

using System.Collections.Generic;
using UnityEngine;

public abstract class ToolAction : ScriptableObject
{
    [SerializeField] private List<ResourceNodeType> canHitNodesOfType;

    public List<ResourceNodeType> GetResourceNodeTypes()
    {
        return canHitNodesOfType;
    }

    public virtual bool OnApply(Vector2 position)
    {
        Debug.Log("OnApply is not implemented");
        return true;
    }
}

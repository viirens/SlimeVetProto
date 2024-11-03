using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAction : ScriptableObject
{
    public virtual bool OnApply(Vector2 position)
    {
        Debug.Log("OnApply is not implemented");
        return true;
    }
}

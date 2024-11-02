using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHit : MonoBehaviour
{
    public virtual void Hit()
    {
        Debug.Log("Hit");
    }

    public virtual bool CanBeHit(List<ResourceNodeType> canBeHitByTool)
    {
        return true;
    }

    public virtual void InteractHold(Character character)
    {
        // Default implementation (can be empty)
    }

    public virtual void InteractReleased(Character character)
    {
        // Default implementation (can be empty)
    }
}

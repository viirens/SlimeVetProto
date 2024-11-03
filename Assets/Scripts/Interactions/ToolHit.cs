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
}

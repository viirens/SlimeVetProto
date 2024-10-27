using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType { Undefined, Tree, Ore }

[CreateAssetMenu(fileName = "New Gather Resource Node", menuName = "Data/Gather Resource Node")]

public class GatherResourceNode : ToolAction
{
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] List<ResourceNodeType> canHitNodesOfType;
    public override bool OnApply(Vector2 position)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            ToolHit toolHit = collider.GetComponent<ToolHit>();
            if (toolHit != null)
            {
                if (toolHit.CanBeHit(canHitNodesOfType))
                {
                    toolHit.Hit();
                    return true;
                }
            }
        }

        return false;
    }
}

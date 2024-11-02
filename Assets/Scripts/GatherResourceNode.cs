using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gather Resource Node", menuName = "Data/Gather Resource Node")]
public class GatherResourceNode : ToolAction
{
    [SerializeField] private float sizeOfInteractableArea = 1.2f;

    public override bool OnApply(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            ToolHit toolHit = collider.GetComponent<ToolHit>();
            if (toolHit != null)
            {
                if (toolHit.CanBeHit(GetResourceNodeTypes()))
                {
                    toolHit.Hit();
                    return true;
                }
            }
        }

        return false;
    }
}

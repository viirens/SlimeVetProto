using System.Collections.Generic;
using UnityEngine;

public class ToolsCharacterController : MonoBehaviour
{
    CharacterController2D character;
    Rigidbody2D rgb2d;
    ToolbarController toolbarController;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;

    void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rgb2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            InteractHold();
        }

        if (Input.GetMouseButton(1))
        {
            InteractHold();
        }

        if (Input.GetMouseButtonUp(1))
        {
            InteractReleased();
        }
    }

    void InteractHold()
    {
        Vector2 position = rgb2d.position + character.lastMotionVector * offsetDistance;
        Item item = toolbarController.GetItem;
        if (item == null) { return; }
        ToolAction onAction = item.onAction;
        if (onAction == null) { return; }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            ToolHit toolHit = collider.GetComponent<ToolHit>();
            if (toolHit != null)
            {
                if (toolHit.CanBeHit(onAction.GetResourceNodeTypes()))
                {
                    toolHit.InteractHold(null);
                    break;
                }
            }
        }
    }

    void InteractReleased()
    {
        Vector2 position = rgb2d.position + character.lastMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            ToolHit toolHit = collider.GetComponent<ToolHit>();
            if (toolHit != null)
            {
                toolHit.InteractReleased(null);
                break;
            }
        }
    }
}

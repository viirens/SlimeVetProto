using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsCharacterController : MonoBehaviour
{
    CharacterController2D character;
    Rigidbody2D rgb2d;
    ToolbarController toolbarController;
    [SerializeField] float offsetDistance = 1f;
    // [SerializeField] float sizeOfInteractableArea = 1.2f;
    ToolsCharacterController charController;

    void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rgb2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseTool();
        }
    }

    private bool UseTool()
    {
        Vector2 position = rgb2d.position + character.lastMotionVector * offsetDistance;

        // Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        // foreach (Collider2D collider in colliders)
        // {
        //     ToolHit toolHit = collider.GetComponent<ToolHit>();
        //     if (toolHit != null)
        //     {
        //         Vector2 directionToCollider = (collider.transform.position - transform.position).normalized;
        //         if (Vector2.Dot(character.lastMotionVector, directionToCollider) > 0.5f) // Check if facing the object
        //         {
        //             toolHit.Hit();
        //             break;
        //         }
        //     }
        
        Item item = toolbarController.GetItem;
        if (item == null) { return false; }
        if (item.onAction == null) { return false; }

        bool complete = item.onAction.OnApply(position);
        return complete;
    }
}

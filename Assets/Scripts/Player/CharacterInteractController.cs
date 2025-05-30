using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    CharacterController2D characterController;
    Rigidbody2D rgb2d;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    Character character;
    [SerializeReference] HighlightController highlightController;

    public Interactable CurrentInteractable { get; private set; }

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        rgb2d = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    void Update()
    {
        CheckForInteractableObjects();
        // Debug.Log("Interacting with object");

        // to consolidate
        if (Input.GetMouseButtonDown(1))
        {
            
            Vector2 position = rgb2d.position + characterController.lastMotionVector * offsetDistance;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

            foreach (Collider2D collider in colliders)
            {
                Interactable interactable = collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (!collider.CompareTag("Slime"))
                    {
                        Interact();
                    }
                    else
                    {
                        InteractHold();
                    }
                    break;
                }
            }
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

    private void CheckForInteractableObjects()
    {
        Vector2 position = rgb2d.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        Interactable closestInteractable = null;

        foreach (Collider2D collider in colliders)
        {
            Interactable hit = collider.GetComponent<Interactable>();
            if (hit != null)
            {
                if (closestInteractable == null || Vector2.Distance(transform.position, hit.transform.position) < Vector2.Distance(transform.position, closestInteractable.transform.position))
                {
                    closestInteractable = hit;
                }
            }
        }

        if (closestInteractable != null)
        {
            highlightController.Highlight(closestInteractable.gameObject);
            CurrentInteractable = closestInteractable;
        }
        else
        {
            highlightController.Hide();
            CurrentInteractable = null;
        }
    }

    void Interact()
    {
        Vector2 position = rgb2d.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                Vector2 directionToCollider = (collider.transform.position - transform.position).normalized;
                if (Vector2.Dot(characterController.lastMotionVector, directionToCollider) > 0.5f) // Check if facing the object
                {
                    interactable.Interact(character);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (rgb2d == null || characterController == null) return;

        // Calculate the position of the overlap circle
        Vector2 position = rgb2d.position + characterController.lastMotionVector * offsetDistance;

        // Set the color for the gizmo
        Gizmos.color = Color.green;

        // Draw the overlap circle
        Gizmos.DrawWireSphere(position, sizeOfInteractableArea);
    }

    void InteractHold()
    {
        Vector2 position = rgb2d.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                Vector2 directionToCollider = (collider.transform.position - transform.position).normalized;
                if (Vector2.Dot(characterController.lastMotionVector, directionToCollider) > 0.5f)
                {
                    interactable.InteractHold(character);
                    break;
                }
            }
        }
    }

    void InteractReleased()
    {
        Vector2 position = rgb2d.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                Vector2 directionToCollider = (collider.transform.position - transform.position).normalized;
                if (Vector2.Dot(characterController.lastMotionVector, directionToCollider) > 0.5f)
                {
                    interactable.InteractReleased(character);
                    break;
                }
            }
        }
    }
}

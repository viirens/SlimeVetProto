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
    // ToolsCharacterController charController;

    private HighlightController highlightController;

    private bool isToolHeld = false;
    private float toolHoldProgress = 0f;
    private float hitTime = 2f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    private GameObject currentTarget;

    void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rgb2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
    }

    void Start()
    {
        highlightController = FindObjectOfType<HighlightController>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            UseToolHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            UseToolReleased();
        }
    }

    private bool UseTool()
    {
        Debug.Log("UseTool");
        Vector2 position = rgb2d.position + character.lastMotionVector * offsetDistance;
        
        Item item = toolbarController.GetItem;
        if (item == null) { return false; }
        if (item.onAction == null) { return false; }

        bool complete = item.onAction.OnApply(position);
        return complete;
    }

    private bool UseToolHold()
    {
        // Debug.Log("UseToolHold");
        // Debug.Log(toolHoldProgress);
        Item item = toolbarController.GetItem;
        if (item == null) { return false; }
        List<ResourceNodeType> canHitNodesOfType = GetCanHitNodesOfType(item.onAction);
        GameObject newTarget = GetInRangeResourceNode(canHitNodesOfType);
        Debug.Log(newTarget);

        if (!isToolHeld)
        {
            if (newTarget != null)
            {
                currentTarget = newTarget;
                Debug.Log(currentTarget.name);
                highlightController.ShowProgressBar(currentTarget, currentTarget.transform.position);
                isToolHeld = true;
                toolHoldProgress = 0f;
            }
        }   

        toolHoldProgress += Time.deltaTime;
        if (currentTarget != null)
        {
            highlightController.UpdateProgressBar(currentTarget, toolHoldProgress / hitTime);
        }
        if (toolHoldProgress >= hitTime)
        {
            
            if (item == null) { return false; }
            if (item.onAction == null) { return false; }
            Vector2 position = rgb2d.position + character.lastMotionVector * offsetDistance;
            item.onAction.OnApply(position);
            isToolHeld = false;
            toolHoldProgress = 0f;
            highlightController.RemoveProgressBar(currentTarget);
        }
        return true;
    }

    private bool UseToolReleased()
    {
        Debug.Log("UseToolReleased");
        if (isToolHeld)
        {
            if (currentTarget != null)
            {
                highlightController.RemoveProgressBar(currentTarget);
            }
            isToolHeld = false;
            toolHoldProgress = 0f;
        }
        return false;
    }

    private GameObject GetInRangeResourceNode(List<ResourceNodeType> canHitNodesOfType)
    {
        Vector2 position = rgb2d.position + character.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);
        foreach (Collider2D collider in colliders)
        {
            ToolHit toolHit = collider.GetComponent<ToolHit>();
            if (toolHit != null)
            {
                if (toolHit.CanBeHit(canHitNodesOfType))
                {
                    return toolHit.gameObject;
                }
            }
        }
        return null;
    }

    private List<ResourceNodeType> GetCanHitNodesOfType(ToolAction toolAction)
    {
        if (toolAction is GatherResourceNode gatherAction)
        {
            return gatherAction.canHitNodesOfType;
        }
        return new List<ResourceNodeType>();
    }
}

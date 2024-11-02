using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class ResourceNode : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] int dropCount = 5;
    [SerializeField] int itemCountInOneDrop = 1;
    [SerializeField] float spread = 0.7f;
    [SerializeField] Item item;
    [SerializeField] ResourceNodeType nodeType;

    [SerializeField] float gatheringDuration = 3f;
    private bool isBeingGathered = false;
    private float gatheringProgress = 0f;
    private HighlightController highlightController;

    void Start()
    {
        highlightController = FindObjectOfType<HighlightController>();
    }

    public override void InteractHold(Character character)
    {
        if (!isBeingGathered)
        {
            isBeingGathered = true;
            highlightController.ShowProgressBar(transform.position);
        }

        gatheringProgress += Time.deltaTime;
        highlightController.UpdateProgressBar(gatheringProgress / gatheringDuration);

        if (gatheringProgress >= gatheringDuration)
        {
            CompleteGathering();
        }
    }

    public override void InteractReleased(Character character)
    {
        if (isBeingGathered)
        {
            isBeingGathered = false;
            gatheringProgress = 0f;
            highlightController.HideProgressBar();
        }
    }

    private void CompleteGathering()
    {
        isBeingGathered = false;
        DropResources();
        highlightController.HideProgressBar();
        Destroy(gameObject);
    }

    private void DropResources()
    {
        while (dropCount > 0)
        {
            dropCount--;

            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2; 
            position.y += spread * UnityEngine.Random.value - spread / 2;

            GameManager.instance.itemSpawnManager.SpawnItem(position, item, itemCountInOneDrop);
        }
    }

    public override bool CanBeHit(List<ResourceNodeType> canBeHitByTool)
    {
        return canBeHitByTool.Contains(nodeType);
    }
}

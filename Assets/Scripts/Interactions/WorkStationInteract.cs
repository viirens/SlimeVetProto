using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationInteract : Interactable
{
    [SerializeField] private WorkStationType workStationType;
    [SerializeField] private ItemContainerMono itemContainer;
    [SerializeField] private GameObject itemSlotSpriteOne;
    [SerializeField] private GameObject itemSlotSpriteTwo;

    private bool isCooking = false;
    [SerializeField] private float cookingTime = 5f;

    private HighlightController highlightController;

    void Start()
    {
        isInteractable = true;
        highlightController = FindObjectOfType<HighlightController>();
    }

    public override void Interact(Character character)
    {
        switch (workStationType)
        {
            case WorkStationType.Fireplace:
                InteractWithFireplace(character);
                break;
            case WorkStationType.Stovetop:
                InteractWithStovetop(character);
                break;
            default:
                Debug.Log("Interacted with unknown workstation");
                break;
        }
    }

    private void InteractWithFireplace(Character character)
    {
        TransferItemToContainer(character);
    }

    private void InteractWithStovetop(Character character)
    {
        TransferItemToContainer(character);
    }

    private void TransferItemToContainer(Character character)
    {
        Item selectedItem = character.GetSelectedItem();

        if (selectedItem != null &&
            ((workStationType == WorkStationType.Fireplace && (selectedItem.resourceNodeType == ResourceNodeType.Tree || selectedItem.resourceNodeType == ResourceNodeType.Herb)) ||
             (workStationType == WorkStationType.Stovetop && (selectedItem.resourceNodeType == ResourceNodeType.Ore || selectedItem.resourceNodeType == ResourceNodeType.Herb))))
        {
            bool added = itemContainer.Add(selectedItem, 1);
            if (added)
            {
                SpriteRenderer itemSpriteRenderer = null;
                if (itemSlotSpriteOne.GetComponent<SpriteRenderer>().sprite == null)
                {
                    itemSpriteRenderer = itemSlotSpriteOne.GetComponent<SpriteRenderer>();
                }
                else if (itemSlotSpriteTwo.GetComponent<SpriteRenderer>().sprite == null)
                {
                    itemSpriteRenderer = itemSlotSpriteTwo.GetComponent<SpriteRenderer>();
                }
                if (itemSpriteRenderer != null)
                {
                    itemSpriteRenderer.sprite = selectedItem.Icon;
                    ShowItemSprite();
                }
                character.RemoveItem(selectedItem, 1);
            }
        }
        else
        {
            Debug.Log("Invalid item for workstation");
        }
    }

    public void ShowItemSprite()
    {
        itemSlotSpriteOne.SetActive(true);
        itemSlotSpriteTwo.SetActive(true);
    }

    private void Update()
    {
        if (!isCooking && itemContainer.slots.Count == 2 && itemContainer.slots[0].item != null && itemContainer.slots[1].item != null)
        {
            StartCooking();
            Debug.Log("Showing progress bar");
            highlightController.ShowProgressBar(gameObject, transform.position);
        }
    }

    public void StartCooking()
    {
        isCooking = true;
        StartCoroutine(CookingCoroutine());
    }

    private IEnumerator CookingCoroutine()
    {
        float currentCookingTime = 0f;
        while (isCooking)
        {
            currentCookingTime += Time.deltaTime;
            highlightController.UpdateProgressBar(gameObject, currentCookingTime / cookingTime);

            if (currentCookingTime >= cookingTime)
            {
                isCooking = false;
                currentCookingTime = 0f;
                highlightController.RemoveProgressBar(gameObject);

                // Clear the items
                itemContainer.slots[0].item = null;
                itemContainer.slots[1].item = null;

                // Clear the item sprites
                itemSlotSpriteOne.GetComponent<SpriteRenderer>().sprite = null;
                itemSlotSpriteTwo.GetComponent<SpriteRenderer>().sprite = null;
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        highlightController.RemoveProgressBar(gameObject);
    }
}

public enum WorkStationType
{
    Fireplace,
    Stovetop
}

public enum FireplaceRequiredItem
{
    Tree,
    Herb
}

public enum StovetopRequiredItem
{
    Ore,
    Herb
}




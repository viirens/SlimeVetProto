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

    [SerializeField] private Item cookedItem;
    private Character character;

    void Start()
    {
        isInteractable = true;
        highlightController = FindObjectOfType<HighlightController>();
        character = FindObjectOfType<Character>();
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
            case WorkStationType.Woodcutter:
                InteractWithWoodcutter(character);
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

    private void InteractWithWoodcutter(Character character)
    {
        TransferItemToContainer(character);
    }

    private void TransferItemToContainer(Character character)
    {
        Item selectedItem = character.GetSelectedItem();

        
        if (selectedItem != null &&
            ((workStationType == WorkStationType.Fireplace && (selectedItem.resourceNodeType == ResourceNodeType.Tree || selectedItem.resourceNodeType == ResourceNodeType.Herb)) ||
             (workStationType == WorkStationType.Stovetop && (selectedItem.resourceNodeType == ResourceNodeType.Ore || selectedItem.resourceNodeType == ResourceNodeType.Herb)) ||
             (workStationType == WorkStationType.Woodcutter && selectedItem.resourceNodeType == ResourceNodeType.Tree))
        )
        {
            bool added = itemContainer.Add(selectedItem, 1);
            if (added)
            {
                SpriteRenderer itemSpriteRenderer = null;
                if (itemSlotSpriteOne.GetComponent<SpriteRenderer>().sprite == null)
                {
                    itemSpriteRenderer = itemSlotSpriteOne.GetComponent<SpriteRenderer>();
                }
                else if (workStationType != WorkStationType.Woodcutter && itemSlotSpriteTwo.GetComponent<SpriteRenderer>().sprite == null)
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
        if (workStationType != WorkStationType.Woodcutter)
        {
            itemSlotSpriteTwo.SetActive(true);
        }
    }

    private void Update()
    {
        if (!isCooking)
        {
            if (workStationType == WorkStationType.Woodcutter && itemContainer.slots.Count >= 1 && itemContainer.slots[0].item != null)
            {
                StartCooking();
                Debug.Log("Showing progress bar");
                highlightController.ShowProgressBar(gameObject, transform.position);
            }
            else if (itemContainer.slots.Count >= 2 && itemContainer.slots[0].item != null && itemContainer.slots[1].item != null)
            {
                Debug.Log(itemContainer.slots[0].item.name + " " + itemContainer.slots[1].item.name);
                if (CheckRecipe(itemContainer.slots[0].item, itemContainer.slots[1].item))
                {
                    StartCooking();
                    Debug.Log("Showing progress bar");
                    highlightController.ShowProgressBar(gameObject, transform.position);
                }
                else
                {
                    Debug.Log("Invalid recipe");
                }
            }
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

                itemContainer.slots[0].item = null;
                if (workStationType != WorkStationType.Woodcutter)
                {
                    itemContainer.slots[1].item = null;
                }

                itemSlotSpriteOne.GetComponent<SpriteRenderer>().sprite = null;
                if (workStationType != WorkStationType.Woodcutter)
                {
                    itemSlotSpriteTwo.GetComponent<SpriteRenderer>().sprite = null;
                }

                Debug.Log(character);
                GameManager.instance.itemSpawnManager.SpawnItem(gameObject.transform.position, cookedItem, 1);
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        highlightController.RemoveProgressBar(gameObject);
    }

    private bool CheckRecipe(Item item1, Item item2)
    {
        Debug.Log(item1.name + " " + item2.name);
        Debug.Log(workStationType);
        if (workStationType == WorkStationType.Fireplace)
        {
            if ((item1.name == "Wood" && item2.name == "Herb") ||
                (item1.name == "Herb" && item2.name == "Wood"))
            {
                return true;
            }
        }
        else if (workStationType == WorkStationType.Stovetop)
        {
            if ((item1.name == "Herb" && item2.name == "Stone") ||
                (item1.name == "Stone" && item2.name == "Herb"))
            {
                return true;
            }
        }
        return false;
    }
}

public enum WorkStationType
{
    Fireplace,
    Stovetop,
    Woodcutter
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




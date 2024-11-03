using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationInteract : Interactable
{
    [SerializeField] private WorkStationType workStationType;
    [SerializeField] private ItemContainerMono itemContainer;
    [SerializeField] private GameObject itemSlotSpriteOne;
    [SerializeField] private GameObject itemSlotSpriteTwo;

    void Start()
    {
        isInteractable = true;
    }

    public override void Interact(Character character)
    {
        switch(workStationType)
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

        Debug.Log("Selected item: " + selectedItem.Name + " Resource node type: " + selectedItem.resourceNodeType);
        if (selectedItem != null && (workStationType == WorkStationType.Fireplace && selectedItem.resourceNodeType == ResourceNodeType.Tree || workStationType == WorkStationType.Fireplace && selectedItem.resourceNodeType == ResourceNodeType.Herb) || (workStationType == WorkStationType.Stovetop && selectedItem.resourceNodeType == ResourceNodeType.Ore || workStationType == WorkStationType.Stovetop && selectedItem.resourceNodeType == ResourceNodeType.Herb))
        {
            bool added = itemContainer.Add(selectedItem, 1);
            if (added)
            {
                SpriteRenderer itemSpriteRenderer = null;
                if (itemSlotSpriteOne.GetComponent<SpriteRenderer>().sprite == null) {
                    itemSpriteRenderer = itemSlotSpriteOne.GetComponent<SpriteRenderer>();
                } else if (itemSlotSpriteTwo.GetComponent<SpriteRenderer>().sprite == null) {
                    itemSpriteRenderer = itemSlotSpriteTwo.GetComponent<SpriteRenderer>();
                }
                if (itemSpriteRenderer != null)
                {
                    itemSpriteRenderer.sprite = selectedItem.Icon;
                    ShowItemSprite();
                }
                character.RemoveItem(selectedItem, 1);
            }
        } else {
            Debug.Log("Invalid item for workstation");
        }
    }

    public void ShowItemSprite() 
    {
        itemSlotSpriteOne.SetActive(true);
        itemSlotSpriteTwo.SetActive(true);
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




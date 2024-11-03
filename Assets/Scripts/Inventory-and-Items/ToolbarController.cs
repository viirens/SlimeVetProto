using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 12;
    [SerializeField] int selectedToolIndex = 0;
    public Action<int> onChange;

    private void OnEnable()
    {
        GameManager.instance.inventoryContainer.OnInventoryChanged += UpdateToolbar;
    }

    private void OnDisable()
    {
        GameManager.instance.inventoryContainer.OnInventoryChanged -= UpdateToolbar;
    }

    public Item GetItem
    {
        get {
            return GameManager.instance.inventoryContainer.slots[selectedToolIndex].item;
        }
    }

    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            if (delta < 0)
            {
                selectedToolIndex++;
                selectedToolIndex = (selectedToolIndex >= toolbarSize) ? 0 : selectedToolIndex;
            }
            else
            {
                selectedToolIndex--;
                selectedToolIndex = (selectedToolIndex < 0) ? toolbarSize - 1 : selectedToolIndex;
            }
            onChange?.Invoke(selectedToolIndex);
        }

        for (int i = 0; i < toolbarSize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedToolIndex = i;
                onChange?.Invoke(selectedToolIndex);
                break;
            }
        }
    }

    public void Set(int id)
    {
        selectedToolIndex = id;
    }

    private void UpdateToolbar()
    {
        // Logic to update the toolbar UI based on the current inventory state
        onChange?.Invoke(selectedToolIndex);
    }
}

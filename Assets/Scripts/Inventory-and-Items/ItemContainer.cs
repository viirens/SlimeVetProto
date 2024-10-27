using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int count;

    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}

[CreateAssetMenu(fileName = "New Item Container", menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;

    public event Action OnInventoryChanged; // Add this line

    public void Add(Item item, int count = 1)
    {
        if (item.Stackable == true)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == item);
            if (itemSlot != null)
            {
                itemSlot.count += count;
            }
            else
            {
                itemSlot = slots.Find(x => x.item == null);
                if (itemSlot != null)
                {
                    itemSlot.item = item;
                    itemSlot.count = count;
                }
            }
        }
        else
        {
            ItemSlot itemSlot = slots.Find(x => x.item == null);
            if (itemSlot != null)
            {
                itemSlot.item = item;
            }
        }
        OnInventoryChanged?.Invoke(); // Trigger event
    }

    public bool HasItem(Item item)
    {
        Debug.Log("Checking inventory for item: " + item.Name);
        foreach (var slot in slots)
        {
            if (slot.item != null)
            {
                Debug.Log("Slot contains: " + slot.item.Name + " x" + slot.count);
            }
            else
            {
                Debug.Log("Slot is empty");
            }

            if (slot.item == item && slot.count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item, int count)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.count >= count)
            {
                slot.count -= count;
                if (slot.count <= 0)
                {
                    slot.Clear();
                }
                OnInventoryChanged?.Invoke(); // Trigger event
                return;
            }
        }
        Debug.LogWarning("Item not found or insufficient quantity to remove.");
    }
}

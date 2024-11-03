using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlotMono
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

public class ItemContainerMono : MonoBehaviour
{
    public List<ItemSlot> slots = new List<ItemSlot>();

    public event Action OnInventoryChanged;

    public int maxSlotNumber = 2;

    public bool Add(Item item, int count = 1)
    {
        if (item.Stackable)
        {
            var itemSlot = slots.Find(x => x.item == item || x.item == null);
            if (itemSlot != null)
            {
                itemSlot.item = item;
                itemSlot.count += count;
            }
            else if (slots.Count < maxSlotNumber)
            {
                slots.Add(new ItemSlot { item = item, count = count });
            }
            else
            {
                return false;
            }
        }
        else if (slots.Count < maxSlotNumber)
        {
            slots.Add(new ItemSlot { item = item, count = 1 });
        }
        else
        {
            return false;
        }
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(Item item)
    {
        foreach (var slot in slots)
        {
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
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }
}
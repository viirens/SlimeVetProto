using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] public ItemContainer inventory;
    [SerializeField] public List<InventoryButton> buttons;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Show();
    }

    public void Init()
    {
        SetIndex();
        Show();
    }

    void SetIndex()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            buttons[i].SetIndex(i);
        }
    }

    public void Show()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            if (inventory.slots[i].item == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(inventory.slots[i]);
            }
        }
    }

    public virtual void OnClick(int id)
    {
        Debug.Log("Clicked on " + id);
    }
}

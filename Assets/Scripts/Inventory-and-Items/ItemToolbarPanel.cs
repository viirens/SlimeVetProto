using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    private void OnEnable()
    {
        Init();
        toolbarController.onChange += Highlight;
        Highlight(0);
        inventory.OnInventoryChanged += Show; 
    }

    private void OnDisable()
    {
        toolbarController.onChange -= Highlight;
        inventory.OnInventoryChanged -= Show; 
    }

    public override void OnClick(int id)
    {
        toolbarController.Set(id);
        Highlight(id);
    }

    int currentSelectedTool;
    
    public void Highlight(int id)
    {
        if (buttons == null || buttons.Count == 0)
            return;

        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[id].Highlight(true);
    }
}

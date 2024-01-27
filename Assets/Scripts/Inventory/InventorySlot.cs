using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;


    void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (transform.childCount == 0)
        {
            inventoryItem.parentAfterDrag = transform;
        }
        else if (GetComponentInChildren<InventoryItem>().item != null && GetComponentInChildren<InventoryItem>().item == inventoryItem.item)
        {
            int totalCount = GetComponentInChildren<InventoryItem>().count + inventoryItem.count;

            if (totalCount <= inventoryItem.item.maxInStack) 
            {
                Destroy(inventoryItem.gameObject);
                GetComponentInChildren<InventoryItem>().count += inventoryItem.count;
                GetComponentInChildren<InventoryItem>().RefreshCount();
            }
            else
            {
                inventoryItem.count = inventoryItem.item.maxInStack;
                inventoryItem.RefreshCount();
                GetComponentInChildren<InventoryItem>().count = totalCount % inventoryItem.item.maxInStack;
                GetComponentInChildren<InventoryItem>().RefreshCount();
            }
        }
    }

    
}

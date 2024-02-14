using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    public Item item;
    public string customData;
    public TextMeshProUGUI countText;

    public Image image;

    public int count = 1;

    public Transform parentAfterDrag;
    public Transform parentBeforeDrag;
    public Transform whileDraggingParent;

    private InventoryItem secondInventoryItem;
    public InventoryManager inventoryManager;
    public InventorySlot currentSlot;

    void Start()
    {
        whileDraggingParent = transform.parent.parent.parent;
        parentBeforeDrag = transform.parent;
    }

    public void InitializeItem(Item newItem, string newCustomData, InventoryManager _inventoryManager, InventorySlot slot)
    {
        item = newItem;
        image.sprite = newItem.image;
        customData = newCustomData;
        inventoryManager = _inventoryManager;
        currentSlot = slot;
        currentSlot.currentInventoryItem = this;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && count > 1)
        {
            int tempCount = count;
            count = count / 2;
            RefreshCount();
            secondInventoryItem = Instantiate(this, transform.parent);
            if (tempCount % 2 == 1)
            {
                count++;
                RefreshCount();
            }

            secondInventoryItem.InitializeItem(item, customData, inventoryManager, currentSlot);
        }

        currentSlot = null;

        image.raycastTarget = false;
        parentBeforeDrag = transform.parent;
        parentAfterDrag = transform.parent;
        transform.SetParent(whileDraggingParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(secondInventoryItem != null && parentAfterDrag == parentBeforeDrag)
        {
            count += secondInventoryItem.count;
            RefreshCount();
            GameObject temp = secondInventoryItem.gameObject;
            secondInventoryItem = null;
            Destroy(temp);
        }
        else if(secondInventoryItem == null && parentAfterDrag != parentBeforeDrag)
        {
            parentBeforeDrag.GetComponent<InventorySlot>().currentInventoryItem = null;
        }
        else if(secondInventoryItem == null && parentAfterDrag == parentBeforeDrag)
        {
            currentSlot = parentBeforeDrag.GetComponent<InventorySlot>();
        }

        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        Player.myPlayer.playerObject.GetComponent<InventoryManager>().UpdateHand();
        Player.myPlayer.playerObject.GetComponent<InventoryManager>().armorSystem.LookForChanges();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Armor tempArmor = item as Armor;
        Tool tempTool = item as Tool;
        if (tempArmor != null)
        {
            inventoryManager.ShowHint(item.itemName + " (" + customData + "/" + tempArmor.durability + ")");
        }
        else if(tempTool != null && tempTool.haveDurability)
        {
            inventoryManager.ShowHint(item.itemName + " (" + customData + "/" + tempTool.durability + ")");
        }
        else
        {
            inventoryManager.ShowHint(item.itemName);
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviourPunCallbacks, IUsableItem
{
    private InventoryItem inventoryItem;
    private ArmorSystem armorSystem;
    public Armor item;

    void Awake()
    {
        if (photonView.IsMine == false)
        {
            this.enabled = false;
        }
    }

    public void Initialize(InventoryItem newInventoryItem)
    {
        inventoryItem = newInventoryItem;
        armorSystem = Player.myPlayer.playerObject.GetComponent<ArmorSystem>();
    }

    public void Update()
    {
        if (Player.myPlayer.playerObject.GetComponent<UiManager>().somePanelTurnedOn) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Equip();
        }
    }

    public void Equip()
    {
        if (armorSystem.inventorySlots[(int)(item.slotType) - 2].currentInventoryItem != null) return;

        inventoryItem.transform.SetParent(armorSystem.inventorySlots[(int)(item.slotType) - 2].transform);
        armorSystem.inventorySlots[(int)(item.slotType) - 2].currentInventoryItem = inventoryItem;
        inventoryItem.currentSlot = armorSystem.inventorySlots[(int)(item.slotType) - 2];
        armorSystem.LookForChanges();

        inventoryItem.inventoryManager.UpdateHand();
    }

    public void Deinitialize()
    {

    }
}

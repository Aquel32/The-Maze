using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IUsableItem
{
    public void Initialize(InventoryItem newInventoryItem);
    public void Deinitialize();
}

public interface ITable
{
    public void Initialize();
}

public class InventoryManager : MonoBehaviourPunCallbacks
{
    public InventorySlot[] inventorySlots;
    public InventoryItem inventoryItemPrefab;

    public int selectedSlot = -1;

    public Transform hand;
    public Transform mainHand;

    public Transform playerCamera;
    public Transform handBoneTarget;
    public bool isThereSomethingInHand;

    public UiManager uiManager;
    public CraftingSystem craftingSystem;

    public List<Item> items;

    public TextMeshProUGUI hintItemNameText;

    public Transform cameraTransform;

    public ArmorSystem armorSystem;
    private void Start()
    {
        uiManager = GetComponent<UiManager>();
        armorSystem = GetComponent<ArmorSystem>();

        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (isThereSomethingInHand)
        {
            handBoneTarget.position = hand.GetChild(0).Find("HandPosition").position;
            handBoneTarget.rotation = hand.GetChild(0).Find("HandPosition").rotation;
        }

        if(playerCamera != null) mainHand.rotation = playerCamera.rotation;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeUIState(AdditionalPanelType.None);
        }

        if(uiManager != null ) if (uiManager.somePanelTurnedOn) return;

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 5)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem(selectedSlot);
        }
    }

    public void ChangeUIState(AdditionalPanelType additionalPanel)
    {
        if (uiManager.currentPanel == Panels.None)
        {
            uiManager.ChangeCurrentPanel(Panels.Inventory);
            uiManager.ChangeAdditionalPanelState(additionalPanel);
        }
        else if (uiManager.currentPanel == Panels.Inventory)
        {
            uiManager.ChangeCurrentPanel(Panels.None);
            uiManager.ChangeAdditionalPanelState(AdditionalPanelType.None);
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0) inventorySlots[selectedSlot].Deselect();

        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        if(inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>())
        {
            ShowHint(inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item.itemName);
        }

        UpdateHand();
    }

    public void UpdateHand()
    {
        InventoryItem inventoryItem = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

        ClearHand();

        if(inventoryItem != null)
        {
            GameObject itemInHandModel = PhotonNetwork.Instantiate(inventoryItem.item.inHandModel.name, Vector3.zero, Quaternion.identity);

            photonView.RPC("InstantiateItemInHandModelRPC", RpcTarget.AllBuffered, itemInHandModel.GetComponent<PhotonView>().ViewID);

            if (itemInHandModel.TryGetComponent(out IUsableItem usableItem))
            {
                usableItem.Initialize(inventoryItem);
            }

            ShowHint(inventoryItem.item.itemName);

            isThereSomethingInHand = true;
            photonView.RPC("SetBoneTargetWeightRPC", RpcTarget.AllBuffered, 1f, handBoneTarget.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    public void SetBoneTargetWeightRPC(float weight, int boneTargetViewID)
    {
        PhotonView.Find(boneTargetViewID).transform.parent.GetComponent<Rig>().weight = weight;
    }

    [PunRPC]
    public void InstantiateItemInHandModelRPC(int itemInHandPhotonView, PhotonMessageInfo pmi)
    {
        Transform itemInHandParent = Player.FindPlayer(pmi.Sender).playerObject.transform.Find("Hand").Find("ItemHolder");
        PhotonView.Find(itemInHandPhotonView).transform.SetParent(itemInHandParent, false);
    }

    public void ClearHand()
    {
        if (hand.childCount > 0) 
        {
            foreach (Transform child in hand)
            {
                if(child.gameObject.TryGetComponent<IUsableItem>(out IUsableItem item))
                {
                    item.Deinitialize();
                }
                PhotonNetwork.Destroy(child.GetComponent<PhotonView>());
            }
        }

        isThereSomethingInHand = false;
        if(handBoneTarget != null) photonView.RPC("SetBoneTargetWeightRPC", RpcTarget.AllBuffered, 0f, handBoneTarget.GetComponent<PhotonView>().ViewID);
    }

    public bool AddItem(Item item, string customData)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.maxInStack && itemInSlot.item.stackable && (inventorySlots[i].slotType == SlotType.All || inventorySlots[i].slotType == item.slotType))
            {
                items.Add(item);
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if(itemInSlot == null && (slot.slotType == SlotType.All || slot.slotType == item.slotType))
            {
                items.Add(item);
                SpawnNewItem(item, slot, customData);
                if (selectedSlot == i) UpdateHand();
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot, string customData)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, slot.transform);
        inventoryItem.InitializeItem(item, customData, this, slot);
    }

    public Item GetItem(int index, bool use)
    {
        InventoryItem itemInSlot = inventorySlots[index].GetComponentInChildren<InventoryItem>();

        if(itemInSlot != null)
        {
            if(use == true)
            {
                items.Remove(itemInSlot.item);
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                    if(selectedSlot == index) { ClearHand(); }
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return itemInSlot.item;
        }
        return null;
    }

    public Item GetItem(Item item, bool use)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].GetComponentInChildren<InventoryItem>() != null && inventorySlots[i].GetComponentInChildren<InventoryItem>().item == item)
            {
                return GetItem(i, use);
            }
        }

        return null;
    }

    public void DropItem(int index)
    {
        InventoryItem inventoryItem = inventorySlots[index].GetComponentInChildren<InventoryItem>();
        if(inventoryItem != null) 
        {
            Vector3 position = transform.Find("Hand").Find("ItemHolder").position;
            if (transform.Find("Hand").Find("ItemHolder").childCount > 0) position = transform.Find("Hand").Find("ItemHolder").GetChild(0).position;

            GameObject handlerObject = PhotonNetwork.Instantiate(inventoryItem.item.handlerPrefab.name, position, Quaternion.identity);
            photonView.RPC("SpawnNewItemHandler", RpcTarget.AllBuffered, inventoryItem.customData, handlerObject.GetComponent<PhotonView>().ViewID);

            GetItem(index, true);
        }
    }

    [PunRPC]
    public void SpawnNewItemHandler(string customData, int handlerViewId)
    {
        ItemHandler handler = PhotonView.Find(handlerViewId).GetComponent<ItemHandler>();
        handler.customData = customData;
    }

    public void DropAllItems()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].transform.childCount > 0)
            {
                DropItem(i);
            }
        }
    }

    public void ShowHint(string text)
    {
        StopAllCoroutines();
        StartCoroutine(ShowHintEnumerator(text));
    }

    private IEnumerator ShowHintEnumerator(string text)
    {
        hintItemNameText.text = text;
        yield return new WaitForSeconds(1);
        hintItemNameText.text = "";
    }
}
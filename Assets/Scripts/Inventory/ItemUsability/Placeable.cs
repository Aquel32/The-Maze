using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviourPunCallbacks, IUsableItem
{
    public GameObject buildPrefab;

    private UiManager uiManager;
    private InventoryManager inventoryManager;

    void Start()
    {
        this.enabled = photonView.IsMine;
    }

    public void Update()
    {
        if (uiManager.somePanelTurnedOn) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Place();
        }
    }

    public void Place()
    {
        Transform attackDirection = inventoryManager.hand.transform.GetChild(0).Find("AttackDirection");
        Ray ray = new Ray(attackDirection.position, attackDirection.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 5f))
        {
            PhotonNetwork.Instantiate(buildPrefab.name, hitInfo.point, Quaternion.identity);
            Player.myPlayer.playerObject.GetComponent<InventoryManager>().GetItem(Player.myPlayer.playerObject.GetComponent<InventoryManager>().selectedSlot, true);
        }
    }

    public void Initialize(InventoryItem newInventoryItem)
    {
        uiManager = Player.myPlayer.playerObject.GetComponent<UiManager>();
        inventoryManager = Player.myPlayer.playerObject.GetComponent<InventoryManager>();
    }

    public void Deinitialize()
    {

    }
}

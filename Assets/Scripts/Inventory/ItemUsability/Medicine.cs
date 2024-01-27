using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviourPunCallbacks, IUsableItem
{
    private InventoryItem inventoryItem;
    public Medical item;

    public AudioClip useSound;

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
    }

    public void Update() 
    {
        if (Player.myPlayer.playerObject.GetComponent<UiManager>().somePanelTurnedOn) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Heal();
        }
    }

    public void Heal()
    {
        Player.myPlayer.playerObject.GetComponent<PlayerAudioSource>().PlaySound(useSound);
        Player.myPlayer.playerObject.GetComponent<HealthSystem>().AddHealth(item.healthToAdd);
        Player.myPlayer.playerObject.GetComponent<InventoryManager>().GetItem(Player.myPlayer.playerObject.GetComponent<InventoryManager>().selectedSlot, true);
    }

    public void Deinitialize()
    {

    }
}

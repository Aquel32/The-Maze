using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviourPunCallbacks, IUsableItem
{
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
        
    }

    public void Update() 
    {
        if (UiManager.Instance.somePanelTurnedOn) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Heal();
        }
    }

    public void Heal()
    {
        //Player.myPlayer.playerObject.GetComponent<PlayerAudioSource>().PlaySound(useSound);
        //Player.myPlayer.playerObject.GetComponent<HealthSystem>().AddHealth(item.healthToAdd);
        InventoryManager.Instance.GetItem(InventoryManager.Instance.selectedSlot, true);
    }

    public void Deinitialize()
    {

    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingOpener : MonoBehaviourPunCallbacks, IInteractible, IDamageable
{
    public GameObject TableHandler;

    public void Damage(int damage, ToolType toolType)
    {
        PhotonNetwork.Instantiate(TableHandler.name, transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(this.gameObject);
    }
    public void Interact()
    {
        Player.myPlayer.playerObject.GetComponent<InventoryManager>().ChangeUIState(AdditionalPanelType.Crafting);
    }
}


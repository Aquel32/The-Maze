using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilOpener : MonoBehaviourPunCallbacks, IInteractible
{
    public void Interact()
    {
        Player.myPlayer.playerObject.GetComponent<InventoryManager>().ChangeUIState(AdditionalPanelType.Anvil);
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTableOpener : MonoBehaviourPunCallbacks, IInteractible
{
    public void Interact()
    {
        Player.myPlayer.playerObject.GetComponent<InventoryManager>().uiManager.ChangeCurrentPanel(Panels.Research);
        Player.myPlayer.playerObject.GetComponent<ExperienceSystem>().UpdateExperienceText();
    }
}

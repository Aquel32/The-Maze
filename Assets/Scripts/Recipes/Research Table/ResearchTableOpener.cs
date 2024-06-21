using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTableOpener : MonoBehaviourPunCallbacks, IInteractible
{
    public void Interact()
    {
        UiManager.Instance.ChangeCurrentPanel(Panels.Research);
        ExperienceSystem.Instance.UpdateExperienceText();
    }
}

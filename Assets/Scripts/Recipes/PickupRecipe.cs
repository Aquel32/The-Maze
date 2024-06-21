using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRecipe : MonoBehaviourPunCallbacks, IInteractible
{
    public Recipe recipe;

    public void Interact()
    {
        RecipeManager.Instance.UnlockRecipe(recipe);
        PhotonNetwork.Destroy(this.gameObject);
    }
}

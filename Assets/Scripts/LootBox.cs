using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviourPunCallbacks, IInteractible
{
    public Item[] possibleLoot;
    public GameObject pickupRecipe;

    bool isOpened;
    Animator animator;

    [SerializeField] private bool type;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        isOpened = false;
    }

    public void Interact()
    {
        if (isOpened == true) return;

        photonView.RPC("OpenChestRPC", RpcTarget.AllBuffered);
        SpawnRandomLoot();
    }

    [PunRPC]
    public void OpenChestRPC()
    {
        isOpened = true;
        animator.SetBool("IsOpened", isOpened);
    }

    public void SpawnRandomLoot()
    {
        if (type == false)
        {
            Item itemToSpawn = possibleLoot[Random.Range(0, possibleLoot.Length)];
            PhotonNetwork.Instantiate(itemToSpawn.handlerPrefab.name, transform.position + Vector3.up * 3, Quaternion.identity);
        }
        else
        {
            int recipeId = Random.Range(0, GameManager.Instance.recipeList.Count);
            GameObject go = PhotonNetwork.Instantiate(pickupRecipe.name, transform.position + Vector3.up * 3, Quaternion.identity);
            photonView.RPC("SetRecipeToPickup", RpcTarget.AllBuffered, recipeId, go.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    public void SetRecipeToPickup(int recipeId, int goviewid)
    {
        PhotonView.Find(goviewid).GetComponent<PickupRecipe>().recipe = GameManager.Instance.recipeList[recipeId];
    }

}

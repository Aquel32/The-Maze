using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Chest : MonoBehaviourPunCallbacks, IInteractible
{
    public Item[] possibleLoot;
    public Transform lootPlace;

    bool isOpened;
    Animator animator;

    AudioSource audioSource;
    public AudioClip openSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
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
        audioSource.PlayOneShot(openSound);
        isOpened = true;
        animator.SetBool("IsOpened", isOpened);
    }

    public void SpawnRandomLoot()
    {
        int lootIndex = Random.Range(0, possibleLoot.Length);
        Item itemToSpawn = possibleLoot[lootIndex];

        PhotonNetwork.Instantiate(itemToSpawn.handlerPrefab.name, lootPlace.position, Quaternion.identity);
    }

}

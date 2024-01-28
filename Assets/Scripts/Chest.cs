using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Chest : MonoBehaviourPunCallbacks, IInteractible, IDamageable
{
    [SerializeField] private GameObject chestItemHandlerPrefab;

    private AudioSource audioSource;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip sound;
    private bool animationState;

    void Start()
    {
        animationState = false;
        audioSource = GetComponent<AudioSource>();
    }

    public void Damage(int damage, ToolType toolType)
    {
        PhotonNetwork.Instantiate(chestItemHandlerPrefab.name, transform.position, Quaternion.identity);

        //DROP ALL ITEMS THAT ARE INSIDE

        PhotonNetwork.Destroy(this.gameObject);
    }

    public void Interact()
    {
        animationState = !animationState;
        photonView.RPC("ChangeAnimationStateRPC", RpcTarget.AllBuffered, animationState);
        if (animationState == true)
        {
            //Player.myPlayer.playerObject.GetComponent<InventoryManager>().ChangeUIState(AdditionalPanelType.Chest);
        }
    }

    [PunRPC]
    public void ChangeAnimationStateRPC(bool newState)
    {
        audioSource.PlayOneShot(sound);
        animationState = newState;
        animator.SetBool("IsOpened", animationState);
    }
}

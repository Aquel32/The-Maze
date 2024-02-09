using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Chest : MonoBehaviourPunCallbacks, IInteractible
{
    private AudioSource audioSource;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip sound;
    public bool animationState;

    [SerializeField] private GameObject canvas;

    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    void Start()
    {
        animationState = false;
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        animationState = !animationState;
        photonView.RPC("ChangeAnimationStateRPC", RpcTarget.AllBuffered, animationState);
        canvas.gameObject.SetActive(animationState);
        if (animationState == true)
        {
            Player.myPlayer.playerObject.GetComponent<InventoryManager>().ChangeUIState(AdditionalPanelType.Chest);
            Player.myPlayer.playerObject.GetComponent<UiManager>().SetChestPanel(canvas);
        }
        else
        {
            Player.myPlayer.playerObject.GetComponent<InventoryManager>().ChangeUIState(AdditionalPanelType.None);
        }
    }

    [PunRPC]
    public void ChangeAnimationStateRPC(bool newState)
    {
        audioSource.PlayOneShot(sound);
        animator.SetBool("IsOpened", animationState);
    }
}

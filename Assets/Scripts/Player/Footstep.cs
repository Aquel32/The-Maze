using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footstep : MonoBehaviourPunCallbacks
{
    AudioSource audioSource;
    public AudioClip[] possibleClips;
    public float timeBetweenFootsteps = 0.3f;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(FootstepsSoundCycle());
    }

    IEnumerator FootstepsSoundCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenFootsteps);
            if (playerMovement.grounded && playerMovement.horizontalInput != 0 || playerMovement.grounded && playerMovement.verticalInput != 0) PlayFootstepSound();
        }
    }

    public void PlayFootstepSound()
    {
        int clip = Random.Range(0, possibleClips.Length);
        photonView.RPC("PlayFootstepSoundRPC", RpcTarget.AllBuffered, clip);
    }

    [PunRPC]
    public void PlayFootstepSoundRPC(int clip, PhotonMessageInfo pmi)
    {
        Player.FindPlayer(pmi.Sender).playerObject.GetComponent<AudioSource>().PlayOneShot(possibleClips[clip]);
    }
}

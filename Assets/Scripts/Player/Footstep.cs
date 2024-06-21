using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footstep : MonoBehaviourPunCallbacks
{
    private AudioSource audioSource;
    public AudioClip[] possibleClips;
    public float timeBetweenFootsteps = 0.3f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(FootstepsSoundCycle());
    }

    IEnumerator FootstepsSoundCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenFootsteps);
            if (PlayerMovement.Instance.grounded && PlayerMovement.Instance.horizontalInput != 0 || PlayerMovement.Instance.grounded && PlayerMovement.Instance.verticalInput != 0) PlayFootstepSound();
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

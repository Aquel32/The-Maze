using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TimeManager : MonoBehaviourPunCallbacks
{
    public static TimeManager Instance;
    public void Awake() { Instance = this; }

    public float time;
    public int dayLenght;
    [SerializeField] private Transform sun;

    public float timeSpeed;

    [SerializeField] private Animator doorAnimator;

    private float timeMultiplier;

    private void Start()
    {
        if (PhotonNetwork.IsConnected == false) return;

        photonView.RPC("AskForTimeRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void AskForTimeRPC(PhotonMessageInfo pmi)
    {
        photonView.RPC("SyncTimeRPC", pmi.Sender, time);
    }

    [PunRPC]
    public void SyncTimeRPC(float newTime)
    {
        time = newTime;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected) return;

        timeMultiplier = 1f / (dayLenght / 24f);

        time += Time.deltaTime * timeSpeed * timeMultiplier;
        time %= 24;

        sun.transform.localRotation = Quaternion.Euler(new Vector3((time / 24f * 360f) - 90f, 170f, 0));
        doorAnimator.SetFloat("Time", time);
    }

    public void SetTime(float newTime)
    {
        photonView.RPC("SetTimeRPC", RpcTarget.AllBuffered, newTime);

    }
    [PunRPC]    
    public void SetTimeRPC(float newTime)
    {
        time = newTime;
    }

    [PunRPC]
    public void SwitchAnimationRPC(bool state)
    {
        doorAnimator.SetBool("IsOpen", state);
    }
}

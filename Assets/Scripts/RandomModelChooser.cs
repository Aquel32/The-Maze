using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModelChooser : MonoBehaviourPunCallbacks
{
    public GameObject[] possibleModels;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InventoryManager inventoryManager;

    void Start()
    {
        int index = UnityEngine.Random.Range(0, possibleModels.Length);
        GameObject model = PhotonNetwork.Instantiate(possibleModels[index].name, transform.position, Quaternion.identity);

        photonView.RPC("SetModelForOtherPlayers", RpcTarget.AllBuffered, model.GetComponent<PhotonView>().ViewID);

        foreach(Transform child in model.transform)
        {
            if (child.GetComponent<SkinnedMeshRenderer>() != null) child.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        inventoryManager.handBoneTarget = model.transform.Find("Rig 1").Find("RightHandTarget");
    }

    [PunRPC]
    public void SetModelForOtherPlayers(int modelViewId, PhotonMessageInfo pmi)
    {
        Transform modelParent = Player.FindPlayer(pmi.Sender).playerObject.transform.Find("Model");
        GameObject model = PhotonView.Find(modelViewId).gameObject;
        model.transform.SetParent(modelParent, true);
        Player.FindPlayer(pmi.Sender).playerObject.GetComponent<PlayerMovement>().animator = model.GetComponent<Animator>();
    }
}

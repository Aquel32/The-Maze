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
    [SerializeField] private ArmorSystem armorSystem;


    void Start()
    {
        int index = UnityEngine.Random.Range(0, possibleModels.Length);
        GameObject model = PhotonNetwork.Instantiate(possibleModels[index].name, transform.position, Quaternion.identity);

        photonView.RPC("SetModelForOtherPlayers", RpcTarget.AllBuffered, model.GetComponent<PhotonView>().ViewID);

        foreach(Transform child in model.transform)
        {
            if (child.GetComponent<SkinnedMeshRenderer>() != null) child.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        string bonesParent = "Female";
        if(model.transform.Find("Male") != null) { bonesParent = "Male"; }

        inventoryManager.handBoneTarget = model.transform.Find("Rig 1").Find("RightHandTarget");

        armorSystem.chestBone = model.transform.Find(bonesParent).Find("LowManHips").Find("LowManSpine").Find("LowManSpine1");
        armorSystem.headBone = model.transform.Find(bonesParent).Find("LowManHips").Find("LowManSpine").Find("LowManSpine1").Find("LowManSpine2").Find("LowManNeck").Find("LowManHead").Find("LowManHead_end");
        armorSystem.RightLegBone = model.transform.Find(bonesParent).Find("LowManHips").Find("LowManRightUpLeg").Find("LowManRightLeg");
        armorSystem.LeftLegBone = model.transform.Find(bonesParent).Find("LowManHips").Find("LowManLeftUpLeg").Find("LowManLeftLeg");
        armorSystem.RightFootBone = model.transform.Find(bonesParent).Find("LowManHips").Find("LowManRightUpLeg").Find("LowManRightLeg").Find("LowManRightFoot").Find("LowManRightToeBase");
        armorSystem.LeftFootBone = model.transform.Find(bonesParent).Find("LowManHips").Find("LowManLeftUpLeg").Find("LowManLeftLeg").Find("LowManLeftFoot").Find("LowManLeftToeBase");
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

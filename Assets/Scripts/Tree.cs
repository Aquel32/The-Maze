using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviourPunCallbacks, IDamageable
{
    public GameObject stumpObject, mainTreeObject;
    public GameObject woodItemPrefab;
    int health = 100;
    public int howManyWoodItemsDrop = 3;

    public bool state = false;

    public void Damage(int damage)
    {
        if(!state) photonView.RPC("CutTreeRPC", RpcTarget.AllBuffered, damage);
    }

    [PunRPC]
    public void CutTreeRPC(int damage)
    {
        health -= damage;
        if (health > 0) return;

        print("Tree was cutted");

        for (int i = 0; i < howManyWoodItemsDrop; i++) PhotonNetwork.Instantiate(woodItemPrefab.name, transform.position + Vector3.up, Quaternion.identity);

        mainTreeObject.SetActive(false);
        stumpObject.SetActive(true);

        state = true;
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(cooldownToDestroy());
    }

    IEnumerator cooldownToDestroy()
    {
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }
}

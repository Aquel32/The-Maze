using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviourPunCallbacks, IDamageable
{
    public GameObject oreItemPrefab;
    int health = 100;
    public int howManyItemDrops = 3;

    public void Damage(int damage)
    {
        photonView.RPC("HitOreRPC", RpcTarget.AllBuffered, damage);

        if(health <= 0)
        {
            for (int i = 0; i < howManyItemDrops; i++)
            {
                PhotonNetwork.Instantiate(oreItemPrefab.name, transform.position + Vector3.up, Quaternion.identity);
            }
            PhotonNetwork.Destroy(this.gameObject);
        }
        
    }

    [PunRPC]
    public void HitOreRPC(int damage)
    {
        print("Ore was hitted");

        health -= damage;
    }

}

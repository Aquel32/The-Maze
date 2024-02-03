using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviourPunCallbacks, IDamageable
{
    public GameObject dropPrefab;

    public void Damage(int damage, ToolType toolType)
    {
        PhotonNetwork.Instantiate(dropPrefab.name, transform.position, Quaternion.identity);
        photonView.RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void DestroyRPC()
    {
        
        Destroy(this.gameObject);
    }
}

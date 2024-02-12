using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviourPunCallbacks, IDamageable
{
    public Item ore;
    public int health = 100;
    public int howManyItemDrops = 3;

    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("HitOreRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Ore));

        if(health <= 0)
        {
            for (int i = 0; i < howManyItemDrops; i++)
            {
                PhotonNetwork.Instantiate(ore.handlerPrefab.name, transform.position + Vector3.up, Quaternion.identity);
            }
            PhotonNetwork.Destroy(this.gameObject);
            
            MaterialOre m_ore = ore as MaterialOre;
            if(m_ore != null) Player.myPlayer.playerObject.GetComponent<ExperienceSystem>().ChangeExperience(m_ore.experiencePoints);
        }
        
    }

    [PunRPC]
    public void HitOreRPC(int damage)
    {
        print("Ore was hitted");

        health -= damage;
    }

}

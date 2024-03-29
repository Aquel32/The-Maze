using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : HealthBarHandler, IDamageable
{
    public Item ore;
    public int health;
    private int maxHealth;
    public int howManyItemDrops = 3;
    public int experience;
    [SerializeField] private GameObject healthBarPrefab;

    void Start()
    {
        maxHealth = health;
    }

    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("HitOreRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Ore));

        if (health <= 0)
        {
            for (int i = 0; i < howManyItemDrops; i++)
            {
                PhotonNetwork.Instantiate(ore.handlerPrefab.name, transform.position + Vector3.up, Quaternion.identity);
            }
            
            Player.myPlayer.playerObject.GetComponent<ExperienceSystem>().ChangeExperience(experience);
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            UpdateHealthBar(health, maxHealth, healthBarPrefab);
        }
    }

    [PunRPC]
    public void HitOreRPC(int damage)
    {
        health -= damage;
    }
}

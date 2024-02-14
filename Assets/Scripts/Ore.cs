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
    [SerializeField] private GameObject healthBarPrefab;

    void Start()
    {
        maxHealth = health;
    }

    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("HitOreRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Ore));

        MaterialOre m_ore = ore as MaterialOre;

        if (health <= 0)
        {
            for (int i = 0; i < howManyItemDrops; i++)
            {
                PhotonNetwork.Instantiate(ore.handlerPrefab.name, transform.position + Vector3.up, Quaternion.identity);
            }
            PhotonNetwork.Destroy(this.gameObject);
            
            if(m_ore != null) Player.myPlayer.playerObject.GetComponent<ExperienceSystem>().ChangeExperience(m_ore.experiencePoints);
        }
        else
        {
            UpdateHealthBar(health, maxHealth, healthBarPrefab);
        }
    }

    [PunRPC]
    public void HitOreRPC(int damage)
    {
        print("Ore was hitted");

        health -= damage;
    }

}

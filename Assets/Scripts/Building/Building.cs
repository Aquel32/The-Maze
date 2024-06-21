using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : HealthBarHandler, IDamageable
{
    [SerializeField] private Build build;
    [SerializeField] private int health;
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private GameObject healthBarPrefab;

    void Start()
    {
        health = build.health;
    }

    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("DamageRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Building));

        if (health <= 0)
        {
            PhotonNetwork.Instantiate(dropPrefab.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            UpdateHealthBar(health, build.health, healthBarPrefab);
        }
    }

    [PunRPC]
    public void DamageRPC(int damage)
    {
        health -= damage;
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : HealthBarHandler, IDamageable
{
    [SerializeField] private int health = 100;
    private int maxHealth = 100;

    [SerializeField] private GameObject[] dropsPrefabs;

    [SerializeField] private GameObject[] possibleModels;
    [SerializeField] private GameObject stumpObject;
    private GameObject mainTreeObject;

    private bool state = true;

    [SerializeField] private GameObject healthBarPrefab;

    void Start()
    {
        maxHealth = health;
        mainTreeObject = Instantiate(possibleModels[Random.Range(0, possibleModels.Length)], transform);
    }

    public void Damage(int damage, ToolType toolType)
    {
        if(state) photonView.RPC("HitTreeRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Tree));
        if (!state)
        {
            for(int i = 0; i < dropsPrefabs.Length; i++)
            {
                PhotonNetwork.Instantiate(dropsPrefabs[i].name, transform.position + Vector3.up, Quaternion.identity);
            }
        }
        else
        {
            UpdateHealthBar(health, maxHealth, healthBarPrefab);
        }
    }

    [PunRPC]
    public void HitTreeRPC(int damage)
    {
        health -= damage;
        if (health > 0) return;

        mainTreeObject.SetActive(false);
        stumpObject.SetActive(true);

        state = false;
        GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(cooldownToDestroy());
    }

    IEnumerator cooldownToDestroy()
    {
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }
}

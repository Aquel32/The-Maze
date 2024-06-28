using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PhotonView))]
public class Animal : HealthBarHandler, IDamageable
{
    [SerializeField] private Mob mob;

    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private int health;
    private int maxHealth;

    [HideInInspector] public Herd herd;
    [SerializeField] private GameObject healthBarPrefab;

    private void Start()
    {
        health = mob.health;
        maxHealth = health;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(movementCycle());
        }
    }

    void Update()
    {
        animator.SetBool("Moving", (agent.velocity.x != 0));
    }

    IEnumerator movementCycle()
    {
        while (true)
        {
            photonView.RPC("SetDestinationRPC", RpcTarget.All, transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
            yield return new WaitForSeconds(Random.Range(3, 15));
        }
    }

    [PunRPC]
    public void SetDestinationRPC(Vector3 position)
    {
        agent.SetDestination(position);
    }


    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("HitRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Mob));

        if(health <= 0)
        {
            for (int i = 0; i < mob.drops.Length; i++)
            {
                PhotonNetwork.Instantiate(mob.drops[i].handlerPrefab.name, transform.position, Quaternion.identity);
            }

            ExperienceSystem.Instance.ChangeExperience(mob.experiencePoints);
            photonView.RPC("KillRPC", RpcTarget.AllBuffered);
        }
        else
        {
            UpdateHealthBar(health, maxHealth, healthBarPrefab);
        }
    }

    [PunRPC]
    public void HitRPC(int damage)
    {
        health -= damage;

        if(health <= 0 && PhotonNetwork.IsMasterClient && herd != null)
        {
            herd.animals.Remove(gameObject);
        }
    }

    [PunRPC]
    public void KillRPC()
    {
        Destroy(gameObject);
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(PhotonView))]
public class Enemy : HealthBarHandler, IDamageable
{
    [SerializeField] private Hostile mob;

    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private int health;
    private int maxHealth;
    [HideInInspector] public Herd herd;

    [SerializeField] private Transform target;
    [SerializeField] private GameObject healthBarPrefab;

    private void Start()
    {
        health = mob.health;
        maxHealth = health;
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();

        GetComponent<SphereCollider>().radius = mob.visionRange;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(movementCycle());
        }

    }

    void Update()
    {
        //animator.SetBool("Moving", (agent.velocity.x != 0));
    }

    IEnumerator movementCycle()
    {
        while (true)
        {
            Vector3 position;
            if (target == null)
            {
                position = transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            }
            else
            {
                position = target.position;

                if (Vector3.Distance(transform.position, target.position) < mob.attackRange)
                {
                    if (target.TryGetComponent<IDamageable>(out IDamageable idamageable))
                    {
                        idamageable.Damage(mob.damage, ToolType.Sword);
                    }
                }
            }

            photonView.RPC("SetDestinationRPC", RpcTarget.All, position);

            yield return new WaitForSeconds(1);
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

        if (health <= 0)
        {
            for (int i = 0; i < mob.drops.Length; i++)
            {
                PhotonNetwork.Instantiate(mob.drops[i].handlerPrefab.name, transform.position, Quaternion.identity);
            }

            Player.myPlayer.playerObject.GetComponent<ExperienceSystem>().ChangeExperience(mob.experiencePoints);
            PhotonNetwork.Destroy(this.gameObject);
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.gameObject.layer == 6)
        {
            target = other.transform;
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.gameObject.layer == 6)
        {
            target = null;
            photonView.RPC("SetDestinationRPC", RpcTarget.All, transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
        }
    }
}

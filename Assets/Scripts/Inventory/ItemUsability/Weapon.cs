using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks, IUsableItem
{
    private InventoryItem inventoryItem;
    public Tool item;
    public bool cooldown;

    public AudioSource audioSource;
    public AudioClip attackSound;


    void Start()
    {
        this.enabled = photonView.IsMine;
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(InventoryItem newInventoryItem)
    {
        inventoryItem = newInventoryItem;
    }

    public void Update()
    {
        if (UiManager.Instance.somePanelTurnedOn) return;

        if(Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            Attack();
        }
    }
    public void Attack()
    {
        if (cooldown) return;

        print("Attacked with " + item.name);

        Ray ray = new Ray(InventoryManager.Instance.playerCamera.position, InventoryManager.Instance.playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, item.effectiveRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IDamageable damageObj))
            {
                damageObj.Damage(item.damage, item.toolType);

                if (item.haveDurability)
                {
                    int customData = int.Parse(inventoryItem.customData);
                    customData -= 1;
                    inventoryItem.customData = customData.ToString();
                    if (customData <= 0) InventoryManager.Instance.GetItem(InventoryManager.Instance.selectedSlot, true);
                }
            }
        }

        photonView.RPC("PlaySoundRPC", RpcTarget.All);
        //StartCoroutine(WeaponAnimation("IsAttacking"));
        StartCoroutine(Cooldown());
    }

    public IEnumerator WeaponAnimation(string animation)
    {
        photonView.RPC("AnimationRPC", RpcTarget.All, animation, true);
        yield return new WaitForSeconds(0.1f);
        photonView.RPC("AnimationRPC", RpcTarget.All, animation, false);
    }

    public IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(item.cooldownTime);
        cooldown = false;
    }

    public void Deinitialize()
    {

    }

    [PunRPC]
    public void PlaySoundRPC()
    {
        audioSource.PlayOneShot(attackSound);
    }
}

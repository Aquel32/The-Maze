using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorOnBody : MonoBehaviourPunCallbacks, IDamageable
{
    public Armor armor;
    public InventoryItem inventoryItem;

    public Player owner;
    public ArmorSystem armorSystem;
    public int slotId;

    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("DamageRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Mob));
    }

    [PunRPC]
    public void DamageRPC(int damage)
    {
        print(gameObject.name);

        if (owner.photonPlayer == Player.myPlayer.photonPlayer)
        {
            int health = int.Parse(inventoryItem.customData) - damage;
            inventoryItem.customData = health.ToString();

            if(health <= 0)
            {
                InventoryItem tempItem = inventoryItem;

                inventoryItem.currentSlot.currentInventoryItem = null;
                inventoryItem.currentSlot = null;
                Destroy(tempItem.gameObject);

                armorSystem.LookForChanges();
            }
        }
    }

}

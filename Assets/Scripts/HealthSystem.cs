using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamageable
{
    public void Damage(int damage, ToolType toolType);
}

public class HealthSystem : MonoBehaviourPunCallbacks, IDamageable
{
    public int health;
    public Slider slider;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();

        ResetHealth();
        RefreshCounter();
    }

    public void ResetHealth()
    {
        health = 100;
    }

    public void AddHealth(int healthToAdd)
    {
        health += healthToAdd;
        RefreshCounter();
        CheckMaxAndMinHealth();
    }

    public void RemoveHealth(int healthToRemove)
    {
        health -= healthToRemove;
        RefreshCounter();
        CheckMaxAndMinHealth();

        if(health == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        print("You died!");

        inventoryManager.DropAllItems();

        GameManager.Instance.DespawnPlayer(Player.myPlayer);
        GameManager.Instance.SpawnPlayer(Player.myPlayer);
    }

    public void RefreshCounter()
    {
        slider.value = health;
    }

    public void CheckMaxAndMinHealth()
    {
        if (health > 100) { health = 100; RefreshCounter(); }
        if (health < 0) { health = 0; RefreshCounter(); }
    }

    public void Damage(int damage, ToolType toolType)
    {
        photonView.RPC("DamageRPC", RpcTarget.AllBuffered, DamageBuffer.instance.BufferDamage(damage, toolType, TargetType.Mob), GetComponent<PlayerReference>().referencedPlayer.photonPlayer);
    }

    [PunRPC]
    public void DamageRPC(int damage, Photon.Realtime.Player targetPlayer)
    {
        if (targetPlayer == Player.myPlayer.photonPlayer)
        {
            RemoveHealth(damage);
        }
    }

}

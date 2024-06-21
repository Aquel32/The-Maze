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

public class HealthSystem : MonoBehaviourPunCallbacks
{
    public static HealthSystem Instance;
    public void Awake() { Instance = this; }

    public int health;
    public int armor;
    public int armorMaxValue;
    public Slider healthSlider;
    public Slider armorSlider;

    private void Start()
    {
        ResetHealth();
        RefreshCounter();
    }

    public void ResetHealth()
    {
        health = 100;
        armor = 0;
    }

    public void AddHealth(int healthToAdd)
    {
        health += healthToAdd;
        CheckMaxAndMinHealth();
        RefreshCounter();
    }

    public void RemoveHealth(int healthToRemove)
    {
        armor -= healthToRemove;
        int armorDamage = healthToRemove;

        if (armor < 0)
        {
            health -= -armor;
            armorDamage = -armor;
        }

        ArmorSystem.Instance.HitArmor(armorDamage);

        CheckMaxAndMinHealth();
        RefreshCounter();

        if(health == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        print("You died!");

        InventoryManager.Instance.DropAllItems();

        GameManager.Instance.DespawnPlayer(Player.myPlayer);
        GameManager.Instance.SpawnPlayer(Player.myPlayer);
    }

    public void RefreshCounter()
    {
        healthSlider.value = health;
        armorSlider.maxValue = armorMaxValue;
        armorSlider.value = armor;
        armorSlider.gameObject.SetActive(armor > 0);
    }

    public void CheckMaxAndMinHealth()
    {
        if (health > 100) { health = 100; RefreshCounter(); }
        if (health < 0) { health = 0; RefreshCounter(); }
    }

    

}

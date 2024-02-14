using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviourPunCallbacks
{
    private GameObject healthBar;

    public void UpdateHealthBar(int newHealth, int maxHealth, GameObject healthBarPrefab)
    {
        if(healthBar == null)
        {
            healthBar = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity, this.transform);
            healthBar.transform.localPosition = Vector3.zero + Vector3.up * 1.5f;
            healthBar.GetComponentInChildren<Slider>().maxValue = maxHealth;
        }

        healthBar.GetComponentInChildren<Slider>().value = newHealth;

        StopAllCoroutines();
        StartCoroutine(barCoroutine());
    }

    public IEnumerator barCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(healthBar);
        healthBar = null;
    }
}

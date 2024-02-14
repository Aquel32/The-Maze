using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform target;

    void Start()
    {
        target = Player.myPlayer.playerObject.transform;
    }

    void Update()
    {
        transform.LookAt(target.position);
    }
}

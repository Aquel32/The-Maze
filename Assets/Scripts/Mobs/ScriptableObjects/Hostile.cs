using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Mob/Hostile")]
public class Hostile : Mob
{
    [Header("Hostile")]
    public int damage;
    public int visionRange;
    public int attackRange;
}

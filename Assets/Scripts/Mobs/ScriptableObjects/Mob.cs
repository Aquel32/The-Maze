using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Mob/Mob")]
public class Mob : ScriptableObject
{
    [Header("General")]
    public string mobName;
    public int health;
    public MobType mobType;
    public Item[] drops;
    public GameObject prefab;
    public int experiencePoints;
}

public enum MobType { Hostile, Friendly }
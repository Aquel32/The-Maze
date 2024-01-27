using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class DamageBuffer : MonoBehaviour
{
    [System.Serializable]
    public class TargetLists
    {
        public List<float> targetList;
    }

    public List<TargetLists> settings;

    public static DamageBuffer instance;

    private void Start()
    {
        instance = this;
    }

    public int BufferDamage(int damage, ToolType toolType, TargetType targetType)
    {
        float endDamage = damage;

        float multiplier = settings[(int)targetType].targetList[(int)toolType];
        endDamage *= multiplier;

        print((int)targetType + " | " + (int)toolType + " | " + multiplier);

        return (int)endDamage;
    }
}


public enum TargetType { Ore, Tree, Mob, Building, Other }